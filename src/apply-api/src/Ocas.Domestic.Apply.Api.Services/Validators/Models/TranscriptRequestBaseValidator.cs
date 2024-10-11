using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.Validators.Models
{
    public class TranscriptRequestBaseValidator : AbstractValidator<TranscriptRequestBase>
    {
        public TranscriptRequestBaseValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x)
                .Must(x => x.FromInstituteId != x.ToInstituteId)
                .WithMessage(x => $"'From Institute Id' cannot be the same as 'To Institute Id': {x.FromInstituteId}, {x.ToInstituteId}");

            RuleFor(x => x.ApplicationId)
                .NotEmpty();

            RuleFor(x => x.FromInstituteId)
                .NotEmpty();

            RuleFor(x => x.TransmissionId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var transmissions = await lookupsCache.GetTranscriptTransmissions(Constants.Localization.EnglishCanada);
                    return transmissions.Any(t => t.Id == y);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.TransmissionId}");
        }
    }
}
