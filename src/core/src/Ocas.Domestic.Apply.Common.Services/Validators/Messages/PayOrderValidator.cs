using FluentValidation;
using Ocas.Domestic.Apply.Services.Messages;
using Ocas.Domestic.Apply.Services.Validators.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Services.Validators.Messages
{
    public class PayOrderValidator : AbstractValidator<PayOrder>
    {
        public PayOrderValidator(IDomesticContext domesticContext, ILookupsCacheBase lookupsCacheBase)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.OrderId)
                .NotEmpty();

            RuleFor(x => x.PayOrderInfo)
                .NotNull()
                .SetValidator(e => new PayOrderInfoValidator(domesticContext, e.OrderId))
                .When(x => !x.IsOfflinePayment);

            RuleFor(x => x.OfflinePaymentInfo)
                .NotNull()
                .SetValidator(new OfflinePaymentInfoValidator(lookupsCacheBase))
                .When(x => x.IsOfflinePayment);
        }
    }
}
