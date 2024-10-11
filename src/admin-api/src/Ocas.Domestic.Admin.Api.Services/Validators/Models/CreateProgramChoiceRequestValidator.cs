using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models
{
    public class CreateProgramChoiceRequestValidator : AbstractValidator<CreateProgramChoiceRequest>
    {
        public CreateProgramChoiceRequestValidator(ILookupsCache lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.ApplicationId)
                .NotEmpty();

            RuleFor(x => x.ProgramId)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .Must(x => x.IsDate(Constants.DateFormat.IntakeStartDate))
                .WithMessage("'{PropertyName}' must be a valid date.");

            RuleFor(x => x.EntryLevelId)
                .NotEmpty()
                .MustAsync(async (x, _) =>
                {
                    var entryLevels = await lookupsCache.GetEntryLevels(Constants.Localization.EnglishCanada);
                    return entryLevels.Any(e => e.Id == x);
                })
                .WithMessage(y => $"'{{PropertyName}}' does not exist: {y.EntryLevelId}");
        }
    }
}
