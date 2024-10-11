using System.Linq;
using FluentValidation;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Services.Validators.Models
{
    public class ProgramChoiceBaseValidator : AbstractValidator<ProgramChoiceBase>
    {
        public ProgramChoiceBaseValidator(ILookupsCacheBase lookupsCache)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

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
