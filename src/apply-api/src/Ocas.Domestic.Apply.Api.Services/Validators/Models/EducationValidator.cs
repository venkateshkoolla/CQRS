using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class EducationValidator : AbstractValidator<Education>
    {
        public EducationValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x)
                .SetValidator(new EducationBaseValidator(lookupsCache, domesticContext, OperationType.Update));
        }
    }
}
