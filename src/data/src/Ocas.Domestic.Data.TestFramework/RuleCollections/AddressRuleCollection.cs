using System.Linq;
using Bogus;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class AddressRuleCollection
    {
        public static Faker<TAddress> ApplyAddressRules<TAddress>(this Faker<TAddress> faker, SeedDataFixture seedDataFixture)
            where TAddress : Models.Address
        {
            var canada = seedDataFixture.Countries.Single(x => x.Name == "Canada");
            var unitedStates = seedDataFixture.Countries.Single(x => x.Name == "United States");

            return faker
                .RuleFor(o => o.Street, f => f.Person.Address.Street)
                .RuleFor(o => o.City, f => f.Person.Address.City)
                .RuleFor(o => o.CountryId, f => f.PickRandom(seedDataFixture.Countries).Id)
                .RuleFor(o => o.Country, (_, u) => seedDataFixture.Countries.Single(x => x.Id == u.CountryId).Name)
                .RuleFor(o => o.PostalCode, (f, u) => u.CountryId == canada.Id ? "A1A1A1" : f.Person.Address.ZipCode)
                .RuleFor(o => o.ProvinceStateId, (f, u) =>
                {
                    if (u.CountryId == canada.Id || u.CountryId == unitedStates.Id)
                    {
                        return f.PickRandom(seedDataFixture.ProvinceStates.Where(x => x.CountryId == u.CountryId)).Id;
                    }

                    return null;
                })
                .RuleFor(o => o.ProvinceState, (_, u) =>
                {
                    if (u.ProvinceStateId is null)
                    {
                        return null;
                    }

                    return seedDataFixture.ProvinceStates.Single(x => x.Id == u.ProvinceStateId).Code;
                })
                .RuleFor(o => o.Verified, f => f.Random.Bool());
        }
    }
}
