using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class AcceptOfferValidator : AbstractValidator<AcceptOffer>
    {
        public AcceptOfferValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.OfferId)
                .NotEmpty();
        }
    }
}
