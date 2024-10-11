using FluentValidation;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class UpdateApplicantValidator : AbstractValidator<UpdateApplicant>
    {
        public UpdateApplicantValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.ApplicantId)
                .Equal(x => x.Applicant.Id)
                .NotEmpty();

            RuleFor(x => x.Applicant)
                .SetValidator(new ApplicantValidator(lookupsCache));
        }
    }
}
