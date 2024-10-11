using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using AllLookups = Ocas.Domestic.Apply.Admin.Models.AllLookups;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin
{
    public interface ILookupsCache : ILookupsCacheBase
    {
        Task<IList<AboriginalStatus>> GetAboriginalStatuses(string locale);
        Task<IList<LookupItem>> GetAccountStatuses(string locale);
        Task<IList<LookupItem>> GetAchievementLevels(string locale);
        Task<IList<LookupItem>> GetAdultTrainings(string locale);
        Task<AllLookups> GetAllLookups(string locale, string[] keys);
        Task<IList<Campus>> GetCampuses();
        Task<IList<LookupItem>> GetCanadianStatuses(string locale);
        Task<IList<CollegeInformation>> GetCollegeInformation(string locale);
        Task<IList<LookupItem>> GetCredentialEvaluationAgencies();
        Task<IList<LookupItem>> GetCredentials(string locale);
        Task<IList<DocumentPrint>> GetDocumentPrints();
        Task<IList<LookupItem>> GetFirstGenerationApplicants(string locale);
        Task<IList<LookupItem>> GetFirstLanguages(string locale);
        Task<IList<LookupItem>> GetGenders(string locale);
        Task<IList<LookupItem>> GetGrades(string locale);
        Task<IList<LookupItem>> GetHighlyCompetitives(string locale);
        Task<IList<LookupItem>> GetInstitutes();
        Task<IList<LookupItem>> GetInstituteTypes(string locale);
        Task<IList<InstituteWarning>> GetInstituteWarnings(string locale);
        Task<IList<LookupItem>> GetIntakeAvailabilities(string locale);
        Task<IList<LookupItem>> GetIntakeExpiryActions(string locale);
        Task<IList<LookupItem>> GetIntakeStatuses(string locale);
        Task<IList<LookupItem>> GetInternationalCreditAssessmentStatuses(string locale);
        Task<PrivacyStatement> GetLatestPrivacyStatement(string locale);
        Task<IList<LookupItem>> GetLanguages(string locale);
        Task<IList<McuCode>> GetMcuCodes(string locale);
        Task<IList<LookupItem>> GetMinistryApprovals(string locale);
        Task<IList<LookupItem>> GetOfferStates(string locale);
        Task<IList<LookupItem>> GetOfferTypes(string locale);
        Task<IList<LookupItem>> GetOfficials(string locale);
        Task<IList<LookupItem>> GetOstNotes(string locale);
        Task<IList<LookupItem>> GetPreferredCorrespondenceMethods(string locale);
        Task<IList<LookupItem>> GetPreferredLanguages(string locale);
        Task<IList<LookupItem>> GetProgramCategories(string locale);
        Task<IList<LookupItem>> GetProgramCredentials(string locale);
        Task<IList<LookupItem>> GetProgramDeliveries(string locale);
        Task<IList<LookupItem>> GetProgramLanguages(string locale);
        Task<IList<LookupItem>> GetProgramLengths(string locale);
        Task<IList<LookupItem>> GetProgramLengthTypes(string locale);
        Task<IList<LookupItem>> GetProgramLevels(string locale);
        Task<IList<LookupItem>> GetProgramPromotions(string locale);
        Task<IList<SubCategory>> GetProgramSubCategories(string locale);
        Task<IList<LookupItem>> GetProgramTypes(string locale);
        Task<IList<LookupItem>> GetShsmCompletions();
        Task<IList<LookupItem>> GetSponsorAgencies(string locale);
        Task<IList<LookupItem>> GetStudyAreas(string locale);
        Task<IList<LookupItem>> GetStudyLevels(string locale);
        Task<IList<LookupItem>> GetStudyMethods(string locale);
        Task<IList<LookupItem>> GetSupportingDocumentTypes(string locale);
        Task<IList<LookupItem>> GetTitles(string locale);
        Task<IList<LookupItem>> GetTranscriptSources(string locale);
        Task<IList<TranscriptTransmission>> GetTranscriptTransmissions(string locale);
        Task<IList<LookupItem>> GetVisaStatuses(string locale);
        void PurgeAllLookups(string[] keys);
    }
}
