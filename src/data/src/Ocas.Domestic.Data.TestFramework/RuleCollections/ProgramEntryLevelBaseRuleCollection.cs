using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class ProgramEntryLevelBaseRuleCollection
    {
        public static Faker<TProgramEntryLevelBase> ApplyProgramEntryLevelBaseRules<TProgramEntryLevelBase>(this Faker<TProgramEntryLevelBase> faker, SeedDataFixture seedDataFixture)
            where TProgramEntryLevelBase : ProgramEntryLevelBase
        {
            return faker
                .RuleFor(x => x.ProgramId, _ => Guid.NewGuid())
                .RuleFor(x => x.EntryLevelId, f => f.PickRandom(seedDataFixture.EntryLevels).Id)
                .RuleFor(x => x.Name, (_, o) => seedDataFixture.EntryLevels.Single(l => l.Id == o.EntryLevelId).Name);
        }
    }
}
