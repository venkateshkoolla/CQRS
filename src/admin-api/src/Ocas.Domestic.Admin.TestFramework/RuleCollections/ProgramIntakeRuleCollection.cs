using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Bogus.Extensions;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.TestFramework.RuleCollections
{
    public static class ProgramIntakeRuleCollection
    {
        private const string Format = Constants.DateFormat.YearMonthDay;

        public static Faker<TProgramIntake> ApplyProgramIntakeRules<TProgramIntake>(this Faker<TProgramIntake> faker, AllLookups lookups, ProgramBase program, IList<string> usedStartDates)
            where TProgramIntake : ProgramIntake
        {
            var entryLevels = lookups.EntryLevels;
            var intakeAvailabilities = lookups.IntakeAvailabilities;
            var intakeStatuses = lookups.IntakeStatuses;
            var intakeExpiryActions = lookups.IntakeExpiryActions;
            var collegeApplicationCycles = lookups.CollegeApplicationCycles;
            var collegeApplicationCycle = collegeApplicationCycles.Single(x => x.Id == program.ApplicationCycleId);
            var applicationCycles = lookups.ApplicationCycles;
            var applicationCycle = applicationCycles.Single(x => x.Id == collegeApplicationCycle.MasterId);

            return faker
                .RuleFor(o => o.IntakeAvailabilityId, f => f.PickRandom(intakeAvailabilities).Id)
                .RuleFor(o => o.IntakeStatusId, f => f.PickRandom(intakeStatuses).Id)
                .RuleFor(o => o.EnrolmentEstimate, f => f.Random.Number(100))
                .RuleFor(o => o.EnrolmentMax, (f, o) => f.Random.Number(o.EnrolmentEstimate.Value, 300))
                .RuleFor(o => o.ExpiryDate, f => f.Date.Future().ToString(Format))
                .RuleFor(o => o.IntakeExpiryActionId, f => f.PickRandom(intakeExpiryActions).Id)
                .RuleFor(o => o.DefaultEntryLevelId, f => f.PickRandom(ValidEntryLevels(program.DefaultEntryLevelId, lookups)).OrNull(f))
                .RuleFor(o => o.EntryLevelIds, (_, o) => o.DefaultEntryLevelId.HasValue ? ValidEntryLevels(o.DefaultEntryLevelId.Value, lookups).Select(x => x.Value).ToList() : null)
                .RuleFor(o => o.StartDate, (f, _) =>
                {
                    var startDates = ValidStartDates(int.Parse(applicationCycle.Year), usedStartDates);
                    var startDate = f.PickRandom(startDates);
                    usedStartDates.Add(startDate);
                    return startDate;
                });
        }

        private static IList<Guid?> ValidEntryLevels(Guid defaultEntryLevelId, AllLookups lookups)
        {
            return lookups.EntryLevels.Where(p => string.Compare(lookups.EntryLevels.Single(e => e.Id == defaultEntryLevelId).Code, p.Code, StringComparison.InvariantCultureIgnoreCase) <= 0).Select(x => (Guid?)x.Id).ToList();
        }

        private static IList<string> ValidStartDates(int appCycleYear, IList<string> usedStartDates)
        {
            var validStartDates = new List<string>();
            for (var i = 8; i <= 19; i++)
            {
                var year = i > 12 ? appCycleYear + 1 : appCycleYear;
                var month = i > 12 ? i - 12 : i;
                var paddedMonth = month.ToString("#00");
                var startDate = year.ToString().Substring(2) + paddedMonth;

                if (usedStartDates.Contains(startDate)) continue;

                validStartDates.Add(startDate);
            }

            return validStartDates;
        }
    }
}
