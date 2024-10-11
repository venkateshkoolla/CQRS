using FluentValidation;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Services.Messages;

namespace Ocas.Domestic.Apply.Services.Validators.Models
{
    public class GetProgramChoicesValidator : AbstractValidator<GetProgramChoices>
    {
        public GetProgramChoicesValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicationId)
                .NullableGuidNotEmpty()
                .When(x => x.ApplicantId.IsEmpty());

            RuleFor(x => x.ApplicantId)
                .NullableGuidNotEmpty()
                .When(x => x.ApplicationId.IsEmpty());
        }
    }
}
