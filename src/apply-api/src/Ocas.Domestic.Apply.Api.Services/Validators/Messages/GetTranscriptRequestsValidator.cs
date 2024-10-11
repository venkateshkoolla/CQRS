using FluentValidation;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class GetTranscriptRequestsValidator : AbstractValidator<GetTranscriptRequests>
    {
        public GetTranscriptRequestsValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            // must provide one or the other
            RuleFor(x => x.ApplicantId)
                .NullableGuidNotEmpty()
                .When(x => x.ApplicationId.IsEmpty());

            RuleFor(x => x.ApplicationId)
                .NullableGuidNotEmpty()
                .When(x => x.ApplicantId.IsEmpty());
        }
    }
}
