using System;
using Bogus;
using Bogus.Extensions;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class ProgramIntakeRuleCollection
    {
        public static Faker<TProgramIntakeBase> ApplyProgramIntakeBaseRules<TProgramIntakeBase>(this Faker<TProgramIntakeBase> faker, SeedDataFixture seedDataFixture)
            where TProgramIntakeBase : Models.ProgramIntakeBase
        {
            return faker
                .RuleFor(o => o.ProgramId, _ => Guid.NewGuid())
                .RuleFor(o => o.AvailabilityId, f => f.PickRandom(seedDataFixture.ProgramIntakeAvailabilities).Id)
                .RuleFor(o => o.ExpiryDate, f => f.Date.Future(2, DateTime.UtcNow))
                .RuleFor(o => o.EnrolmentProjection, f => f.Random.Number(100).OrNull(f))
                .RuleFor(o => o.EnrolmentMaximum, (f, o) => o.EnrolmentProjection.HasValue ? f.Random.Number(o.EnrolmentProjection.Value, 300) : (int?)null)
                .RuleFor(o => o.ExpiryActionId, f => f.PickRandom(seedDataFixture.ExpiryActions).Id)
                .RuleFor(o => o.ProgramIntakeStatusId, f => f.PickRandom(seedDataFixture.ProgramIntakeStatuses).Id)
                .RuleFor(o => o.HasSemesterOverride, f => f.Random.Bool().OrNull(f))
                .RuleFor(o => o.DefaultEntrySemesterId, (f, o) => o.HasSemesterOverride == true ? f.PickRandom(seedDataFixture.EntryLevels).Id : (Guid?)null);
        }
    }
}
