using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class ProgramIntakeEntrySemesterBaseRuleCollection
    {
        public static Faker<TProgramIntakeEntrySemesterBase> ApplyProgramIntakeEntrySemesterBaseRules<TProgramIntakeEntrySemesterBase>(this Faker<TProgramIntakeEntrySemesterBase> faker, SeedDataFixture seedDataFixture)
            where TProgramIntakeEntrySemesterBase : ProgramIntakeEntrySemesterBase
        {
            return faker
                .RuleFor(x => x.ProgramIntakeId, _ => Guid.NewGuid())
                .RuleFor(x => x.EntrySemesterId, f => f.PickRandom(seedDataFixture.EntryLevels).Id)
                .RuleFor(x => x.Name, (_, o) => seedDataFixture.EntryLevels.Single(l => l.Id == o.EntrySemesterId).Name);
        }
    }
}
