using System.Collections.Generic;
using Newtonsoft.Json;
using Ocas.Domestic.Apply.Models.Lookups;

namespace Ocas.Domestic.Apply.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AllLookups
    {
        public IList<AboriginalStatus> AboriginalStatuses { get; set; }
        public IList<LookupItem> AccountStatuses { get; set; }
        public IList<LookupItem> AchievementLevels { get; set; }
        public IList<LookupItem> ApplicationStatuses { get; set; }
        public IList<LookupItem> CanadianStatuses { get; set; }
        public IList<LookupItem> CredentialEvaluationAgencies { get; set; }
        public IList<LookupItem> Credentials { get; set; }
        public IList<Country> Countries { get; set; }
        public IList<LookupItem> Currencies { get; set; }
        public IList<LookupItem> EntryLevels { get; set; }
        public IList<LookupItem> FirstGenerationApplicants { get; set; }
        public IList<LookupItem> FirstLanguages { get; set; }
        public IList<LookupItem> Genders { get; set; }
        public IList<LookupItem> Grades { get; set; }
        public IList<LookupItem> Institutes { get; set; }
        public IList<LookupItem> InstituteTypes { get; set; }
        public IList<InstituteWarning> InstituteWarnings { get; set; }
        public PrivacyStatement LatestPrivacyStatement { get; set; }
        public IList<LookupItem> OfferStates { get; set; }
        public IList<LookupItem> OfferStatuses { get; set; }
        public IList<LookupItem> OfferTypes { get; set; }
        public IList<LookupItem> Officials { get; set; }
        public IList<LookupItem> PaymentMethods { get; set; }
        public IList<LookupItem> PaymentResults { get; set; }
        public IList<LookupItem> PreferredCorrespondenceMethods { get; set; }
        public IList<LookupItem> PreferredLanguages { get; set; }
        public IList<ProvinceState> ProvinceStates { get; set; }
        public IList<LookupItem> Sources { get; set; }
        public IList<LookupItem> SponsorAgencies { get; set; }
        public IList<LookupItem> StudyLevels { get; set; }
        public IList<LookupItem> StudyMethods { get; set; }
        public IList<LookupItem> StandardizedTestTypes { get; set; }
        public IList<LookupItem> SupportingDocumentTypes { get; set; }
        public IList<LookupItem> Titles { get; set; }
        public IList<LookupItem> TranscriptRequestStatuses { get; set; }
        public IList<TranscriptTransmission> TranscriptTransmissions { get; set; }
        public IList<LookupItem> VisaStatuses { get; set; }

        [JsonIgnore]
        public IList<ApplicationCycle> ApplicationCycles { get; set; }

        [JsonIgnore]
        public IList<LookupItem> ApplicationCycleStatuses { get; set; }

        [JsonIgnore]
        public IList<LookupItem> BasisForAdmissions { get; set; }

        [JsonIgnore]
        public IList<Campus> Campuses { get; set; }

        [JsonIgnore]
        public IList<City> Cities { get; set; }

        [JsonIgnore]
        public IList<CollegeApplicationCycle> CollegeApplicationCycles { get; set; }

        [JsonIgnore]
        public IList<College> Colleges { get; set; }

        [JsonIgnore]
        public IList<LookupItem> CommunityInvolvements { get; set; }

        [JsonIgnore]
        public IList<LookupItem> CourseDeliveries { get; set; }

        [JsonIgnore]
        public IList<LookupItem> CourseStatuses { get; set; }

        [JsonIgnore]
        public IList<LookupItem> CourseTypes { get; set; }

        [JsonIgnore]
        public IList<LookupItem> Currents { get; set; }

        [JsonIgnore]
        public IList<DocumentPrint> DocumentPrints { get; set; }

        [JsonIgnore]
        public IList<LookupItem> GradeTypes { get; set; }

        [JsonIgnore]
        public IList<LookupItem> HighestEducations { get; set; }

        [JsonIgnore]
        public IList<HighSchool> HighSchools { get; set; }

        [JsonIgnore]
        public IList<LookupItem> HighSkillsMajors { get; set; }

        [JsonIgnore]
        public IList<LookupItem> InternationalCreditAssessmentStatuses { get; set; }

        [JsonIgnore]
        public IList<LookupItem> LiteracyTests { get; set; }

        [JsonIgnore]
        public IList<LookupItem> ProgramIntakeAvailabilities { get; set; }

        [JsonIgnore]
        public IList<LookupItem> ProgramIntakeStatuses { get; set; }

        [JsonIgnore]
        public IList<LookupItem> Promotions { get; set; }

        [JsonIgnore]
        public IList<ReferralPartner> ReferralPartners { get; set; }

        [JsonIgnore]
        public IList<University> Universities { get; set; }
    }
}
