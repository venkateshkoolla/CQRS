using FluentValidation;
using Ocas.Domestic.Apply.Admin.Messages;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages
{
    public class GetCollegeTransmissionHistoriesValidator : AbstractValidator<GetCollegeTransmissionHistories>
    {
        public GetCollegeTransmissionHistoriesValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.ApplicationId)
                .NotEmpty();

            RuleFor(x => x.Options)
                .SetValidator(new GetCollegeTransmissionHistoryOptionsValidator());
        }
    }
}
