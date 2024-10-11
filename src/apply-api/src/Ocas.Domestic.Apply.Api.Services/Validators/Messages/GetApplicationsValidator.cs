using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class GetApplicationsValidator : AbstractValidator<GetApplications>
    {
        public GetApplicationsValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());
        }
    }
}
