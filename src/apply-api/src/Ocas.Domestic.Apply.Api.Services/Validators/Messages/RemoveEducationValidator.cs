using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class RemoveEducationValidator : AbstractValidator<RemoveEducation>
    {
        public RemoveEducationValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.EducationId)
                .NotEmpty();
        }
    }
}
