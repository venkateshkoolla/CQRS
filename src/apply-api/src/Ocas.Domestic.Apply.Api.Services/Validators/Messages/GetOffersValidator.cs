using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class GetOffersValidator : AbstractValidator<GetOffers>
    {
        public GetOffersValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());
        }
    }
}
