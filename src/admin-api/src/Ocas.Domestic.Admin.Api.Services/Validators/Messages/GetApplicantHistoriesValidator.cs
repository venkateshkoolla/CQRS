using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetApplicantHistoriesValidator : AbstractValidator<GetApplicantHistories>
    {
        public GetApplicantHistoriesValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IApplicantUserValidator());

            RuleFor(x => x.Options)
                .SetValidator(new GetApplicantHistoryOptionsValidator());

            RuleFor(x => x.ApplicationId)
                .NullableGuidNotEmpty()
                .When(y => y.ApplicationId.HasValue);
        }
    }
}
