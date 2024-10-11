using System;
using System.Linq;
using Bogus;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class ProgramChoiceRuleCollection
    {
        public static Faker<TProgramChoiceBase> ApplyProgramChoiceBaseRules<TProgramChoiceBase>(this Faker<TProgramChoiceBase> faker, SeedDataFixture seedDataFixture)
           where TProgramChoiceBase : Models.ProgramChoiceBase
        {
            var source = seedDataFixture.Sources.FirstOrDefault(s => s.Code == TestConstants.Sources.A2C2);

            return faker
                .RuleFor(o => o.ApplicantId, _ => Guid.NewGuid())
                .RuleFor(o => o.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(o => o.ProgramIntakeId, _ => TestConstants.ProgramChoices.ProgramIntake)
                .RuleFor(o => o.EntryLevelId, f => f.PickRandom(seedDataFixture.EntryLevels).Id)
                .RuleFor(o => o.PreviousYearApplied, f => f.Date.Past(3).Year)
                .RuleFor(o => o.PreviousYearAttended, f => f.Date.Past(2).Year)
                .RuleFor(o => o.SourceId, _ => source.Id)
                .RuleFor(o => o.SequenceNumber, _ => 1)
                .RuleFor(o => o.EffectiveDate, _ => new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 0, 0, 0, DateTimeKind.Utc));
        }
    }
}
