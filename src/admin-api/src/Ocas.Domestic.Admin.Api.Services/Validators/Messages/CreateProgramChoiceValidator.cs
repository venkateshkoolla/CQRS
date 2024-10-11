using FluentValidation;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class CreateProgramChoiceValidator : AbstractValidator<CreateProgramChoice>
    {
        public CreateProgramChoiceValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicationId)
                .NotEmpty();

            RuleFor(x => x.ProgramChoice)
                .NotNull()
                .Must((c, o) => c.ApplicationId == o.ApplicationId)
                .WithMessage("'{PropertyName}' must be for requested application.")
                .SetValidator(new CreateProgramChoiceRequestValidator(lookupsCache));
        }
    }
}
