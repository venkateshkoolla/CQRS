using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Ocas.Common;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class ApplicantRuleCollection
    {
        public static Faker<TApplicant> ApplyApplicantRules<TApplicant>(this Faker<TApplicant> faker, AllLookups lookups)
            where TApplicant : Applicant
        {
            var addressFaker = new Faker<ApplicantAddress>().ApplyApplicantAddressRules(lookups);
            var canAddressModel = addressFaker.Generate("default,Can");
            var usaAddressModel = addressFaker.Generate("default,Usa");
            var intlAddressModel = addressFaker.Generate("default,Intl");

            var countries = lookups.Countries;
            var canada = countries.Single(x => x.Code == Constants.Countries.Canada);
            var unitedStates = countries.Single(x => x.Code == Constants.Countries.UnitedStates);
            var intlCountries = countries.Where(x => x.Code != Constants.Countries.Canada && x.Code != Constants.Countries.UnitedStates);
            var firstGenerationApplicants = lookups.FirstGenerationApplicants;
            var firstLanguages = lookups.FirstLanguages;
            var genders = lookups.Genders;
            var preferredLanguages = lookups.PreferredLanguages;
            var preferredCorrespondenceMethods = lookups.PreferredCorrespondenceMethods;
            var canadianStatuses = lookups.CanadianStatuses;
            var sponsorAgencies = lookups.SponsorAgencies;
            var aboriginalStatuses = lookups.AboriginalStatuses.Where(x => x.ShowInPortal);
            var visaStatuses = lookups.VisaStatuses;

            var studyPermit = lookups.CanadianStatuses.Single(x => x.Code == Constants.CanadianStatuses.StudyPermit);

            return faker
                .RuleFor(u => u.Id, _ => Guid.NewGuid())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.PreferredName, f => f.Name.FirstName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName.Truncate(11), u.LastName.Truncate(11), "test.ocas.ca", "_" + DateTime.Today.ToString("yyyyMMdd") + "_test").ToLowerInvariant())
                .RuleFor(u => u.UserName, (_, u) => u.Email)
                .RuleFor(u => u.BirthDate, f => f.Date.Past(50, DateTime.UtcNow.AddYears(-18)).ToUniversalTime().ToStringOrDefault())
                .RuleFor(x => x.FirstGenerationId, f => f.PickRandom(firstGenerationApplicants).Id)
                .RuleFor(x => x.FirstLanguageId, f => f.PickRandom(firstLanguages).Id)
                .RuleFor(x => x.GenderId, f => f.PickRandom(genders).Id)
                .RuleFor(x => x.PreferredLanguageId, f => f.PickRandom(preferredLanguages).Id)
                .RuleFor(x => x.SponsorAgencyId, f => f.PickRandom(sponsorAgencies).Id)
                .RuleFor(x => x.AgreedToCasl, f => f.Random.Bool())
                .RuleFor(o => o.EnrolledInHighSchool, f => f.Random.Bool())
                .RuleFor(o => o.GraduatedHighSchool, (f, o) => o.EnrolledInHighSchool == false ? f.Random.Bool() : (bool?)null)
                .RuleFor(o => o.GraduationHighSchoolDate, (f, o) => o.EnrolledInHighSchool == true ? f.Date.Future(5).ToString(Constants.DateFormat.YearMonthDashed) : null)
                .RuleFor(x => x.IsAboriginalPerson, f => f.Random.Bool())
                .RuleFor(x => x.AboriginalStatuses, (f, u) => u.IsAboriginalPerson == true ? f.PickRandom(aboriginalStatuses, f.Random.Int(1, 4)).Select(x => x.Id).ToList() : null)
                .RuleFor(x => x.OtherAboriginalStatus, (f, u) =>
                {
                    if (u.AboriginalStatuses is null) return null;

                    var otherAboriginalStatusId = aboriginalStatuses.Single(x => x.Code == Constants.AboriginalStatuses.Other).Id;

                    return u.AboriginalStatuses.Any(x => x == otherAboriginalStatusId) ? f.Address.City() : null;
                })
                .RuleFor(x => x.CountryOfBirthId, f => f.PickRandom(countries).Id)
                .RuleFor(x => x.CountryOfCitizenshipId, f => f.PickRandom(countries).Id)
                .RuleFor(x => x.MailingAddress, (f, o) => intlCountries.Any(x => x.Id == o.CountryOfCitizenshipId) ?
                                                            intlAddressModel
                                                            : f.PickRandom(new List<ApplicantAddress> { usaAddressModel, canAddressModel }))
                .RuleFor(x => x.HomePhone, (f, o) => f.Random.Bool() ? intlCountries.Any(x => x.Id == o.MailingAddress.CountryId) ?
                                                            f.Random.String(15, '0', '9')
                                                            : f.Random.String(10, '0', '9')
                                                            : null)
                .RuleFor(x => x.MobilePhone, (f, o) => string.IsNullOrEmpty(o.HomePhone) ? intlCountries.Any(x => x.Id == o.MailingAddress.CountryId) ?
                                                            f.Random.String(15, '0', '9')
                                                            : f.Random.String(10, '0', '9')
                                                            : null)
                .RuleFor(o => o.DateOfArrival, (f, o) =>
                {
                    if (o.CountryOfBirthId == canada.Id)
                        return null;

                    var birthDate = o.BirthDate.ToDateTime();
                    return f.Random.Bool() ? null : f.Date.Between(birthDate, DateTime.UtcNow.ToDateInEstAsUtc()).ToStringOrDefault();
                })
                .RuleFor(o => o.StatusInCanadaId, (f, o) =>
                {
                    return o.CountryOfCitizenshipId == canada.Id
                        ? (Guid?)canadianStatuses.Single(x => x.Code == Constants.CanadianStatuses.CanadianCitizen).Id
                        : (Guid?)f.PickRandom(canadianStatuses.Where(x => x.Code != Constants.CanadianStatuses.CanadianCitizen)).Id;
                })
                .RuleFor(o => o.StatusOfVisaId, (f, o) => o.StatusInCanadaId != studyPermit.Id ? (Guid?)null : f.PickRandom(visaStatuses).Id)
                .RuleSet("Intl", set =>
                {
                    set.RuleFor(x => x.MailingAddress, _ => intlAddressModel)
                    .RuleFor(x => x.CountryOfBirthId, f => f.PickRandom(intlCountries).Id)
                    .RuleFor(x => x.CountryOfCitizenshipId, f => f.PickRandom(intlCountries).Id)
                    .RuleFor(o => o.DateOfArrival, (f, o) =>
                    {
                        var birthDate = o.BirthDate.ToDateTime();
                        return f.Random.Bool() ? null : f.Date.Between(birthDate, DateTime.UtcNow.ToDateInEstAsUtc()).ToStringOrDefault();
                    })
                    .RuleFor(x => x.HomePhone, f => f.Random.Bool() ? f.Random.String(15, '0', '9') : null)
                    .RuleFor(x => x.MobilePhone, (f, o) => string.IsNullOrEmpty(o.HomePhone) ? f.Random.String(15, '0', '9') : null)
                    .RuleFor(o => o.StatusInCanadaId, f => f.PickRandom(canadianStatuses.Where(x => x.Code != Constants.CanadianStatuses.CanadianCitizen)).Id)
                    .RuleFor(o => o.StatusOfVisaId, (f, o) => o.StatusInCanadaId != studyPermit.Id ? (Guid?)null : f.PickRandom(visaStatuses).Id);
                })
                .RuleSet("Can", set =>
                {
                    set.RuleFor(x => x.MailingAddress, _ => canAddressModel)
                    .RuleFor(x => x.CountryOfBirthId, _ => canada.Id)
                    .RuleFor(x => x.CountryOfCitizenshipId, _ => canada.Id)
                    .RuleFor(o => o.DateOfArrival, _ => null)
                    .RuleFor(x => x.HomePhone, f => f.Random.Bool() ? f.Random.String(10, '0', '9') : null)
                    .RuleFor(x => x.MobilePhone, (f, o) => string.IsNullOrEmpty(o.HomePhone) ? f.Random.String(10, '0', '9') : null)
                    .RuleFor(x => x.StatusInCanadaId, _ => canadianStatuses.Single(x => x.Code == Constants.CanadianStatuses.CanadianCitizen).Id)
                    .RuleFor(o => o.StatusOfVisaId, (f, o) => o.StatusInCanadaId != studyPermit.Id ? (Guid?)null : f.PickRandom(visaStatuses).Id);
                })
                .RuleSet("Usa", set =>
                {
                    set.RuleFor(x => x.MailingAddress, _ => usaAddressModel)
                    .RuleFor(x => x.CountryOfBirthId, _ => unitedStates.Id)
                    .RuleFor(x => x.CountryOfCitizenshipId, _ => unitedStates.Id)
                    .RuleFor(o => o.DateOfArrival, (f, o) =>
                    {
                        var birthDate = o.BirthDate.ToDateTime();
                        return f.Random.Bool() ? null : f.Date.Between(birthDate, DateTime.UtcNow.ToDateInEstAsUtc()).ToStringOrDefault();
                    })
                    .RuleFor(x => x.HomePhone, f => f.Random.Bool() ? f.Random.String(10, '0', '9') : null)
                    .RuleFor(x => x.MobilePhone, (f, o) => string.IsNullOrEmpty(o.HomePhone) ? f.Random.String(10, '0', '9') : null)
                    .RuleFor(o => o.StatusInCanadaId, f => f.PickRandom(canadianStatuses.Where(x => x.Code != Constants.CanadianStatuses.CanadianCitizen)).Id)
                    .RuleFor(o => o.StatusOfVisaId, (f, o) => o.StatusInCanadaId != studyPermit.Id ? (Guid?)null : f.PickRandom(visaStatuses).Id);
                });
        }
    }
}
