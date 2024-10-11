using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models
{
    public class ProgramIntakeValidator : AbstractValidator<ProgramIntake>
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly Guid _programDefaultEntryLevelId;

        public ProgramIntakeValidator(ILookupsCache lookupsCache, Guid programDefaultEntryLevelId)
        {
            _lookupsCache = lookupsCache;
            _programDefaultEntryLevelId = programDefaultEntryLevelId;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.StartDate)
                .NotEmpty();

            RuleFor(x => x.EnrolmentEstimate)
                .InclusiveBetween(0, 99999)
                .Unless(v => v == null);

            RuleFor(x => x.EnrolmentMax)
                .InclusiveBetween(0, 99999)
                .Unless(v => v == null);

            RuleFor(x => x.IntakeAvailabilityId)
                .NotEmpty();

            RuleFor(x => x.IntakeStatusId)
                .NotEmpty();

            When(x => !string.IsNullOrWhiteSpace(x.ExpiryDate), () =>
            {
                RuleFor(x => x.ExpiryDate).Must(v => v.IsDate());
                RuleFor(x => x.IntakeExpiryActionId).NotNull().NotEqual(Guid.Empty);
            });

            When(x => !x.DefaultEntryLevelId.IsEmpty() || x.EntryLevelIds?.Any() == true, () =>
            {
                RuleFor(x => x.DefaultEntryLevelId)
                    .NullableGuidNotEmpty()
                    .Must(IsDefaultEntryLevelIncluded)
                    .WithMessage("'{PropertyName}' should be included in list of entry levels")
                    .MustAsync((y, _) => IsValidDefaultEntryLevel(y))
                    .WithMessage("'{PropertyName}' should not be lower then program's default entry level");

                RuleFor(x => x.EntryLevelIds)
                    .MustAsync((x, y, _) => IsValidEntryLevels(x, y))
                    .WithMessage("Please don't select entry level lower then default");
            });
        }

        private async Task<bool> IsValidEntryLevels(ProgramIntake intake, IEnumerable<Guid> entryLevels)
        {
            if (intake.DefaultEntryLevelId.IsEmpty() || intake.EntryLevelIds?.Any() == false) return false;

            var entryLevelsLookups = await _lookupsCache.GetEntryLevels(Constants.Localization.EnglishCanada);
            var codeForDefaultEntryLevelId = entryLevelsLookups.Single(e => e.Id == intake.DefaultEntryLevelId).Code;
            var fullEntryLevels = entryLevelsLookups.Where(p => entryLevels.Any(p2 => p2 == p.Id));
            return fullEntryLevels.All(p => string.Compare(codeForDefaultEntryLevelId, p.Code, StringComparison.InvariantCultureIgnoreCase) <= 0);
        }

        private async Task<bool> IsValidDefaultEntryLevel(Guid? defaultEntryLevelId)
        {
            if (_programDefaultEntryLevelId.IsEmpty()) return false;

            var entryLevels = await _lookupsCache.GetEntryLevels(Constants.Localization.EnglishCanada);
            var defaultEntryLevelCode = entryLevels.Single(e => e.Id == defaultEntryLevelId).Code;
            var programDefaultEntryLevelCode = entryLevels.SingleOrDefault(e => e.Id == _programDefaultEntryLevelId)?.Code;
            return string.Compare(programDefaultEntryLevelCode, defaultEntryLevelCode, StringComparison.InvariantCultureIgnoreCase) <= 0;
        }

        private bool IsDefaultEntryLevelIncluded(ProgramIntake intake, Guid? defaultEntryLevelId)
        {
            if (defaultEntryLevelId.IsEmpty() || intake.EntryLevelIds?.Any() == false) return false;
            return intake.EntryLevelIds.Contains(defaultEntryLevelId.Value);
        }
    }
}
