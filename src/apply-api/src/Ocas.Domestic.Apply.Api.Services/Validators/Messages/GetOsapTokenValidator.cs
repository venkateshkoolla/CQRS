using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class GetOsapTokenValidator : AbstractValidator<GetOsapToken>
    {
        public GetOsapTokenValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());
        }
    }
}
