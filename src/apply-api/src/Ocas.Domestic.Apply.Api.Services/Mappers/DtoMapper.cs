using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public class DtoMapper : IDtoMapper
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly IDomesticContext _domesticContext;
        private readonly IMapper _mapper;

        public DtoMapper(ILookupsCache lookupsCache, IDomesticContext domesticContext, IMapper mapper)
        {
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task PatchContact(Dto.Contact dbDto, Applicant model)
        {
            var countries = await _lookupsCache.GetCountries(Constants.Localization.EnglishCanada);

            dbDto.CountryOfBirthId = model.CountryOfBirthId;
            dbDto.CountryOfCitizenshipId = model.CountryOfCitizenshipId;
            dbDto.Email = model.Email;
            dbDto.FirstGenerationId = model.FirstGenerationId;
            dbDto.FirstLanguageId = model.FirstLanguageId;
            dbDto.GenderId = model.GenderId;
            dbDto.HighSchoolEnrolled = model.EnrolledInHighSchool;
            dbDto.HighSchoolGraduated = model.GraduatedHighSchool;
            dbDto.HighSchoolGraduationDate = model.GraduationHighSchoolDate.IsDate(Constants.DateFormat.YearMonthDashed) ? model.GraduationHighSchoolDate.ToNullableDateTime(Constants.DateFormat.YearMonthDashed) : null;
            dbDto.MiddleName = model.MiddleName;
            dbDto.PreferredLanguageId = model.PreferredLanguageId;
            dbDto.PreferredName = model.PreferredName;
            dbDto.PreviousLastName = model.PreviousLastName;
            dbDto.SponsorAgencyId = model.SponsorAgencyId;
            dbDto.TitleId = model.TitleId;

            dbDto.HomePhone = model.HomePhone;
            if (string.IsNullOrWhiteSpace(model.HomePhone))
            {
                dbDto.HomePhone = model.MobilePhone;
            }

            dbDto.MobilePhone = model.MobilePhone;

            dbDto.DoNotSendMM = true;
            if (model.AgreedToCasl.HasValue)
            {
                dbDto.DoNotSendMM = !model.AgreedToCasl.Value;
            }

            dbDto.IsAboriginalPerson = model.IsAboriginalPerson;
            dbDto.AboriginalStatusId = null;
            dbDto.OtherAboriginalStatus = null;
            if (model.IsAboriginalPerson == true)
            {
                if (model.AboriginalStatuses?.Count > 0)
                {
                    var aboriginalStatuses = await _lookupsCache.GetAboriginalStatuses(Constants.Localization.EnglishCanada);
                    var selectedAboriginalStatuses = aboriginalStatuses.Where(x => model.AboriginalStatuses.Contains(x.Id));

                    var mask = 0;

                    foreach (var aboriginalStatus in selectedAboriginalStatuses)
                    {
                        var maskValue = Convert.ToInt32(aboriginalStatus.ColtraneCode, 2);
                        mask |= maskValue;
                    }

                    var coltraneCode = Convert.ToString(mask, 2).PadLeft(4, '0');
                    var crmAboriginalStatus = aboriginalStatuses.Single(x => x.ColtraneCode == coltraneCode);

                    dbDto.AboriginalStatusId = crmAboriginalStatus.Id;
                }

                // Map OtherAboriginalStatus only if they select it
                if (dbDto.AboriginalStatusId.HasValue)
                {
                    var aboriginalStatuses = await _lookupsCache.GetAboriginalStatuses(Constants.Localization.EnglishCanada);
                    var selectedAboriginalStatus = aboriginalStatuses.Single(x => x.Id == dbDto.AboriginalStatusId);
                    var otherAboriginalStatus = aboriginalStatuses.Single(x => x.Code == Constants.AboriginalStatuses.Other);

                    var selectedMask = Convert.ToInt32(selectedAboriginalStatus.ColtraneCode, 2);
                    var otherMask = Convert.ToInt32(otherAboriginalStatus.ColtraneCode, 2);
                    if ((selectedMask & otherMask) != 0)
                    {
                        dbDto.OtherAboriginalStatus = model.OtherAboriginalStatus;
                    }
                }
            }

            dbDto.DateOfArrival = null;
            if (model.CountryOfBirthId != null)
            {
                var birthCountry = countries.Single(x => x.Id == model.CountryOfBirthId);
                dbDto.DateOfArrival = birthCountry.Code == Constants.Countries.Canada ? null : model.DateOfArrival.ToNullableDateTime();
            }

            dbDto.StatusInCanadaId = null;
            dbDto.StatusOfVisaId = null;
            if (model.CountryOfCitizenshipId != null)
            {
                var citizenshipCountry = countries.Single(x => x.Id == model.CountryOfCitizenshipId);
                var canadianStatuses = await _lookupsCache.GetCanadianStatuses(Constants.Localization.EnglishCanada);
                var canadianCitizen = canadianStatuses.Single(x => x.Code == Constants.CanadianStatuses.CanadianCitizen);

                dbDto.StatusInCanadaId = citizenshipCountry.Code == Constants.Countries.Canada ? canadianCitizen.Id : model.StatusInCanadaId;

                if (model.StatusInCanadaId != null)
                {
                    var statusInCanada = canadianStatuses.Single(x => x.Id == model.StatusInCanadaId);

                    if (statusInCanada.Code == Constants.CanadianStatuses.StudyPermit)
                    {
                        dbDto.StatusOfVisaId = citizenshipCountry.Code == Constants.Countries.Canada ? null : model.StatusOfVisaId;
                    }
                }
            }

            dbDto.MailingAddress = null;
            if (model.MailingAddress != null)
            {
                dbDto.MailingAddress = new Dto.Address
                {
                    City = model.MailingAddress.City,
                    PostalCode = model.MailingAddress.PostalCode,
                    Street = model.MailingAddress.Street,
                    Verified = model.MailingAddress.Verified
                };

                // Map Country and ProvinceState Ids to Name/Code because CRM
                var country = countries.Single(x => x.Id == model.MailingAddress.CountryId);
                dbDto.MailingAddress.Country = country.Name;

                if (country.Code == Constants.Countries.Canada || country.Code == Constants.Countries.UnitedStates)
                {
                    var provinceStates = await _lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
                    var provinceState = provinceStates.Single(x => x.Id == model.MailingAddress.ProvinceStateId);

                    dbDto.MailingAddress.ProvinceState = provinceState.Code;
                }
                else
                {
                    dbDto.MailingAddress.ProvinceState = null;
                }
            }

            // Always set PreferredCorrespondenceMethodId to Email
            var correspondenceMethods = await _lookupsCache.GetPreferredCorrespondenceMethods(Constants.Localization.EnglishCanada);
            var emailMethod = correspondenceMethods.Single(x => x.Code == Constants.PreferredCorrespondenceMethods.Email);
            dbDto.PreferredCorrespondenceMethodId = emailMethod.Id;
        }

        public async Task PatchEducation(Dto.EducationBase dbDto, EducationBase model)
        {
            var educationType = model.GetEducationType(await _lookupsCache.GetCountries(Constants.Localization.FallbackLocalization), await _lookupsCache.GetInstituteTypes(Constants.Localization.FallbackLocalization));
            var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
            var credentials = await _lookupsCache.GetCredentials(Constants.Localization.EnglishCanada);
            var provinceStates = await _lookupsCache.GetProvinceStates(Constants.Localization.EnglishCanada);
            var ontario = provinceStates.First(x => x.Code == Constants.Provinces.Ontario);

            switch (educationType)
            {
                case EducationType.International:
                    var instituteTypes = await _lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada);

                    dbDto.AttendedFrom = model.AttendedFrom;
                    dbDto.ApplicantId = model.ApplicantId;
                    dbDto.CountryId = model.CountryId;
                    dbDto.CurrentlyAttending = model.CurrentlyAttending;
                    dbDto.InstituteName = model.InstituteName;
                    dbDto.InstituteTypeId = model.InstituteTypeId;
                    dbDto.LevelAchievedId = model.LevelAchievedId;

                    dbDto.Major = null;
                    dbDto.CredentialId = null;
                    if (instituteTypes.FirstOrDefault(x => x.Id == model.InstituteTypeId)?.Code != Constants.InstituteTypes.HighSchool)
                    {
                        dbDto.Major = model.Major;
                        dbDto.CredentialId = model.CredentialId;
                    }

                    dbDto.AttendedTo = null;
                    if (model.CurrentlyAttending == false)
                    {
                        dbDto.AttendedTo = model.AttendedTo;
                    }

                    dbDto.AcademicUpgrade = false;
                    dbDto.CityId = null;
                    dbDto.FirstNameOnRecord = null;
                    dbDto.InstituteId = null;
                    dbDto.LastGradeCompletedId = null;
                    dbDto.LastNameOnRecord = null;
                    dbDto.LevelOfStudiesId = null;
                    dbDto.OntarioEducationNumber = null;
                    dbDto.OtherCredential = null;
                    dbDto.ProvinceId = null;
                    dbDto.StudentNumber = null;
                    break;

                case EducationType.AcademicUpgrading:
                    dbDto.AttendedFrom = model.AttendedFrom;
                    dbDto.ApplicantId = model.ApplicantId;
                    dbDto.CountryId = model.CountryId;
                    dbDto.CurrentlyAttending = model.CurrentlyAttending;
                    dbDto.InstituteId = model.InstituteId;
                    dbDto.InstituteName = colleges.FirstOrDefault(x => x.Id == model.InstituteId)?.Name
                        ?? (await _domesticContext.GetCollege(model.InstituteId.Value))?.Name;
                    dbDto.InstituteTypeId = model.InstituteTypeId;
                    dbDto.ProvinceId = model.ProvinceId;
                    dbDto.StudentNumber = model.StudentNumber;

                    dbDto.AttendedTo = null;
                    if (model.CurrentlyAttending == false)
                    {
                        dbDto.AttendedTo = model.AttendedTo;
                    }

                    dbDto.AcademicUpgrade = true;
                    dbDto.CityId = null;
                    dbDto.CredentialId = null;
                    dbDto.FirstNameOnRecord = null;
                    dbDto.LastGradeCompletedId = null;
                    dbDto.LastNameOnRecord = null;
                    dbDto.LevelAchievedId = null;
                    dbDto.LevelOfStudiesId = null;
                    dbDto.Major = null;
                    dbDto.OntarioEducationNumber = null;
                    dbDto.OtherCredential = null;
                    break;

                case EducationType.CanadianUniversity:

                    dbDto.AttendedFrom = model.AttendedFrom;
                    dbDto.ApplicantId = model.ApplicantId;
                    dbDto.CountryId = model.CountryId;
                    dbDto.CredentialId = model.CredentialId;
                    dbDto.CurrentlyAttending = model.CurrentlyAttending;
                    dbDto.InstituteTypeId = model.InstituteTypeId;
                    dbDto.LevelOfStudiesId = model.LevelOfStudiesId;
                    dbDto.Major = model.Major;
                    dbDto.ProvinceId = model.ProvinceId;

                    // from A2C: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Services%2FPostSecondaryEducation%2FPostSecondaryEducationService.cs&version=GBmaster&line=167&lineStyle=plain&lineEnd=173&lineStartColumn=13&lineEndColumn=14
                    dbDto.StudentNumber = string.Empty;
                    if (model.ProvinceId == ontario.Id)
                    {
                        dbDto.StudentNumber = model.StudentNumber;
                    }

                    dbDto.OtherCredential = null;
                    if (credentials.First(x => x.Id == model.CredentialId).Code == Constants.Credentials.Other)
                    {
                        dbDto.OtherCredential = model.OtherCredential;
                    }

                    dbDto.InstituteId = null;
                    dbDto.InstituteName = model.InstituteName;
                    if (!model.InstituteId.IsEmpty())
                    {
                        var universities = await _lookupsCache.GetUniversities();

                        dbDto.InstituteId = model.InstituteId;
                        dbDto.InstituteName = universities.FirstOrDefault(x => x.Id == model.InstituteId)?.Name
                        ?? (await _domesticContext.GetUniversity(model.InstituteId.Value))?.Name;
                    }

                    dbDto.AttendedTo = null;
                    if (model.CurrentlyAttending == false)
                    {
                        dbDto.AttendedTo = model.AttendedTo;
                    }

                    dbDto.AcademicUpgrade = false;
                    dbDto.CityId = null;
                    dbDto.FirstNameOnRecord = null;
                    dbDto.LastGradeCompletedId = null;
                    dbDto.LastNameOnRecord = null;
                    dbDto.LevelAchievedId = null;
                    dbDto.OntarioEducationNumber = null;
                    break;

                case EducationType.CanadianCollege:
                    dbDto.AttendedFrom = model.AttendedFrom;
                    dbDto.ApplicantId = model.ApplicantId;
                    dbDto.CountryId = model.CountryId;
                    dbDto.CredentialId = model.CredentialId;
                    dbDto.CurrentlyAttending = model.CurrentlyAttending;
                    dbDto.InstituteTypeId = model.InstituteTypeId;
                    dbDto.Major = model.Major;
                    dbDto.ProvinceId = model.ProvinceId;

                    // from A2C: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Services%2FPostSecondaryEducation%2FPostSecondaryEducationService.cs&version=GBmaster&line=167&lineStyle=plain&lineEnd=173&lineStartColumn=13&lineEndColumn=14
                    dbDto.StudentNumber = string.Empty;
                    if (model.ProvinceId == ontario.Id)
                    {
                        dbDto.StudentNumber = model.StudentNumber;
                    }

                    dbDto.OtherCredential = null;
                    if (credentials.First(x => x.Id == model.CredentialId).Code == Constants.Credentials.Other)
                    {
                        dbDto.OtherCredential = model.OtherCredential;
                    }

                    dbDto.InstituteId = null;
                    dbDto.InstituteName = model.InstituteName;
                    if (!model.InstituteId.IsEmpty())
                    {
                        dbDto.InstituteId = model.InstituteId;
                        dbDto.InstituteName = colleges.FirstOrDefault(x => x.Id == model.InstituteId)?.Name
                        ?? (await _domesticContext.GetCollege(model.InstituteId.Value))?.Name;
                    }

                    dbDto.AttendedTo = null;
                    if (model.CurrentlyAttending == false)
                    {
                        dbDto.AttendedTo = model.AttendedTo;
                    }

                    dbDto.AcademicUpgrade = false;
                    dbDto.LevelOfStudiesId = null;
                    dbDto.CityId = null;
                    dbDto.FirstNameOnRecord = null;
                    dbDto.LastGradeCompletedId = null;
                    dbDto.LastNameOnRecord = null;
                    dbDto.LevelAchievedId = null;
                    dbDto.OntarioEducationNumber = null;
                    break;

                case EducationType.CanadianHighSchool:
                    dbDto.AttendedFrom = model.AttendedFrom;
                    dbDto.ApplicantId = model.ApplicantId;
                    dbDto.CityId = model.CityId;
                    dbDto.CountryId = model.CountryId;
                    dbDto.CurrentlyAttending = model.CurrentlyAttending;
                    dbDto.FirstNameOnRecord = model.FirstNameOnRecord;
                    dbDto.InstituteTypeId = model.InstituteTypeId;
                    dbDto.LastGradeCompletedId = model.LastGradeCompletedId;
                    dbDto.LastNameOnRecord = model.LastNameOnRecord;
                    dbDto.LevelAchievedId = model.LevelAchievedId;
                    dbDto.ProvinceId = model.ProvinceId;
                    dbDto.StudentNumber = model.StudentNumber;

                    dbDto.AttendedTo = null;
                    if (model.CurrentlyAttending == false)
                    {
                        dbDto.AttendedTo = model.AttendedTo;
                    }

                    dbDto.InstituteId = null;
                    dbDto.InstituteName = model.InstituteName;
                    if (!model.InstituteId.IsEmpty())
                    {
                        var highSchools = await _lookupsCache.GetHighSchools(Constants.Localization.EnglishCanada);
                        var highSchool = highSchools.FirstOrDefault(x => x.Id == model.InstituteId)
                        ?? _mapper.Map<HighSchool>(await _domesticContext.GetHighSchool(model.InstituteId.Value, Domestic.Enums.Locale.English));

                        if (highSchool is null) throw new Common.Exceptions.ValidationException("High School not found");

                        dbDto.InstituteId = model.InstituteId;
                        dbDto.InstituteName = highSchool.Name;

                        if (model.ProvinceId == ontario.Id)
                        {
                            var cities = await _lookupsCache.GetCities(Constants.Localization.EnglishCanada);
                            var city = cities.FirstOrDefault(x => x.ProvinceId == dbDto.ProvinceId && x.Name == highSchool.Address.City);

                            if (city is null) throw new Common.Exceptions.ValidationException($"{highSchool.Address.City} not found");

                            dbDto.CityId = city.Id;
                        }
                    }

                    dbDto.Graduated = model.Graduated;
                    if (model.CurrentlyAttending == true)
                    {
                        dbDto.LevelAchievedId = null;
                        dbDto.Graduated = false;
                    }

                    if (model.ProvinceId == ontario.Id)
                    {
                        dbDto.OntarioEducationNumber = model.OntarioEducationNumber;
                    }

                    dbDto.AcademicUpgrade = false;
                    dbDto.CredentialId = null;
                    dbDto.LevelOfStudiesId = null;
                    dbDto.Major = null;
                    dbDto.OtherCredential = null;
                    break;
            }
        }

        public void PatchProgramChoice(Dto.ProgramChoiceBase dbDto, ProgramChoiceBase model)
        {
            dbDto.ApplicationId = model.ApplicationId;
            dbDto.ApplicantId = model.ApplicantId;
            dbDto.EntryLevelId = model.EntryLevelId;
            dbDto.ProgramIntakeId = model.IntakeId;
            dbDto.PreviousYearApplied = model.PreviousYearApplied;
            dbDto.PreviousYearAttended = model.PreviousYearAttended;
        }
    }
}
