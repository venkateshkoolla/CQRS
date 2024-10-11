using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Services.Validators.Messages
{
    public class OfflinePaymentInfoValidator : AbstractValidator<OfflinePaymentInfo>
    {
        public OfflinePaymentInfoValidator(ILookupsCacheBase lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.PaymentMethodId)
                .NotEmpty()
                .MustAsync(async (x, _) =>
                {
                    var paymentMethods = await lookupsCache.GetPaymentMethods(Constants.Localization.EnglishCanada);
                    return paymentMethods.Any(e => e.Id == x);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.PaymentMethodId}");

            RuleFor(x => x.Amount)
                .NotEmpty()
                .GreaterThan(0)
                .ScalePrecision(2, 10)
                .WithMessage("'{PropertyName}' cannot have more than 2 decimals.");

            RuleFor(x => x.Notes)
                .NotEmpty()
                .WhenAsync(async (x, _) =>
               {
                   var paymentMethods = await lookupsCache.GetPaymentMethods(Constants.Localization.EnglishCanada);

                   paymentMethods = paymentMethods.Where(z => z.Code == Constants.PaymentMethods.Cheque
                                                    || z.Code == Constants.PaymentMethods.MoneyOrder
                                                    || z.Code == Constants.PaymentMethods.InteracOnline
                                                    || z.Code == Constants.PaymentMethods.OnlineBanking).ToList();

                   return paymentMethods.Any(e => e.Id == x.PaymentMethodId);
               })
                .WithMessage(x => $"Payment number is required to proceed with offline payment for this Payment Type: {x.PaymentMethodId}");
        }
    }
}
