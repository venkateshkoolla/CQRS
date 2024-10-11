using System;
using System.Linq;
using Bogus;
using Ocas.Domestic.Data.TestFramework.Extensions;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Data.TestFramework.RuleCollections
{
    public static class ContactRuleCollection
    {
        public static Faker<TContactBase> ApplyContactBaseRules<TContactBase>(this Faker<TContactBase> faker, SeedDataFixture seedDataFixture)
            where TContactBase : Models.ContactBase
        {
            var activeAccountStatus = seedDataFixture.AccountStatuses.First(s => s.Code == TestConstants.Contact.AccountStatuses.Active);

            return faker
                .RuleFor(o => o.FirstName, f => f.Person.FirstName)
                .RuleFor(o => o.LastName, f => f.Person.LastName)
                .RuleFor(o => o.PreferredName, f => f.Person.FirstName)
                .RuleFor(o => o.Email, (f, o) => f.Internet.Email(o.FirstName, o.LastName, "test.ocas.ca", DateTime.Today.ToString("_yyyyMMdd")).ToLowerInvariant())
                .RuleFor(o => o.Username, (_, u) => u.Email)
                .RuleFor(o => o.SubjectId, _ => Guid.NewGuid().ToString())
                .RuleFor(o => o.BirthDate, (f, _) =>
                {
                    var birthDate = f.Person.DateOfBirth;
                    birthDate = new DateTime(birthDate.Year, birthDate.Month, birthDate.Day, 0, 0, 0, DateTimeKind.Unspecified);
                    birthDate = TimeZoneInfo.ConvertTimeToUtc(birthDate, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));

                    return DateTime.SpecifyKind(birthDate, DateTimeKind.Utc);
                })
                .RuleFor(o => o.LastLogin, (f, _) =>
                {
                    var lastLogin = f.Date.Recent();
                    lastLogin = new DateTime(lastLogin.Year, lastLogin.Month, lastLogin.Day, 0, 0, 0, DateTimeKind.Unspecified);
                    lastLogin = TimeZoneInfo.ConvertTimeFromUtc(lastLogin, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                    return DateTime.SpecifyKind(lastLogin, DateTimeKind.Utc);
                })
                .RuleFor(o => o.SourceId, f => f.PickRandom(seedDataFixture.Sources).Id)
                .RuleFor(o => o.SourcePartnerId, f => f.PickRandom(seedDataFixture.Colleges).Id)
                .RuleFor(o => o.AccountStatusId, _ => activeAccountStatus.Id)
                .RuleFor(o => o.ContactType, f => f.PickRandom<ContactType>())
                .RuleFor(o => o.AcceptedPrivacyStatementId, (f, _) => f.Random.Bool() ? (Guid?)f.PickRandom(seedDataFixture.PrivacyStatements).Id : null)
                .RuleFor(o => o.PreferredLanguageId, f => f.PickRandom(seedDataFixture.PreferredLanguages).Id)
                .RuleSet("Applicant", set => set.RuleFor(o => o.ContactType, _ => ContactType.Applicant));
        }

        public static Faker<TContact> ApplyContactRules<TContact>(this Faker<TContact> faker, SeedDataFixture seedDataFixture)
            where TContact : Models.Contact
        {
            var addressFaker = new Faker<Models.Address>().ApplyAddressRules(seedDataFixture);

            var canada = seedDataFixture.Countries.Single(x => x.Name == "Canada");
            var otherAboriginalStatus = seedDataFixture.AboriginalStatuses.Single(x => x.Code == TestConstants.AboriginalStatuses.Other);
            var otherMask = Convert.ToInt32(otherAboriginalStatus.ColtraneCode, 2);
            var otherAboriginalStatusIds = seedDataFixture.AboriginalStatuses.Where(x => (Convert.ToInt32(x.ColtraneCode, 2) & otherMask) != 0).Select(x => x.Id).ToList();

            return faker
                .ApplyContactBaseRules(seedDataFixture)
                .RuleFor(o => o.ContactType, _ => ContactType.Applicant)
                .RuleFor(o => o.MiddleName, f => f.Person.LastName)
                .RuleFor(o => o.PreviousLastName, f => f.Person.LastName)
                .RuleFor(o => o.PaymentLocked, _ => false)
                .RuleFor(x => x.GenderId, f => f.PickRandom(seedDataFixture.Genders).Id)
                .RuleFor(x => x.FirstGenerationId, f => f.PickRandom(seedDataFixture.FirstGenerationApplicants).Id)
                .RuleFor(x => x.FirstLanguageId, f => f.PickRandom(seedDataFixture.FirstLanguages).Id)
                .RuleFor(o => o.PreferredCorrespondenceMethodId, f => f.PickRandom(seedDataFixture.PreferredCorrespondenceMethods).Id)
                .RuleFor(o => o.CountryOfBirthId, f => f.PickRandom(seedDataFixture.Countries).Id)
                .RuleFor(o => o.CountryOfCitizenshipId, f => f.PickRandom(seedDataFixture.Countries).Id)
                .RuleFor(o => o.DateOfArrival, (f, o) =>
                {
                    if (o.CountryOfBirthId == canada.Id)
                        return null;

                    return f.Date.Between(o.BirthDate, DateTime.UtcNow.ToDateInEstAsUtc()).ToDateInEstAsUtc();
                })
                .RuleFor(o => o.HomePhone, f => f.Random.String(10, '0', '9'))
                .RuleFor(o => o.MobilePhone, f => f.Random.Bool() ? f.Random.String(10, '0', '9') : null)
                .RuleFor(o => o.IsAboriginalPerson, f => f.Random.Bool())
                .RuleFor(o => o.AboriginalStatusId, (f, o) => o.IsAboriginalPerson == true ? (Guid?)f.PickRandom(seedDataFixture.AboriginalStatuses).Id : null)
                .RuleFor(o => o.OtherAboriginalStatus, (f, o) => otherAboriginalStatusIds.Any(x => x == o.AboriginalStatusId) ? f.Random.Word() : null)
                .RuleFor(o => o.StatusInCanadaId, f => f.PickRandom(seedDataFixture.CanadianStatuses).Id)
                .RuleFor(o => o.StatusOfVisaId, (f, o) =>
                {
                    var canadianStatus = seedDataFixture.CanadianStatuses.FirstOrDefault(x => x.Id == o.StatusInCanadaId);

                    if (canadianStatus?.Code != "4") return null;

                    return f.PickRandom(seedDataFixture.StatusOfVisas).Id;
                })
                .RuleFor(o => o.SponsorAgencyId, f => f.PickRandom(seedDataFixture.PreferredSponsorAgencies).Id)
                .RuleFor(o => o.HighSchoolEnrolled, f => f.Random.Bool())
                .RuleFor(o => o.HighSchoolGraduated, (f, o) => o.HighSchoolEnrolled == false ? f.Random.Bool() : (bool?)null)
                .RuleFor(o => o.HighSchoolGraduationDate, (f, o) =>
                {
                    if (o.HighSchoolEnrolled == true)
                    {
                        var graduationDate = f.Date.Future();
                        graduationDate = new DateTime(graduationDate.Year, graduationDate.Month, graduationDate.Day, 0, 0, 0, DateTimeKind.Unspecified);
                        graduationDate = TimeZoneInfo.ConvertTimeToUtc(graduationDate, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"));
                        return DateTime.SpecifyKind(graduationDate, DateTimeKind.Utc);
                    }

                    return null;
                })
                .RuleFor(o => o.MailingAddress, _ => addressFaker.Generate());
        }
    }
}
