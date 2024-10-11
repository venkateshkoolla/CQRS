using FluentValidation;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class UpdateProgramValidator : AbstractValidator<UpdateProgram>
    {
        public UpdateProgramValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ProgramId)
                .NotEmpty()
                .Equal(x => x.Program.Id);

            RuleFor(x => x.Program)
                .NotEmpty()
                .SetValidator(new ProgramValidator(lookupsCache, domesticContext));
        }
    }
}
