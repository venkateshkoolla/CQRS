using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class VerifyApplicantEmailAddressValidator : AbstractValidator<VerifyApplicantEmailAddress>
    {
        public VerifyApplicantEmailAddressValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ApplicantId)
                .NotEmpty();

            RuleFor(x => x.User)
                .NotEmpty();
        }
    }
}
