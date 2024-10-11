using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators
{
    public class IApplicantUserValidator : AbstractValidator<IApplicantUser>
    {
        public IApplicantUserValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicantId)
                .NotEmpty();
        }
    }
}
