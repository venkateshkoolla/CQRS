using FluentValidation;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class CreateApplicantBaseValidator : AbstractValidator<CreateApplicantBase>
    {
        public CreateApplicantBaseValidator(IUserAuthorization userAuthorization)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicantBase)
                .NotNull()
                .SetValidator(new ApplicantBaseValidator());

            RuleFor(x => x.User)
                .NotBeOcasUser(userAuthorization);
        }
    }
}
