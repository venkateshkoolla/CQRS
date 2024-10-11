using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Clients;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.Apply.Services.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Data.Extras;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;
using ValidationException = Ocas.Common.Exceptions.ValidationException;

namespace Ocas.Domestic.Apply.Services.Handlers
{
    public class PayOrderHandler : IRequestHandler<PayOrder, FinancialTransaction>
    {
        private readonly ILookupsCacheBase _lookupsCache;
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IDomesticContextExtras _domesticContextExtras;
        private readonly IUserAuthorizationBase _userAuthorization;
        private readonly IApiMapperBase _apiMapper;
        private readonly IMonerisClient _monerisClient;
        private readonly string _sourcePartner;

        public PayOrderHandler(ILogger<PayOrderHandler> logger, IDomesticContext domesticContext, IUserAuthorizationBase userAuthorization, ILookupsCacheBase lookupsCache, IApiMapperBase apiMapper, IMonerisClient monerisClient, IDomesticContextExtras domesticContextExtras, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _monerisClient = monerisClient ?? throw new ArgumentNullException(nameof(monerisClient));
            _domesticContextExtras = domesticContextExtras ?? throw new ArgumentNullException(nameof(domesticContextExtras));
            _sourcePartner = requestCache.Get<string>(Constants.RequestCacheKeys.Partner);
        }

        public async Task<FinancialTransaction> Handle(PayOrder request, CancellationToken cancellationToken)
        {
            var applicationCycleStatuses = await _lookupsCache.GetApplicationCycleStatuses(Constants.Localization.EnglishCanada);
            var activeCycleStatusId = applicationCycleStatuses.First(x => x.Code == Constants.ApplicationCycleStatuses.Active).Id;

            var order = await _domesticContext.GetOrder(request.OrderId) ?? throw new NotFoundException($"Order not found: {request.OrderId}");

            var completedSteps = order.ApplicantId.HasValue ? await _domesticContext.GetCompletedStep(order.ApplicantId.Value) : null;
            if (completedSteps is null || completedSteps < CompletedSteps.Experience)
            {
                throw new ValidationException(ErrorCodes.Applicant.IncompleteError, "Applicant profile is not complete");
            }

            if (order.ApplicantId is null)
                throw new ValidationException($"No ApplicantId on Order: {order.Id}");

            if (order.ApplicationId is null)
                throw new ValidationException($"No ApplicationId on Order: {order.Id}");

            var application = await _domesticContext.GetApplication(order.ApplicationId.Value) ?? throw new NotFoundException($"Application not found: {order.ApplicationId}");

            if (application.Id != order.ApplicationId.Value || order.ApplicantId != application.ApplicantId)
            {
                throw new NotAuthorizedException();
            }

            await _userAuthorization.CanAccessApplicantAsync(request.User, order.ApplicantId.Value);

            if (application.CompletedSteps is null || application.CompletedSteps.Value < (int)ApplicationCompletedSteps.TranscriptRequests)
            {
                throw new ValidationException($"Application {application.Id} is incomplete");
            }

            if (order.OrderPaymentStatus == OrderPaymentStatus.Paid)
                throw new ValidationException($"Order has already been paid: {order.Id}");

            if (order.OrderPaymentStatus == OrderPaymentStatus.CheckedOut)
                throw new ValidationException($"Order has already been checked out for payment: {order.Id}");

            if (order.OrderPaymentStatus == OrderPaymentStatus.Cancelled)
                throw new ValidationException($"Order is cancelled: {order.Id}");

            if (order.ApplicationCycleStatusId != activeCycleStatusId)
                throw new ValidationException($"Order must be for an active cycle: {order.ApplicationCycleStatusId}");

            var contact = await _domesticContext.GetContact(order.ApplicantId.Value) ?? throw new NotFoundException($"Applicant {order.ApplicantId} not found");

            if (contact.PaymentLocked)
                throw new ValidationException($"Applicant {order.ApplicantId} is payment locked");

            var modifiedBy = request.User.GetUpnOrEmail();

            // if this is a Zero-dollar payment, update the pending TRs before completing the payment
            if (order.FinalTotal == 0)
            {
                await _domesticContext.BeginTransaction();
                try
                {
                    try
                    {
                        await ActivateTranscriptRequests(order, modifiedBy);
                    }
                    catch
                    {
                        // swallow the exception because:
                        //  a) we don't want to block payment
                        //  b) updating the TR status is not required, just nice to have on the UI
                    }

                    if (!request.IsOfflinePayment)
                    {
                        order = await _domesticContextExtras.WriteDetailsToOrder(
                        contact,
                        application,
                        order,
                        modifiedBy,
                        await _lookupsCache.GetPaymentMethodsDto(Constants.Localization.EnglishCanada));
                    }
                    else
                    {
                        order = await _domesticContextExtras.WriteDetailsToOrder(
                        contact,
                        application,
                        order,
                        modifiedBy,
                        await _lookupsCache.GetPaymentMethodsDto(Constants.Localization.EnglishCanada),
                        false,
                        request.OfflinePaymentInfo.PaymentMethodId,
                        request.OfflinePaymentInfo.Notes,
                        request.OfflinePaymentInfo.Amount);
                    }

                    await _domesticContext.CommitTransaction();
                }
                catch
                {
                    try
                    {
                        // TODO: when transactions are supported, uncomment call to RollbackTransaction
                        // TODO: for now, we wont call RollbackTransaction because it will just re-throw the given exception
                        // _domesticContext.RollbackTransaction(receiptException);

                        // TODO: when transactions are supported, delete this call to RollbackTranscriptRequests
                        await RollbackTranscriptRequests(order, modifiedBy);
                    }
                    catch
                    {
                        // TODO: when transactions are supported, handle exception thrown from RollbackTransaction
                    }

                    throw;
                }
            }
            else if (!request.IsOfflinePayment)
            {
                order = await _domesticContextExtras.WriteDetailsToOrder(
                contact,
                application,
                order,
                modifiedBy,
                await _lookupsCache.GetPaymentMethodsDto(Constants.Localization.EnglishCanada));
            }
            else if (request.IsOfflinePayment)
            {
                order = await _domesticContextExtras.WriteDetailsToOrder(
                contact,
                application,
                order,
                modifiedBy,
                await _lookupsCache.GetPaymentMethodsDto(Constants.Localization.EnglishCanada),
                false,
                request.OfflinePaymentInfo.PaymentMethodId,
                request.OfflinePaymentInfo.Notes,
                request.OfflinePaymentInfo.Amount);
            }

            if (order.FinalTotal > 0 && !request.IsOfflinePayment)
            {
                var customerId = contact.AccountNumber + order.ApplicationNumber;

                var chargeCardResult = await _monerisClient.ChargeCard(
                        request.PayOrderInfo.CardNumberToken,
                        order.FinalTotal,
                        customerId,
                        order.OrderNumber,
                        contact.Email,
                        request.PayOrderInfo.Csc,
                        request.PayOrderInfo.ExpiryDate,
                        request.PayOrderInfo.CardHolderName);

                if (!chargeCardResult.ChargeSuccess)
                {
                    // update order status to Cancelled when order is not Paid
                    if (order.OrderPaymentStatus != OrderPaymentStatus.Paid)
                    {
                        order.OrderPaymentStatus = OrderPaymentStatus.Cancelled;
                        order.OrderPaymentType = OrderPaymentType.Online;
                        order.Note = chargeCardResult.ReceiptBase.Message;
                        order.PaymentResponseCode = chargeCardResult.ReceiptBase.ResponseCode?.Substring(0, 3);
                        order.OrderConfirmationNumber = "0";
                        order.ModifiedBy = modifiedBy;

                        await _domesticContext.UpdateOrder(order);
                    }

                    throw new ValidationException("Card declined");
                }

                await _domesticContext.BeginTransaction();
                try
                {
                    // update the pending TRs before completing the payment
                    try
                    {
                        await ActivateTranscriptRequests(order, modifiedBy);
                    }
                    catch
                    {
                        // swallow the exception because:
                        //  a) we don't want to block payment
                        //  b) updating the TR status is not required, just nice to have on the UI
                    }

                    // complete payment
                    await _domesticContext.CreatePaymentTransactionDetail(chargeCardResult.ReceiptBase);

                    await _domesticContext.CommitTransaction();
                }
                catch (Exception receiptException)
                {
                    try
                    {
                        // TODO: when transactions are supported, uncomment call to RollbackTransaction
                        // TODO: for now, we wont call RollbackTransaction because it will just re-throw the given exception
                        // _domesticContext.RollbackTransaction(receiptException);

                        // TODO: when transactions are supported, delete this call to RollbackTranscriptRequests
                        await RollbackTranscriptRequests(order, modifiedBy);
                    }
                    catch
                    {
                        // TODO: when transactions are supported, handle exception thrown from RollbackTransaction
                    }

                    try
                    {
                        // user successfully paid so try to prevent them paying again
                        contact.PaymentLocked = true;
                        contact.ModifiedBy = modifiedBy;
                        await _domesticContext.UpdateContact(contact);
                    }
                    catch (Exception paymentLockException)
                    {
                        throw new AggregateException(receiptException, paymentLockException);
                    }

                    throw;
                }
            }

            var financialTransactions = await _domesticContext.GetFinancialTransactions(new Dto.GetFinancialTransactionOptions
            {
                ApplicantId = contact.Id,
                ApplicationId = application.Id,
                OrderId = order.Id
            });

            var financialTransaction = financialTransactions.FirstOrDefault();

            return financialTransaction is null ? null : _apiMapper.MapFinancialTransaction(financialTransaction);
        }

        private async Task ActivateTranscriptRequests(Dto.Order order, string modifiedBy)
        {
            var transcriptRequestStatuses = await _lookupsCache.GetTranscriptRequestStatuses(Constants.Localization.EnglishCanada);
            var waitingPaymentStatusId = transcriptRequestStatuses.First(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id;
            var waitingPaymentApprovalStatusId = transcriptRequestStatuses.First(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPaymentApproval).Id;

            var orderedTranscriptRequests = order.Details.Where(x => x.Type == ShoppingCartItemType.TranscriptRequestFee).ToList();

            if (!orderedTranscriptRequests.Any())
                return;

            var transcriptRequests = await _domesticContext.GetTranscriptRequests(new Dto.GetTranscriptRequestOptions
            {
                ApplicationId = order.ApplicationId
            });

            foreach (var unpaidTranscriptRequest in transcriptRequests.Where(x => x.TranscriptRequestStatusId == waitingPaymentStatusId && orderedTranscriptRequests.Any(y => y.ReferenceId == x.Id)))
            {
                unpaidTranscriptRequest.ModifiedBy = modifiedBy;
                unpaidTranscriptRequest.TranscriptRequestStatusId = waitingPaymentApprovalStatusId;

                try
                {
                    await _domesticContext.UpdateTranscriptRequest(unpaidTranscriptRequest);
                }
                catch (Exception e)
                {
                    _logger.LogCritical($"Failed to update TR {unpaidTranscriptRequest.Id} to {Constants.TranscriptRequestStatuses.WaitingPaymentApproval} status", e);
                }
            }
        }

        // TODO: when transactions are supported, delete this function
        private async Task RollbackTranscriptRequests(Dto.Order order, string modifiedBy)
        {
            var transcriptRequestStatuses = await _lookupsCache.GetTranscriptRequestStatuses(Constants.Localization.EnglishCanada);
            var waitingPaymentStatusId = transcriptRequestStatuses.First(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id;
            var waitingPaymentApprovalStatusId = transcriptRequestStatuses.First(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPaymentApproval).Id;

            var orderedTranscriptRequests = order.Details.Where(x => x.Type == ShoppingCartItemType.TranscriptRequestFee).ToList();

            if (!orderedTranscriptRequests.Any())
                return;

            var transcriptRequests = await _domesticContext.GetTranscriptRequests(new Dto.GetTranscriptRequestOptions
            {
                ApplicationId = order.ApplicationId
            });

            foreach (var waitingTranscriptRequest in transcriptRequests.Where(x => x.TranscriptRequestStatusId == waitingPaymentApprovalStatusId && orderedTranscriptRequests.Any(y => y.ReferenceId == x.Id)))
            {
                waitingTranscriptRequest.ModifiedBy = modifiedBy;
                waitingTranscriptRequest.TranscriptRequestStatusId = waitingPaymentStatusId;

                try
                {
                    await _domesticContext.UpdateTranscriptRequest(waitingTranscriptRequest);
                }
                catch (Exception e)
                {
                    _logger.LogCritical($"Failed to update TR {waitingTranscriptRequest.Id} to {Constants.TranscriptRequestStatuses.WaitingPayment} status", e);
                }
            }
        }
    }
}
