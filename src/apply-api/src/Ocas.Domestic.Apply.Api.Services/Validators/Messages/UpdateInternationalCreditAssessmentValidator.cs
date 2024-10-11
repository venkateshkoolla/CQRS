using FluentValidation;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class UpdateInternationalCreditAssessmentValidator : AbstractValidator<UpdateInternationalCreditAssessment>
    {
        public UpdateInternationalCreditAssessmentValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.IntlCredentialAssessment)
                .SetValidator(new IntlCredentialAssessmentValidator(lookupsCache));
        }
    }
}
