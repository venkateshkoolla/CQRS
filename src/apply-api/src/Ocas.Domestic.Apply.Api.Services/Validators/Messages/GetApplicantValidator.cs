using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class GetApplicantValidator : AbstractValidator<GetApplicant>
    {
        public GetApplicantValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicantId)
                .NullableGuidNotEmpty()
                .When(y => y.ApplicantId.HasValue);
        }
    }
}
