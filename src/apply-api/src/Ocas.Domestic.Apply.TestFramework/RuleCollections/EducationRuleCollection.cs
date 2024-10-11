using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework.RuleCollections
{
    public static class EducationRuleCollection
    {
        public static Faker<TEducation> ApplyEducationRules<TEducation>(this Faker<TEducation> faker, AllLookups lookups)
            where TEducation : EducationBase
        {
            var achievementLevels = lookups.AchievementLevels;
            var cities = lookups.Cities;
            var colleges = lookups.Colleges;
            var countries = lookups.Countries;
            var credentials = lookups.Credentials;
            var grades = lookups.Grades;
            var highSchools = lookups.HighSchools.Where(h => h.ShowInEducation);
            var instituteTypes = lookups.InstituteTypes;
            var provinceStates = lookups.ProvinceStates;
            var studyLevels = lookups.StudyLevels;
            var universities = lookups.Universities.Where(u => u.ShowInEducation);

            var canada = countries.Single(x => x.Code == Constants.Countries.Canada);
            var college = instituteTypes.Single(x => x.Code == Constants.InstituteTypes.College);
            var highSchool = instituteTypes.Single(x => x.Code == Constants.InstituteTypes.HighSchool);
            var ontario = provinceStates.Single(x => x.Code == Constants.Provinces.Ontario);
            var otherCredential = credentials.Single(x => x.Code == Constants.Credentials.Other);
            var university = instituteTypes.Single(x => x.Code == Constants.InstituteTypes.University);

            var allInstituteIds = new List<Guid>();
            allInstituteIds.AddRange(colleges.Select(x => x.Id).ToList());
            allInstituteIds.AddRange(universities.Select(x => x.Id).ToList());
            allInstituteIds.AddRange(highSchools.Select(x => x.Id).ToList());

            var provinces = lookups.ProvinceStates.Where(x => x.CountryId == canada.Id).ToList();

            return faker
                .RuleFor(x => x.ApplicantId, Guid.NewGuid)
                .RuleFor(x => x.AttendedFrom, f => f.Date.Past(10).ToString(Constants.DateFormat.YearMonthDashed))
                .RuleFor(x => x.CurrentlyAttending, f => f.Random.Bool())
                .RuleFor(x => x.AttendedTo, (f, o) => o.CurrentlyAttending == true ? null : f.Date.Between(o.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed), DateTime.UtcNow).ToString(Constants.DateFormat.YearMonthDashed))
                .RuleFor(x => x.AcademicUpgrade, false)
                .RuleFor(x => x.FirstNameOnRecord, _ => null)
                .RuleFor(x => x.LastNameOnRecord, _ => null)
                .RuleFor(x => x.OntarioEducationNumber, () => null)
                .RuleFor(x => x.OtherCredential, () => null)
                .RuleFor(x => x.StudentNumber, (f, o) => o.CountryId == canada.Id ? f.Random.AlphaNumeric(12) : null)
                .RuleFor(x => x.CountryId, (f, o) => o.AcademicUpgrade.GetValueOrDefault() ? canada.Id : f.PickRandom(countries).Id)
                .RuleFor(x => x.ProvinceId, (f, o) => o.CountryId == canada.Id ? (o.AcademicUpgrade.GetValueOrDefault() ? ontario.Id : f.PickRandom(provinces).Id) : (Guid?)null)
                .RuleFor(x => x.InstituteTypeId, (f, o) => o.AcademicUpgrade.GetValueOrDefault() ? college.Id : f.PickRandom(instituteTypes).Id)
                .RuleFor(x => x.InstituteId, (f, o) => o.CountryId == canada.Id ? (o.AcademicUpgrade.GetValueOrDefault() ? f.PickRandom(colleges).Id : (o.InstituteTypeId == college.Id ? f.PickRandom(colleges).Id : (o.InstituteTypeId == university.Id ? f.PickRandom(universities).Id : f.PickRandom(highSchools).Id))) : (Guid?)null)
                .RuleFor(x => x.InstituteName, (_, o) => instituteTypes.Single(y => y.Id == o.InstituteTypeId).Label + " Name")
                .RuleFor(x => x.CredentialId, (f, o) => o.InstituteTypeId == highSchool.Id ? (Guid?)null : f.PickRandom(credentials).Id)
                .RuleFor(x => x.Major, (_, o) => o.InstituteTypeId == highSchool.Id ? null : $"{instituteTypes.Single(y => y.Id == o.InstituteTypeId).Label} Major")
                .RuleFor(x => x.LevelAchievedId, (f, o) => (o.CountryId == canada.Id && o.InstituteTypeId == highSchool.Id) ? (Guid?)null : f.PickRandom(achievementLevels).Id)
                .RuleFor(x => x.LevelOfStudiesId, (f, o) => (o.CountryId == canada.Id && o.InstituteTypeId == university.Id) ? f.PickRandom(studyLevels).Id : (Guid?)null)
                .RuleFor(x => x.LastGradeCompletedId, (f, o) => (o.CountryId == canada.Id && o.InstituteTypeId == highSchool.Id) ? (o.CurrentlyAttending == false ? f.PickRandom(grades).Id : (Guid?)null) : null)
                .RuleFor(x => x.Graduated, (f, o) => (o.InstituteTypeId == highSchool.Id && o.CountryId == canada.Id) ? (o.CurrentlyAttending == true ? false : f.Random.Bool()) : (bool?)null)
                .RuleFor(x => x.CityId, (f, o) => (o.InstituteTypeId == highSchool.Id && o.CountryId == canada.Id && o.ProvinceId != ontario.Id) ? f.PickRandom(cities.Where(y => y.ProvinceId == o.ProvinceId).ToList()).Id : (Guid?)null)
                .RuleSet("AcademicUpgrading", set =>
                {
                    set.RuleFor(x => x.AcademicUpgrade, true)
                       .RuleFor(x => x.InstituteId, f => f.PickRandom(colleges).Id)
                       .RuleFor(x => x.InstituteName, (_, o) => colleges.Single(x => x.Id == o.InstituteId).Name)
                       .RuleFor(x => x.TranscriptFee, (_, o) => colleges.Single(x => x.Id == o.InstituteId).HasEtms ? colleges.Single(x => x.Id == o.InstituteId).TranscriptFee ?? 0 : (decimal?)null)
                       .RuleFor(x => x.Address, (_, o) => colleges.Single(x => x.Id == o.InstituteId).Address)
                       .RuleFor(x => x.StudentNumber, f => f.Random.AlphaNumeric(20))
                       .RuleFor(x => x.CountryId, canada.Id)
                       .RuleFor(x => x.InstituteTypeId, college.Id)
                       .RuleFor(x => x.ProvinceId, ontario.Id)
                       .RuleFor(x => x.CredentialId, () => null)
                       .RuleFor(x => x.LevelAchievedId, () => null)
                       .RuleFor(x => x.Major, () => null);
                })
                .RuleSet("University", set =>
                {
                    set.RuleFor(x => x.CredentialId, (f, _) => f.PickRandom(credentials).Id)
                       .RuleFor(x => x.InstituteTypeId, university.Id)
                       .RuleFor(x => x.InstituteId, (f, o) => o.ProvinceId == ontario.Id ? f.PickRandom(universities).Id : (Guid?)null)
                       .RuleFor(x => x.InstituteName, (_, o) => o.InstituteId.IsEmpty() ? instituteTypes.Single(y => y.Id == o.InstituteTypeId).Label + " Name" : universities.Single(x => x.Id == o.InstituteId).Name)
                       .RuleFor(x => x.TranscriptFee, (_, o) => o.InstituteId.IsEmpty() ? null : universities.Single(x => x.Id == o.InstituteId).HasEtms ? universities.Single(x => x.Id == o.InstituteId).TranscriptFee ?? 0 : (decimal?)null)
                       .RuleFor(x => x.Address, (_, o) => o.InstituteId.IsEmpty() ? null : universities.Single(i => i.Id == o.InstituteId).Address)
                       .RuleFor(x => x.LevelOfStudiesId, f => f.PickRandom(studyLevels).Id)
                       .RuleFor(x => x.Major, _ => "University Major")
                       .RuleFor(x => x.OtherCredential, (_, o) => o.CredentialId == otherCredential.Id ? "Other Credential Type" : null)
                       .RuleFor(x => x.StudentNumber, (f, o) => o.ProvinceId == ontario.Id ? f.Random.AlphaNumeric(20) : null)
                       .RuleFor(x => x.LevelAchievedId, () => null);
                })
                .RuleSet("College", set =>
                {
                    set.RuleFor(x => x.CredentialId, (f, _) => f.PickRandom(credentials).Id)
                       .RuleFor(x => x.InstituteTypeId, college.Id)
                       .RuleFor(x => x.InstituteId, (f, o) => o.ProvinceId == ontario.Id ? f.PickRandom(colleges).Id : (Guid?)null)
                       .RuleFor(x => x.InstituteName, (_, o) => o.InstituteId.IsEmpty() ? instituteTypes.Single(y => y.Id == o.InstituteTypeId).Label + " Name" : colleges.Single(x => x.Id == o.InstituteId).Name)
                       .RuleFor(x => x.TranscriptFee, (_, o) => o.InstituteId.IsEmpty() ? null : colleges.Single(x => x.Id == o.InstituteId).HasEtms ? colleges.Single(x => x.Id == o.InstituteId).TranscriptFee ?? 0 : (decimal?)null)
                       .RuleFor(x => x.Address, (_, o) => o.InstituteId.IsEmpty() ? null : colleges.Single(i => i.Id == o.InstituteId).Address)
                       .RuleFor(x => x.Major, _ => "College Major")
                       .RuleFor(x => x.OtherCredential, (_, o) => o.CredentialId == otherCredential.Id ? "Other Credential Type" : null)
                       .RuleFor(x => x.StudentNumber, (f, o) => o.ProvinceId == ontario.Id ? f.Random.AlphaNumeric(20) : null)
                       .RuleFor(x => x.LevelAchievedId, () => null);
                })
                .RuleSet("Highschool", set =>
                {
                    set.RuleFor(x => x.InstituteTypeId, _ => highSchool.Id)
                        .RuleFor(x => x.CityId, (f, o) => (o.CountryId == canada.Id && o.ProvinceId != ontario.Id) ? f.PickRandom(cities.Where(y => y.ProvinceId == o.ProvinceId).ToList()).Id : (Guid?)null)
                        .RuleFor(x => x.InstituteId, (f, o) => o.ProvinceId == ontario.Id ? f.PickRandom(highSchools).Id : (Guid?)null)
                        .RuleFor(x => x.InstituteName, (_, o) => o.InstituteId.IsEmpty() ? "High School Name" : highSchools.Single(y => y.Id == o.InstituteId).Name)
                        .RuleFor(x => x.Address, (_, o) => o.InstituteId.IsEmpty() ? null : highSchools.Single(i => i.Id == o.InstituteId).Address)
                        .RuleFor(x => x.TranscriptFee, (_, o) => o.InstituteId.IsEmpty() ? null : highSchools.Single(x => x.Id == o.InstituteId).HasEtms ? highSchools.Single(x => x.Id == o.InstituteId).TranscriptFee ?? 0 : (decimal?)null)
                        .RuleFor(x => x.Graduated, (f, o) => o.CountryId == canada.Id ? (o.CurrentlyAttending == true ? false : f.Random.Bool()) : (bool?)null)
                        .RuleFor(x => x.LastGradeCompletedId, (f, o) => o.CountryId == canada.Id ? (o.CurrentlyAttending == false ? f.PickRandom(grades).Id : (Guid?)null) : null)
                        .RuleFor(x => x.LevelOfStudiesId, _ => null)
                        .RuleFor(x => x.OntarioEducationNumber, (_, o) => o.CountryId == canada.Id && o.ProvinceId == ontario.Id ? Constants.Education.DefaultOntarioEducationNumber : null)
                        .RuleFor(x => x.StudentNumber, (f, o) => o.CountryId == canada.Id ? f.Random.AlphaNumeric(12) : null)
                        .RuleFor(x => x.FirstNameOnRecord, (f, o) => o.CountryId == canada.Id ? f.Person.FirstName : null)
                        .RuleFor(x => x.LastNameOnRecord, (f, o) => o.CountryId == canada.Id ? f.Person.LastName : null)
                        .RuleFor(x => x.LevelAchievedId, (f, o) => o.CountryId == canada.Id ? (Guid?)null : f.PickRandom(achievementLevels).Id)
                        .RuleFor(x => x.CredentialId, () => null)
                        .RuleFor(x => x.Major, () => null);
                })
                .RuleSet("PostSecondary", set =>
                {
                    set.RuleFor(x => x.InstituteTypeId, f => f.PickRandom(new List<Guid> { college.Id, university.Id }))
                        .RuleFor(x => x.Major, _ => "PostSecondary Major")
                        .RuleFor(x => x.CredentialId, f => f.PickRandom(credentials).Id)
                        .RuleFor(x => x.OtherCredential, (_, o) => o.CredentialId == otherCredential.Id ? "Other Credential Type" : null);
                })
                .RuleSet("Intl", set =>
                {
                    set.RuleFor(x => x.CountryId, f => f.PickRandom(countries.Where(y => y.Id != canada.Id)).Id)
                        .RuleFor(x => x.ProvinceId, () => null)
                        .RuleFor(x => x.InstituteId, () => null)
                        .RuleFor(x => x.LevelAchievedId, f => f.PickRandom(achievementLevels).Id);
                })
                .RuleSet("Canadian", set =>
                {
                    set.RuleFor(x => x.CountryId, canada.Id)
                        .RuleFor(x => x.ProvinceId, f => f.PickRandom(provinces.Where(x => x.Id != ontario.Id)).Id);
                })
                .RuleSet("Ontario", set => set.RuleFor(x => x.ProvinceId, ontario.Id));
        }
    }
}
