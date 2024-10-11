using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Core
{
    public interface ILookupsCache : ILookupsCacheBase
    {
        Task<IList<AboriginalStatus>> GetAboriginalStatuses(string locale);
        Task<IList<LookupItem>> GetAccountStatuses(string locale);
        Task<IList<LookupItem>> GetAchievementLevels(string locale);
        Task<IList<Campus>> GetCampuses();
        Task<IList<LookupItem>> GetCanadianStatuses(string locale);
        Task<IList<CollegeInformation>> GetCollegeInformation(string locale);
        Task<IList<LookupItem>> GetCredentials(string locale);
        Task<IList<LookupItem>> GetCredentialEvaluationAgencies();
        Task<IList<DocumentPrint>> GetDocumentPrints();
        Task<IList<LookupItem>> GetFirstGenerationApplicants(string locale);
        Task<IList<LookupItem>> GetFirstLanguages(string locale);
        Task<IList<LookupItem>> GetGenders(string locale);
        Task<IList<LookupItem>> GetGrades(string locale);
        Task<IList<LookupItem>> GetInstitutes();
        Task<IList<LookupItem>> GetInstituteTypes(string locale);
        Task<IList<InstituteWarning>> GetInstituteWarnings(string locale);
        Task<IList<LookupItem>> GetInternationalCreditAssessmentStatuses(string locale);
        Task<PrivacyStatement> GetLatestPrivacyStatement(string locale);
        Task<IList<LookupItem>> GetOfferStates(string locale);
        Task<IList<LookupItem>> GetOfferTypes(string locale);
        Task<IList<LookupItem>> GetOfficials(string locale);
        Task<IList<LookupItem>> GetOstNotes(string locale);
        Task<IList<LookupItem>> GetPreferredCorrespondenceMethods(string locale);
        Task<IList<LookupItem>> GetPreferredLanguages(string locale);
        Task<IList<LookupItem>> GetSponsorAgencies(string locale);
        Task<IList<LookupItem>> GetStudyLevels(string locale);
        Task<IList<LookupItem>> GetStudyMethods(string locale);
        Task<IList<LookupItem>> GetSupportingDocumentTypes(string locale);
        Task<IList<LookupItem>> GetTitles(string locale);
        Task<IList<TranscriptTransmission>> GetTranscriptTransmissions(string locale);
        Task<IList<LookupItem>> GetVisaStatuses(string locale);

        Task<AllLookups> GetAllLookups(string locale, string[] keys);
        void PurgeAllLookups(string[] keys);
    }
}
