using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class ApplicantAddressRuleCollection
    {
        public static Faker<TApplicantAddress> ApplyApplicantAddressRules<TApplicantAddress>(this Faker<TApplicantAddress> faker, AllLookups lookups)
            where TApplicantAddress : ApplicantAddress
        {
            var countries = lookups.Countries;
            var canada = countries.Single(x => x.Code == Constants.Countries.Canada);
            var unitedStates = countries.Single(x => x.Code == Constants.Countries.UnitedStates);
            var intlCountries = countries.Where(x => x.Code != Constants.Countries.Canada && x.Code != Constants.Countries.UnitedStates);

            var provinceStates = lookups.ProvinceStates;

            return faker
                .RuleFor(x => x.Verified, f => f.Random.Bool())
                .RuleFor(x => x.Street, f => f.Person.Address.Street)
                .RuleFor(x => x.City, f => f.Person.Address.City)
                .RuleFor(x => x.CountryId, f => f.PickRandom(countries).Id)
                .RuleFor(x => x.PostalCode, (f, o) =>
                {
                    if (o.CountryId == canada.Id) return "A1A1A1";
                    if (o.CountryId == unitedStates.Id) return f.Person.Address.ZipCode;

                    return f.Random.AlphaNumeric(10).ToUpperInvariant();
                })
                .RuleFor(x => x.ProvinceStateId, (f, o) =>
                {
                    if (o.CountryId == canada.Id) return f.PickRandom(provinceStates.Where(y => y.CountryId == canada.Id)).Id;
                    if (o.CountryId == unitedStates.Id) return f.PickRandom(provinceStates.Where(y => y.CountryId == unitedStates.Id)).Id;

                    return null;
                })
                .RuleFor(x => x.ProvinceState, (f, o) =>
                {
                    if (o.CountryId == canada.Id) return f.PickRandom(provinceStates.Where(y => y.CountryId == canada.Id)).Code;
                    if (o.CountryId == unitedStates.Id) return f.PickRandom(provinceStates.Where(y => y.CountryId == unitedStates.Id)).Code;

                    return null;
                })
                .RuleSet("Can", set =>
                {
                    set.RuleFor(x => x.CountryId, _ => canada.Id)
                       .RuleFor(x => x.PostalCode, _ => "A1A1A1")
                       .RuleFor(x => x.ProvinceStateId, f => f.PickRandom(provinceStates.Where(y => y.CountryId == canada.Id)).Id)
                       .RuleFor(x => x.ProvinceState, (_, o) => provinceStates.Single(x => x.Id == o.ProvinceStateId).Code);
                })
                .RuleSet("Usa", set =>
                {
                    set.RuleFor(x => x.CountryId, _ => unitedStates.Id)
                       .RuleFor(x => x.PostalCode, f => f.Person.Address.ZipCode)
                       .RuleFor(x => x.ProvinceStateId, f => f.PickRandom(provinceStates.Where(y => y.CountryId == unitedStates.Id)).Id)
                       .RuleFor(x => x.ProvinceState, (_, o) => provinceStates.Single(x => x.Id == o.ProvinceStateId).Code);
                })
                .RuleSet("Intl", set =>
                {
                    set.RuleFor(x => x.CountryId, f => f.PickRandom(intlCountries).Id)
                       .RuleFor(x => x.PostalCode, f => f.Random.AlphaNumeric(10).ToUpperInvariant())
                       .RuleFor(x => x.ProvinceStateId, _ => null)
                       .RuleFor(x => x.ProvinceState, _ => null);
                });
        }
    }
}
