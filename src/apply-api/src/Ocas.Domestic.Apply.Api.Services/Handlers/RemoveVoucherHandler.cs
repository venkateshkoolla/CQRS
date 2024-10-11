using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class RemoveVoucherHandler : IRequestHandler<RemoveVoucher>
    {
        public const string Error = "Cannot remove voucher";
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;

        public RemoveVoucherHandler(ILogger<RemoveVoucherHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
        }

        public async Task<Unit> Handle(RemoveVoucher request, CancellationToken cancellationToken)
        {
            var application = await _domesticContext.GetApplication(request.ApplicationId) ?? throw new NotFoundException($"Application {request.ApplicationId} not found");

            await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId);

            var applicant = await _domesticContext.GetContact(application.ApplicantId) ?? throw new NotFoundException($"Applicant {application.ApplicantId} not found");

            if (applicant.PaymentLocked)
            {
                _logger.LogError("Applicant is payment locked");
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

            var voucher = await _domesticContext.GetVoucher(new GetVoucherOptions
            {
                Code = request.Code
            });

            if (voucher is null)
            {
                _logger.LogWarning($"Voucher {request.Code} does not exist");
                throw new ValidationException(Error);
            }

            if (voucher.VoucherState != VoucherState.Claimed)
            {
                _logger.LogWarning($"Voucher {voucher.Code} not in claimed state, cannot remove");
                throw new ValidationException(Error);
            }

            var shoppingCartDetail = shoppingCart.Details.FirstOrDefault(x => x.Type == ShoppingCartItemType.Voucher && x.VoucherId == voucher.Id);
            if (shoppingCartDetail is null)
            {
                // confirm that the shopping cart does indeed contain the voucher
                _logger.LogWarning($"ShoppingCart does not contain voucher: {voucher.Code}");
                throw new ValidationException(Error);
            }

            var modifiedBy = request.User.GetUpnOrEmail();
            await _domesticContext.RemoveVoucherFromShoppingCart(voucher, modifiedBy);

            return Unit.Value;
        }
    }
}
