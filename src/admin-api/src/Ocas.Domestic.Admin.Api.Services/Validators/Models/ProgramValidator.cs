using FluentValidation;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models
{
    public class ProgramValidator : AbstractValidator<Program>
    {
        public ProgramValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x)
                .SetValidator(new ProgramBaseValidator(lookupsCache, domesticContext));
        }
    }
}
