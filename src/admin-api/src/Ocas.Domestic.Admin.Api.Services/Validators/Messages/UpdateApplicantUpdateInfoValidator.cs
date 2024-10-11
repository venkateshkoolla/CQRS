using FluentValidation;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class UpdateApplicantUpdateInfoValidator : AbstractValidator<UpdateApplicantInfo>
    {
        public UpdateApplicantUpdateInfoValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.ApplicantUpdateInfo)
                .NotNull()
                .SetValidator(new ApplicantUpdateInfoValidator());
        }
    }
}
