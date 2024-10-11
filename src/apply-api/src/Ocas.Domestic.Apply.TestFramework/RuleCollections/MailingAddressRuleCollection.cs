using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class MailingAddressRuleCollection
    {
        public static Faker<TMailingAddress> ApplyMailingAddressRules<TMailingAddress>(this Faker<TMailingAddress> faker, AllLookups lookups)
            where TMailingAddress : MailingAddress
        {
            var countries = lookups.Countries;
            var canada = countries.Single(x => x.Code == Constants.Countries.Canada);
            var unitedStates = countries.Single(x => x.Code == Constants.Countries.UnitedStates);
            var intlCountries = countries.Where(x => x.Code != Constants.Countries.Canada && x.Code != Constants.Countries.UnitedStates);
            var provinceStates = lookups.ProvinceStates;
            var ontario = provinceStates.Single(x => x.Code == Constants.Provinces.Ontario);

            return faker
                .RuleFor(x => x.Street, f => f.Person.Address.Street)
                .RuleFor(x => x.City, f => f.Person.Address.City)
                .RuleFor(x => x.Country, f => f.PickRandom(countries).Name)
                .RuleFor(x => x.PostalCode, (f, o) =>
                {
                    var countryId = countries.FirstOrDefault(c => c.Name == o.Country)?.Id;
                    if (countryId == canada.Id) return "A1A1A1";
                    if (countryId == unitedStates.Id) return f.Person.Address.ZipCode;

                    return null;
                })
                .RuleFor(x => x.ProvinceState, (f, o) =>
                {
                    var countryId = countries.FirstOrDefault(c => c.Name == o.Country)?.Id;
                    if (countryId == canada.Id) return f.PickRandom(provinceStates.Where(y => y.CountryId == canada.Id)).Code;
                    if (countryId == unitedStates.Id) return f.PickRandom(provinceStates.Where(y => y.CountryId == unitedStates.Id)).Code;

                    return null;
                })
                .RuleSet("Can", set =>
                {
                    set.RuleFor(x => x.Country, _ => canada.Name)
                       .RuleFor(x => x.PostalCode, _ => "A1A1A1")
                       .RuleFor(x => x.ProvinceState, f => f.PickRandom(provinceStates.Where(y => y.CountryId == canada.Id)).Code);
                })
                .RuleSet("Ont", set => set.RuleFor(x => x.ProvinceState, _ => ontario.Code))
                .RuleSet("Usa", set =>
                {
                    set.RuleFor(x => x.Country, _ => unitedStates.Name)
                       .RuleFor(x => x.PostalCode, f => f.Person.Address.ZipCode)
                       .RuleFor(x => x.ProvinceState, f => f.PickRandom(provinceStates.Where(y => y.CountryId == unitedStates.Id)).Code);
                })
                .RuleSet("Intl", set =>
                {
                    set.RuleFor(x => x.Country, f => f.PickRandom(intlCountries).Name)
                       .RuleFor(x => x.PostalCode, f => f.Person.Address.ZipCode)
                       .RuleFor(x => x.ProvinceState, _ => null);
                });
        }
    }
}
