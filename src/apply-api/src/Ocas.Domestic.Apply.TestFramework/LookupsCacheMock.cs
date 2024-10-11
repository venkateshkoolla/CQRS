using Moq;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.TestFramework
{
    public class LookupsCacheMock : Mock<ILookupsCache>
    {
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly AllLookups _allLookups;

        public LookupsCacheMock()
        {
            _modelFakerFixture = new ModelFakerFixture();
            _allLookups = _modelFakerFixture.AllApplyLookups;
            SetupLookupData();
        }

        private void SetupLookupData()
        {
            Setup(m => m.GetAboriginalStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.AboriginalStatuses);
            Setup(m => m.GetAccountStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.AccountStatuses);
            Setup(m => m.GetAchievementLevels(It.IsAny<string>())).ReturnsAsync(_allLookups.AchievementLevels);
            Setup(m => m.GetApplicationCycles()).ReturnsAsync(_allLookups.ApplicationCycles);
            Setup(m => m.GetApplicationCyclesDto()).ReturnsAsync(_modelFakerFixture.SeedDataFixture.ApplicationCycles);
            Setup(m => m.GetApplicationCycleStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.ApplicationCycleStatuses);
            Setup(m => m.GetApplicationStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.ApplicationStatuses);
            Setup(m => m.GetBasisForAdmissions(It.IsAny<string>())).ReturnsAsync(_allLookups.BasisForAdmissions);
            Setup(m => m.GetBasisForAdmissionsDto(It.IsAny<string>())).ReturnsAsync(_modelFakerFixture.SeedDataFixture.BasisForAdmissions);
            Setup(m => m.GetCampuses()).ReturnsAsync(_allLookups.Campuses);
            Setup(m => m.GetCanadianStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.CanadianStatuses);
            Setup(m => m.GetCities(It.IsAny<string>())).ReturnsAsync(_allLookups.Cities);
            Setup(m => m.GetCollegeApplicationCycles()).ReturnsAsync(_allLookups.CollegeApplicationCycles);
            Setup(m => m.GetColleges(It.IsAny<string>())).ReturnsAsync(_allLookups.Colleges);
            Setup(m => m.GetCommunityInvolvements(It.IsAny<string>())).ReturnsAsync(_allLookups.CommunityInvolvements);
            Setup(m => m.GetCountries(It.IsAny<string>())).ReturnsAsync(_allLookups.Countries);
            Setup(m => m.GetCourseDeliveries(It.IsAny<string>())).ReturnsAsync(_allLookups.CourseDeliveries);
            Setup(m => m.GetCourseStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.CourseStatuses);
            Setup(m => m.GetCourseTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.CourseTypes);
            Setup(m => m.GetCredentialEvaluationAgencies()).ReturnsAsync(_allLookups.CredentialEvaluationAgencies);
            Setup(m => m.GetCredentials(It.IsAny<string>())).ReturnsAsync(_allLookups.Credentials);
            Setup(m => m.GetCurrencies()).ReturnsAsync(_allLookups.Currencies);
            Setup(m => m.GetCurrents(It.IsAny<string>())).ReturnsAsync(_allLookups.Currents);
            Setup(m => m.GetCurrentsDto(It.IsAny<string>())).ReturnsAsync(_modelFakerFixture.SeedDataFixture.Currents);
            Setup(m => m.GetDocumentPrints()).ReturnsAsync(_allLookups.DocumentPrints);
            Setup(m => m.GetEntryLevels(It.IsAny<string>())).ReturnsAsync(_allLookups.EntryLevels);
            Setup(m => m.GetFirstGenerationApplicants(It.IsAny<string>())).ReturnsAsync(_allLookups.FirstGenerationApplicants);
            Setup(m => m.GetFirstLanguages(It.IsAny<string>())).ReturnsAsync(_allLookups.FirstLanguages);
            Setup(m => m.GetGenders(It.IsAny<string>())).ReturnsAsync(_allLookups.Genders);
            Setup(m => m.GetGrades(It.IsAny<string>())).ReturnsAsync(_allLookups.Grades);
            Setup(m => m.GetGradeTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.GradeTypes);
            Setup(m => m.GetHighestEducations(It.IsAny<string>())).ReturnsAsync(_allLookups.HighestEducations);
            Setup(m => m.GetHighSchools(It.IsAny<string>())).ReturnsAsync(_allLookups.HighSchools);
            Setup(m => m.GetHighSkillsMajors(It.IsAny<string>())).ReturnsAsync(_allLookups.HighSkillsMajors);
            Setup(m => m.GetInternationalCreditAssessmentStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.InternationalCreditAssessmentStatuses);
            Setup(m => m.GetInstituteTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.InstituteTypes);
            Setup(m => m.GetInstitutes()).ReturnsAsync(_allLookups.Institutes);
            Setup(m => m.GetLiteracyTests(It.IsAny<string>())).ReturnsAsync(_allLookups.LiteracyTests);
            Setup(m => m.GetOfferStates(It.IsAny<string>())).ReturnsAsync(_allLookups.OfferStates);
            Setup(m => m.GetOfferStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.OfferStatuses);
            Setup(m => m.GetOfferTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.OfferTypes);
            Setup(m => m.GetOfficials(It.IsAny<string>())).ReturnsAsync(_allLookups.Officials);
            Setup(m => m.GetPaymentMethods(It.IsAny<string>())).ReturnsAsync(_allLookups.PaymentMethods);
            Setup(m => m.GetPaymentMethodsDto(It.IsAny<string>())).ReturnsAsync(_modelFakerFixture.SeedDataFixture.PaymentMethods);
            Setup(m => m.GetPaymentResults()).ReturnsAsync(_allLookups.PaymentResults);
            Setup(m => m.GetPreferredCorrespondenceMethods(It.IsAny<string>())).ReturnsAsync(_allLookups.PreferredCorrespondenceMethods);
            Setup(m => m.GetPreferredLanguages(It.IsAny<string>())).ReturnsAsync(_allLookups.PreferredLanguages);
            Setup(m => m.GetProgramIntakeAvailabilities(It.IsAny<string>())).ReturnsAsync(_allLookups.ProgramIntakeAvailabilities);
            Setup(m => m.GetProgramIntakeStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.ProgramIntakeStatuses);
            Setup(m => m.GetPromotions(It.IsAny<string>())).ReturnsAsync(_allLookups.Promotions);
            Setup(m => m.GetProvinceStates(It.IsAny<string>())).ReturnsAsync(_allLookups.ProvinceStates);
            Setup(m => m.GetReferralPartners()).ReturnsAsync(_allLookups.ReferralPartners);
            Setup(m => m.GetSources(It.IsAny<string>())).ReturnsAsync(_allLookups.Sources);
            Setup(m => m.GetSponsorAgencies(It.IsAny<string>())).ReturnsAsync(_allLookups.SponsorAgencies);
            Setup(m => m.GetStudyLevels(It.IsAny<string>())).ReturnsAsync(_allLookups.StudyLevels);
            Setup(m => m.GetStudyMethods(It.IsAny<string>())).ReturnsAsync(_allLookups.StudyMethods);
            Setup(m => m.GetStandardizedTestTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.StandardizedTestTypes);
            Setup(m => m.GetSupportingDocumentTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.SupportingDocumentTypes);
            Setup(m => m.GetTitles(It.IsAny<string>())).ReturnsAsync(_allLookups.Titles);
            Setup(m => m.GetTranscriptRequestStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.TranscriptRequestStatuses);
            Setup(m => m.GetTranscriptTransmissions(It.IsAny<string>())).ReturnsAsync(_allLookups.TranscriptTransmissions);
            Setup(m => m.GetUniversities()).ReturnsAsync(_allLookups.Universities);
            Setup(m => m.GetVisaStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.VisaStatuses);

            Setup(m => m.GetAllLookups(It.IsAny<string>(), It.IsAny<string[]>())).ReturnsAsync(_allLookups);
        }
    }
}
