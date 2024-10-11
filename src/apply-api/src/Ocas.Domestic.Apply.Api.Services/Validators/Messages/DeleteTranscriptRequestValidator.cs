using FluentValidation;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class DeleteTranscriptRequestValidator : AbstractValidator<DeleteTranscriptRequest>
    {
        public DeleteTranscriptRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.TranscriptRequestId)
                .NotEmpty();
        }
    }
}
