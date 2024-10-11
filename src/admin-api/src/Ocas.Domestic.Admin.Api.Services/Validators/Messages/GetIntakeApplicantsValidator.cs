using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetIntakeApplicantsValidator : AbstractValidator<GetIntakeApplicants>
    {
        public GetIntakeApplicantsValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.IntakeId)
                .NotEmpty();
        }
    }
}
