using System.Collections.Generic;
using Newtonsoft.Json;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;

namespace Ocas.Domestic.Apply.Admin.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AllLookups
    {
        public IList<AboriginalStatus> AboriginalStatuses { get; set; }
        public IList<Country> Countries { get; set; }
        public IList<InstituteWarning> InstituteWarnings { get; set; }
        public PrivacyStatement LatestPrivacyStatement { get; set; }
        public IList<SubCategory> ProgramSubCategories { get; set; }
        public IList<ProvinceState> ProvinceStates { get; set; }
        public IList<TranscriptTransmission> TranscriptTransmissions { get; set; }

        // Lookup items
        public IList<LookupItem> AccountStatuses { get; set; }
        public IList<LookupItem> AchievementLevels { get; set; }
        public IList<LookupItem> AdultTrainings { get; set; }
        public IList<LookupItem> ApplicationCycleStatuses { get; set; }
        public IList<LookupItem> ApplicationStatuses { get; set; }
        public IList<LookupItem> CanadianStatuses { get; set; }
        public IList<LookupItem> CommunityInvolvements { get; set; }
        public IList<LookupItem> CourseDeliveries { get; set; }
        public IList<LookupItem> CourseStatuses { get; set; }
        public IList<LookupItem> CourseTypes { get; set; }
        public IList<LookupItem> CredentialEvaluationAgencies { get; set; }
        public IList<LookupItem> Credentials { get; set; }
        public IList<LookupItem> Currencies { get; set; }
        public IList<LookupItem> EntryLevels { get; set; }
        public IList<LookupItem> FirstGenerationApplicants { get; set; }
        public IList<LookupItem> FirstLanguages { get; set; }
        public IList<LookupItem> Genders { get; set; }
        public IList<LookupItem> Grades { get; set; }
        public IList<LookupItem> GradeTypes { get; set; }
        public IList<LookupItem> HighestEducations { get; set; }
        public IList<LookupItem> HighlyCompetitives { get; set; }
        public IList<LookupItem> HighSkillsMajors { get; set; }
        public IList<LookupItem> InstituteTypes { get; set; }
        public IList<LookupItem> IntakeAvailabilities { get; set; }
        public IList<LookupItem> IntakeExpiryActions { get; set; }
        public IList<LookupItem> IntakeStatuses { get; set; }
        public IList<LookupItem> Languages { get; set; }
        public IList<LookupItem> LiteracyTests { get; set; }
        public IList<LookupItem> MinistryApprovals { get; set; }
        public IList<LookupItem> OfferStates { get; set; }
        public IList<LookupItem> OfferStatuses { get; set; }
        public IList<LookupItem> OfferTypes { get; set; }
        public IList<LookupItem> Officials { get; set; }
        public IList<LookupItem> OstNotes { get; set; }
        public IList<LookupItem> PaymentMethods { get; set; }
        public IList<LookupItem> PaymentResults { get; set; }
        public IList<LookupItem> PreferredCorrespondenceMethods { get; set; }
        public IList<LookupItem> PreferredLanguages { get; set; }
        public IList<LookupItem> ProgramCategories { get; set; }
        public IList<LookupItem> ProgramCredentials { get; set; }
        public IList<LookupItem> ProgramDeliveries { get; set; }
        public IList<LookupItem> ProgramLanguages { get; set; }
        public IList<LookupItem> ProgramLengths { get; set; }
        public IList<LookupItem> ProgramLengthTypes { get; set; }
        public IList<LookupItem> ProgramLevels { get; set; }
        public IList<LookupItem> ProgramPromotions { get; set; }
        public IList<LookupItem> ProgramTypes { get; set; }
        public IList<LookupItem> Promotions { get; set; }
        public IList<LookupItem> Sources { get; set; }
        public IList<LookupItem> SponsorAgencies { get; set; }
        public IList<LookupItem> StandardizedTestTypes { get; set; }
        public IList<LookupItem> StudyAreas { get; set; }
        public IList<LookupItem> StudyLevels { get; set; }
        public IList<LookupItem> StudyMethods { get; set; }
        public IList<LookupItem> SupportingDocumentTypes { get; set; }
        public IList<LookupItem> Titles { get; set; }
        public IList<LookupItem> TranscriptRequestStatuses { get; set; }
        public IList<LookupItem> VisaStatuses { get; set; }


        [JsonIgnore]
        public IList<ApplicationCycle> ApplicationCycles { get; set; }

        [JsonIgnore]
        public IList<Campus> Campuses { get; set; }

        [JsonIgnore]
        public IList<CollegeApplicationCycle> CollegeApplicationCycles { get; set; }

        [JsonIgnore]
        public IList<College> Colleges { get; set; }

        [JsonIgnore]
        public IList<HighSchool> HighSchools { get; set; }

        [JsonIgnore]
        public IList<LookupItem> Institutes { get; set; }

        [JsonIgnore]
        public IList<McuCode> McuCodes { get; set; }

        [JsonIgnore]
        public IList<LookupItem> TranscriptSources { get; set; }

        [JsonIgnore]
        public IList<University> Universities { get; set; }
    }
}
