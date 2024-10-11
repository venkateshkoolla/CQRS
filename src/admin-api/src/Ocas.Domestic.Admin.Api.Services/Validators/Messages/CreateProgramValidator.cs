using FluentValidation;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class CreateProgramValidator : AbstractValidator<CreateProgram>
    {
        public CreateProgramValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.Program)
                .NotEmpty()
                .SetValidator(new ProgramBaseValidator(lookupsCache, domesticContext));
        }
    }
}
