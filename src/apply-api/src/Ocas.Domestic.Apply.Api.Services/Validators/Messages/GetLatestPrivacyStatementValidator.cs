using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class GetLatestPrivacyStatementValidator : AbstractValidator<GetLatestPrivacyStatement>
    {
        public GetLatestPrivacyStatementValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());
        }
    }
}
