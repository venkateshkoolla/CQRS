using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Messages
{
    public class CreateTranscriptRequestsValidator : AbstractValidator<CreateTranscriptRequests>
    {
        public CreateTranscriptRequestsValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .SetValidator(new IIdentityUserValidator());

            RuleFor(x => x.TranscriptRequests)
                .Must(x => x.Any())
                .WithMessage("'{PropertyName}' must not be empty.")
                .Must(x => x.Select(c => c.ApplicationId).AllEqual())
                .WithMessage("'{PropertyName}' must be for same application.")
                .ForEach(r => r.SetValidator(new TranscriptRequestBaseValidator(lookupsCache)));
        }
    }
}