using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class AcceptPrivacyStatementValidator : AbstractValidator<AcceptPrivacyStatement>
    {
        public AcceptPrivacyStatementValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.ApplicantId)
                .NotEmpty();

            RuleFor(x => x.PrivacyStatementId)
                .NotEmpty();
        }
    }
}
