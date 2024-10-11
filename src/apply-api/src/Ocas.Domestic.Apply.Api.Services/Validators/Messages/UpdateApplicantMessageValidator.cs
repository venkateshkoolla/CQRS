using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class UpdateApplicantMessageValidator : AbstractValidator<UpdateApplicantMessage>
    {
        public UpdateApplicantMessageValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.Id)
                .NotEmpty();
        }
    }
}
