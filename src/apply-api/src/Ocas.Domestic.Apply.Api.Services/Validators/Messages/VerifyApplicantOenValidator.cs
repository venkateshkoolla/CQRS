using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class VerifyApplicantOenValidator : AbstractValidator<VerifyApplicantOen>
    {
        public VerifyApplicantOenValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicantId)
                .NotEmpty();
        }
    }
}
