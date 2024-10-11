using Moq;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;

namespace Ocas.Domestic.Apply.Admin.TestFramework
{
    public class LookupsCacheMock : Mock<ILookupsCache>
    {
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly AllLookups _allLookups;

        public LookupsCacheMock()
        {
            _modelFakerFixture = new ModelFakerFixture();
            _allLookups = _modelFakerFixture.AllAdminLookups;
            SetupLookupData();
        }

        private void SetupLookupData()
        {
            Setup(m => m.GetAboriginalStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.AboriginalStatuses);
            Setup(m => m.GetApplicationCycles()).ReturnsAsync(_allLookups.ApplicationCycles);
            Setup(m => m.GetApplicationCycleStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.ApplicationCycleStatuses);
            Setup(m => m.GetApplicationStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.ApplicationStatuses);
            Setup(m => m.GetCampuses()).ReturnsAsync(_allLookups.Campuses);
            Setup(m => m.GetCollegeApplicationCycles()).ReturnsAsync(_allLookups.CollegeApplicationCycles);
            Setup(m => m.GetColleges(It.IsAny<string>())).ReturnsAsync(_allLookups.Colleges);
            Setup(m => m.GetCommunityInvolvements(It.IsAny<string>())).ReturnsAsync(_allLookups.CommunityInvolvements);
            Setup(m => m.GetCourseDeliveries(It.IsAny<string>())).ReturnsAsync(_allLookups.CourseDeliveries);
            Setup(m => m.GetCourseStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.CourseStatuses);
            Setup(m => m.GetCourseTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.CourseTypes);
            Setup(m => m.GetEntryLevels(It.IsAny<string>())).ReturnsAsync(_allLookups.EntryLevels);
            Setup(m => m.GetCredentials(It.IsAny<string>())).ReturnsAsync(_allLookups.Credentials);
            Setup(m => m.GetGradeTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.GradeTypes);
            Setup(m => m.GetHighestEducations(It.IsAny<string>())).ReturnsAsync(_allLookups.HighestEducations);
            Setup(m => m.GetHighlyCompetitives(It.IsAny<string>())).ReturnsAsync(_allLookups.HighlyCompetitives);
            Setup(m => m.GetHighSchools(It.IsAny<string>())).ReturnsAsync(_allLookups.HighSchools);
            Setup(m => m.GetHighSkillsMajors(It.IsAny<string>())).ReturnsAsync(_allLookups.HighSkillsMajors);
            Setup(m => m.GetInstituteTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.InstituteTypes);
            Setup(m => m.GetInstitutes()).ReturnsAsync(_allLookups.Institutes);
            Setup(m => m.GetIntakeStatuses(It.IsAny<string>())).ReturnsAsync(_allLookups.IntakeStatuses);
            Setup(m => m.GetIntakeAvailabilities(It.IsAny<string>())).ReturnsAsync(_allLookups.IntakeAvailabilities);
            Setup(m => m.GetLiteracyTests(It.IsAny<string>())).ReturnsAsync(_allLookups.LiteracyTests);
            Setup(m => m.GetMcuCodes(It.IsAny<string>())).ReturnsAsync(_allLookups.McuCodes);
            Setup(m => m.GetMinistryApprovals(It.IsAny<string>())).ReturnsAsync(_allLookups.MinistryApprovals);
            Setup(m => m.GetOfferStates(It.IsAny<string>())).ReturnsAsync(_allLookups.OfferStates);
            Setup(m => m.GetOfficials(It.IsAny<string>())).ReturnsAsync(_allLookups.Officials);
            Setup(m => m.GetOstNotes(It.IsAny<string>())).ReturnsAsync(_allLookups.OstNotes);
            Setup(m => m.GetPaymentMethods(It.IsAny<string>())).ReturnsAsync(_allLookups.PaymentMethods);
            Setup(m => m.GetProgramCategories(It.IsAny<string>())).ReturnsAsync(_allLookups.ProgramCategories);
            Setup(m => m.GetProgramSubCategories(It.IsAny<string>())).ReturnsAsync(_allLookups.ProgramSubCategories);
            Setup(m => m.GetProgramDeliveries(It.IsAny<string>())).ReturnsAsync(_allLookups.ProgramDeliveries);
            Setup(m => m.GetProgramLanguages(It.IsAny<string>())).ReturnsAsync(_allLookups.ProgramLanguages);
            Setup(m => m.GetProgramLengthTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.ProgramLengths);
            Setup(m => m.GetProgramLevels(It.IsAny<string>())).ReturnsAsync(_allLookups.ProgramLevels);
            Setup(m => m.GetProgramTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.ProgramTypes);
            Setup(m => m.GetProvinceStates(It.IsAny<string>())).ReturnsAsync(_allLookups.ProvinceStates);
            Setup(m => m.GetSources(It.IsAny<string>())).ReturnsAsync(_allLookups.Sources);
            Setup(m => m.GetStandardizedTestTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.StandardizedTestTypes);
            Setup(m => m.GetStudyAreas(It.IsAny<string>())).ReturnsAsync(_allLookups.StudyAreas);
            Setup(m => m.GetStudyMethods(It.IsAny<string>())).ReturnsAsync(_allLookups.StudyMethods);
            Setup(m => m.GetSupportingDocumentTypes(It.IsAny<string>())).ReturnsAsync(_allLookups.SupportingDocumentTypes);
            Setup(m => m.GetTitles(It.IsAny<string>())).ReturnsAsync(_allLookups.Titles);
            Setup(m => m.GetTranscriptTransmissions(It.IsAny<string>())).ReturnsAsync(_allLookups.TranscriptTransmissions);
            Setup(m => m.GetTranscriptSources(It.IsAny<string>())).ReturnsAsync(_allLookups.TranscriptSources);
            Setup(m => m.GetUniversities()).ReturnsAsync(_allLookups.Universities);
        }
    }
}
