using System;
using System.Linq;
using Bogus;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class TestRuleCollection
    {
        public static Faker<TTestBase> ApplyTestBaseRules<TTestBase>(this Faker<TTestBase> faker, SeedDataFixture seedDataFixture)
            where TTestBase : Models.TestBase
        {
            return faker
                .RuleFor(o => o.TestTypeId, f => f.PickRandom(seedDataFixture.TestTypes).Id)
                .RuleFor(o => o.CountryId, f => f.PickRandom(seedDataFixture.Countries).Id)
                .RuleFor(o => o.ProvinceStateName, (f, o) => seedDataFixture.ProvinceStates.Any(x => x.CountryId == o.CountryId) ? string.Empty : f.Address.State())
                .RuleFor(o => o.ProvinceStateId, (f, o) => seedDataFixture.ProvinceStates.Any(x => x.CountryId == o.CountryId) ? f.PickRandom(seedDataFixture.ProvinceStates.Where(x => x.CountryId == o.CountryId)).Id : (Guid?)null)
                .RuleFor(o => o.CityName, (f, o) => seedDataFixture.Cities.Any(x => x.ProvinceId == o.ProvinceStateId) ? string.Empty : f.Address.City())
                .RuleFor(o => o.CityId, (f, o) => seedDataFixture.Cities.Any(x => x.ProvinceId == o.ProvinceStateId) ? f.PickRandom(seedDataFixture.Cities.Where(x => x.ProvinceId == o.ProvinceStateId)).Id : (Guid?)null)
                .RuleFor(o => o.DateTestTaken, (f) =>
                {
                    var date = f.Date.Past();
                    return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Utc);
                })
                .RuleFor(o => o.Description, f => f.Lorem.Sentence(5))
                .RuleFor(o => o.IsOfficial, _ => false)
                .RuleSet("Official", set => set.RuleFor(o => o.IsOfficial, _ => true));
        }
    }
}
