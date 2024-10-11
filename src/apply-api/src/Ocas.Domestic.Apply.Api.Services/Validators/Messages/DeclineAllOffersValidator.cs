using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class DeclineAllOffersValidator : AbstractValidator<DeclineAllOffers>
    {
        public DeclineAllOffersValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicationId)
                .NotEmpty();
        }
    }
}
