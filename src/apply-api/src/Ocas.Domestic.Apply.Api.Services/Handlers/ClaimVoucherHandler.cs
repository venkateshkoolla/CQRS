using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class ClaimVoucherHandler : IRequestHandler<ClaimVoucher>
    {
        public const string Error = "Cannot claim voucher";
        private readonly ILookupsCache _lookupsCache;
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;

        public ClaimVoucherHandler(ILogger<ClaimVoucherHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<Unit> Handle(ClaimVoucher request, CancellationToken cancellationToken)
        {
            var sources = await _lookupsCache.GetSources(Constants.Localization.EnglishCanada);
            var sourceId = sources.First(x => x.Code == Constants.Sources.A2C2).Id;

            var application = await _domesticContext.GetApplication(request.ApplicationId) ?? throw new NotFoundException($"Application {request.ApplicationId} not found");

            await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId);

            var applicant = await _domesticContext.GetContact(application.ApplicantId) ?? throw new NotFoundException($"Applicant {application.ApplicantId} not found");

            if (applicant.PaymentLocked)
            {
                _logger.LogError("Applicant is payment locked");
                throw new ValidationException(Error);
            }

            var voucher = await _domesticContext.GetVoucher(new GetVoucherOptions
            {
                Code = request.Code
            });

            if (voucher is null)
            {
                _logger.LogWarning($"Voucher {request.Code} does not exist");
                throw new ValidationException(Error);
            }

            if (voucher.VoucherState != VoucherState.Issued)
            {
                _logger.LogWarning($"Voucher {voucher.Code} not issued");
                throw new ValidationException(Error);
            }

            var shoppingCart = await _domesticContext.GetShoppingCart(
                new GetShoppingCartOptions
                {
                    ApplicationId = request.ApplicationId
                },
                Locale.English);

            if (shoppingCart is null)
            {
                _logger.LogCritical($"ShoppingCart for application {request.ApplicationId} does not exist");
                throw new ValidationException(Error);
            }

            var shoppingCartDetails = shoppingCart.Details;
            if (!shoppingCartDetails.Any())
            {
                // if there is nothing in the shopping cart, do not claim
                _logger.LogWarning($"ShoppingCart is empty, not claiming voucher: {voucher.Code}");
                throw new ValidationException(Error);
            }

            if (voucher.ProductId is null)
            {
                _logger.LogCritical($"Voucher {voucher.Code} missing ProductId");
                throw new ValidationException(Error);
            }

            if (shoppingCartDetails.All(x => x.ProductId != voucher.ProductId))
            {
                // there is no item in this shopping cart that matches this voucher's ProductId
                _logger.LogWarning($"ShoppingCart does not contain Voucher's product: {voucher.Code}");
                throw new ValidationException(Error);
            }

            if (shoppingCartDetails.Any(p => p.Type == ShoppingCartItemType.Voucher && p.VoucherProductId == voucher.ProductId))
            {
                // cannot add a duplicate Voucher
                // the duplicate check is based on the voucher's ProductId
                _logger.LogWarning($"Cannot apply a Voucher with the same product again: {voucher.Code}");
                throw new ValidationException(Error);
            }

            var voucherByApplicationId = await _domesticContext.GetVoucher(new GetVoucherOptions
            {
                ApplicationId = request.ApplicationId
            });

            // From A2C: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Services%2FVoucher%2FVoucherService.cs&version=GBmaster&line=157&lineStyle=plain&lineEnd=166&lineStartColumn=13&lineEndColumn=14
            if (voucherByApplicationId != null)
            {
                // Get voucher by ApplicationId and check if there is a voucher associated with that Application if so then can not apply
                _logger.LogWarning($"Cannot apply more than one voucher to the same application: {voucher.Code}");
                throw new ValidationException(Error);
            }

            var modifiedBy = request.User.GetUpnOrEmail();

            await _domesticContext.AddVoucherToShoppingCart(voucher, application.Id, application.ApplicantId, shoppingCart.Id, sourceId, modifiedBy);

            return Unit.Value;
        }
    }
}
