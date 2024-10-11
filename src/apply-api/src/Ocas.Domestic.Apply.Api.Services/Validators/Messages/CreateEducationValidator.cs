using FluentValidation;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class CreateEducationValidator : AbstractValidator<CreateEducation>
    {
        public CreateEducationValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.ApplicantId)
                .NotEmpty()
                .Equal(x => x.Education.ApplicantId);

            RuleFor(x => x.Education)
                .SetValidator(new EducationBaseValidator(lookupsCache, domesticContext));
        }
    }
}
