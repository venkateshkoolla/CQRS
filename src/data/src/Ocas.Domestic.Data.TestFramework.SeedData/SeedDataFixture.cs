using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.TestFramework
{
    public class SeedDataFixture
    {
        public SeedDataFixture()
        {
            AboriginalStatuses = Read<IList<AboriginalStatus>>("AboriginalStatuses.json");
            AccountStatuses = Read<IList<AccountStatus>>("AccountStatuses.json");
            AddressTypes = Read<IList<AddressType>>("AddressTypes.json");
            AdultTrainings = Read<IList<AdultTraining>>("AdultTrainings.json");
            ApplicationCycles = Read<IList<ApplicationCycle>>("ApplicationCycles.json");
            ApplicationCycleStatuses = Read<IList<ApplicationCycleStatus>>("ApplicationCycleStatuses.json");
            ApplicationStatuses = Read<IList<ApplicationStatus>>("ApplicationStatuses.json");
            BasisForAdmissions = Read<IList<BasisForAdmission>>("BasisForAdmissions.json");
            Campuses = Read<IList<Campus>>("Campuses.json");
            CanadianStatuses = Read<IList<CanadianStatus>>("CanadianStatuses.json");
            Cities = Read<IList<City>>("Cities.json");
            CollegeApplicationCycles = Read<IList<CollegeApplicationCycle>>("CollegeApplicationCycles.json");
            CollegeInformations = Read<IList<CollegeInformation>>("CollegeInformations.json");
            Colleges = Read<IList<College>>("Colleges.json");
            CommunityInvolvements = Read<IList<CommunityInvolvement>>("CommunityInvolvements.json");
            Countries = Read<IList<Country>>("Countries.json");
            CourseDeliveries = Read<IList<CourseDelivery>>("CourseDeliveries.json");
            CourseLanguages = Read<IList<CourseLanguage>>("CourseLanguages.json");
            CourseStatuses = Read<IList<CourseStatus>>("CourseStatuses.json");
            CourseTypes = Read<IList<CourseType>>("CourseTypes.json");
            CredentialEvaluationAgencies = Read<IList<CourseType>>("CredentialEvaluationAgencies.json");
            Credentials = Read<IList<Credential>>("Credentials.json");
            Currencies = Read<IList<Currency>>("Currencies.json");
            Currents = Read<IList<Current>>("Currents.json");
            DocumentPrints = Read<IList<DocumentPrint>>("DocumentPrints.json");
            EntryLevels = Read<IList<EntryLevel>>("EntryLevels.json");
            ExpiryActions = Read<IList<ExpiryAction>>("ExpiryActions.json");
            FirstGenerationApplicants = Read<IList<FirstGenerationApplicant>>("FirstGenerationApplicants.json");
            FirstLanguages = Read<IList<FirstLanguage>>("FirstLanguages.json");
            Genders = Read<IList<Gender>>("Genders.json");
            GradeTypes = Read<IList<GradeType>>("GradeTypes.json");
            HighestEducations = Read<IList<HighestEducation>>("HighestEducations.json");
            HighlyCompetitives = Read<IList<HighlyCompetitive>>("HighlyCompetitives.json");
            HighSchools = Read<IList<HighSchool>>("HighSchools.json");
            HighSkillsMajors = Read<IList<HighSkillsMajor>>("HighSkillsMajors.json");
            Institutes = Read<IList<Institute>>("Institutes.json");
            InstituteTypes = Read<IList<InstituteType>>("InstituteTypes.json");
            InternationalCreditAssessmentStatuses = Read<IList<InternationalCreditAssessmentStatus>>("InternationalCreditAssessmentStatuses.json");
            Languages = Read<IList<Language>>("Languages.json");
            LastGradeCompleteds = Read<IList<LastGradeCompleted>>("LastGradeCompleteds.json");
            LevelAchieveds = Read<IList<LevelAchieved>>("LevelAchieveds.json");
            LevelOfStudies = Read<IList<LevelOfStudy>>("LevelOfStudies.json");
            LiteracyTests = Read<IList<LiteracyTest>>("LiteracyTests.json");
            McuCodes = Read<IList<McuCode>>("McuCodes.json");
            MinistryApprovals = Read<IList<MinistryApproval>>("MinistryApprovals.json");
            OfferStates = Read<IList<OfferState>>("OfferStates.json");
            OfferStatuses = Read<IList<OfferStatus>>("OfferStatuses.json");
            OfferStudyMethods = Read<IList<OfferStudyMethod>>("OfferStudyMethods.json");
            OfferTypes = Read<IList<OfferType>>("OfferTypes.json");
            Officials = Read<IList<Official>>("Officials.json");
            OstNotes = Read<IList<OstNote>>("OstNotes.json");
            PaymentMethods = Read<IList<PaymentMethod>>("PaymentMethods.json");
            PaymentResults = Read<IList<PaymentResult>>("PaymentResults.json");
            PreferredCorrespondenceMethods = Read<IList<PreferredCorrespondenceMethod>>("PreferredCorrespondenceMethods.json");
            PreferredLanguages = Read<IList<PreferredLanguage>>("PreferredLanguages.json");
            PreferredSponsorAgencies = Read<IList<PreferredSponsorAgency>>("PreferredSponsorAgencies.json");
            PrivacyStatements = Read<IList<PrivacyStatement>>("PrivacyStatements.json");
            Products = Read<IList<Product>>("Products.json");
            ProgramCategories = Read<IList<ProgramCategory>>("ProgramCategories.json");
            ProgramIntakeAvailabilities = Read<IList<ProgramIntakeAvailability>>("ProgramIntakeAvailabilities.json");
            ProgramIntakeStatuses = Read<IList<ProgramIntakeStatus>>("ProgramIntakeStatuses.json");
            ProgramLanguages = Read<IList<ProgramLanguage>>("ProgramLanguages.json");
            ProgramLevels = Read<IList<ProgramLevel>>("ProgramLevels.json");
            ProgramSpecialCodes = Read<IList<ProgramSpecialCode>>("ProgramSpecialCodes.json");
            ProgramSubCategories = Read<IList<ProgramSubCategory>>("ProgramSubCategories.json");
            ProgramTypes = Read<IList<ProgramType>>("ProgramTypes.json");
            Promotions = Read<IList<Promotion>>("Promotions.json");
            ProvinceStates = Read<IList<ProvinceState>>("ProvinceStates.json");
            ReferralPartners = Read<IList<ReferralPartner>>("ReferralPartners.json");
            SchoolStatuses = Read<IList<SchoolStatus>>("SchoolStatuses.json");
            SchoolTypes = Read<IList<SchoolType>>("SchoolTypes.json");
            ShsmCompletions = Read<IList<ShsmCompletion>>("ShsmCompletions.json");
            Sources = Read<IList<Source>>("Sources.json");
            StatusOfVisas = Read<IList<StatusOfVisa>>("StatusOfVisas.json");
            StudyAreas = Read<IList<StudyArea>>("StudyAreas.json");
            SupportingDocumentTypes = Read<IList<SupportingDocumentType>>("SupportingDocumentTypes.json");
            TestTypes = Read<IList<TestType>>("TestTypes.json");
            Titles = Read<IList<Title>>("Titles.json");
            TranscriptRequestExceptions = Read<IList<TranscriptRequestException>>("TranscriptRequestExceptions.json");
            TranscriptRequestStatuses = Read<IList<TranscriptRequestStatus>>("TranscriptRequestStatuses.json");
            TranscriptSources = Read<IList<TranscriptSource>>("TranscriptSources.json");
            TranscriptTransmissions = Read<IList<TranscriptTransmission>>("TranscriptTransmissionDates.json");
            UnitOfMeasures = Read<IList<UnitOfMeasure>>("UnitOfMeasures.json");
            Universities = Read<IList<University>>("Universities.json");
        }

        public IList<AboriginalStatus> AboriginalStatuses { get; }
        public IList<AccountStatus> AccountStatuses { get; }
        public IList<AddressType> AddressTypes { get;  }
        public IList<AdultTraining> AdultTrainings { get; }
        public IList<ApplicationCycle> ApplicationCycles { get; }
        public IList<ApplicationCycleStatus> ApplicationCycleStatuses { get; }
        public IList<ApplicationStatus> ApplicationStatuses { get; }
        public IList<BasisForAdmission> BasisForAdmissions { get; }
        public IList<Campus> Campuses { get; }
        public IList<CanadianStatus> CanadianStatuses { get; }
        public IList<City> Cities { get;  }
        public IList<College> Colleges { get; }
        public IList<CollegeApplicationCycle> CollegeApplicationCycles { get; }
        public IList<CollegeInformation> CollegeInformations { get;  }
        public IList<CommunityInvolvement> CommunityInvolvements { get;  }
        public IList<Country> Countries { get;  }
        public IList<CourseDelivery> CourseDeliveries { get;  }
        public IList<CourseLanguage> CourseLanguages { get;  }
        public IList<CourseStatus> CourseStatuses { get;  }
        public IList<CourseType> CourseTypes { get;  }
        public IList<CourseType> CredentialEvaluationAgencies { get;  }
        public IList<Credential> Credentials { get; }
        public IList<Currency> Currencies { get;  }
        public IList<Current> Currents { get;  }
        public IList<DocumentPrint> DocumentPrints { get; }
        public IList<EntryLevel> EntryLevels { get; }
        public IList<ExpiryAction> ExpiryActions { get;  }
        public IList<FirstGenerationApplicant> FirstGenerationApplicants { get;  }
        public IList<FirstLanguage> FirstLanguages { get;  }
        public IList<Gender> Genders { get;  }
        public IList<GradeType> GradeTypes { get;  }
        public IList<HighestEducation> HighestEducations { get;  }
        public IList<HighlyCompetitive> HighlyCompetitives { get; }
        public IList<HighSchool> HighSchools { get;  }
        public IList<HighSkillsMajor> HighSkillsMajors { get;  }
        public IList<Institute> Institutes { get;  }
        public IList<InstituteType> InstituteTypes { get;  }
        public IList<InternationalCreditAssessmentStatus> InternationalCreditAssessmentStatuses { get;  }
        public IList<Language> Languages { get;  }
        public IList<LastGradeCompleted> LastGradeCompleteds { get;  }
        public IList<LevelAchieved> LevelAchieveds { get;  }
        public IList<LevelOfStudy> LevelOfStudies { get;  }
        public IList<LiteracyTest> LiteracyTests { get;  }
        public IList<McuCode> McuCodes { get; }
        public IList<MinistryApproval> MinistryApprovals { get; }
        public IList<OfferState> OfferStates { get;  }
        public IList<OfferStatus> OfferStatuses { get;  }
        public IList<OfferStudyMethod> OfferStudyMethods { get; }
        public IList<OfferType> OfferTypes { get;  }
        public IList<Official> Officials { get;  }
        public IList<OstNote> OstNotes { get;  }
        public IList<PaymentMethod> PaymentMethods { get;  }
        public IList<PaymentResult> PaymentResults { get;  }
        public IList<PreferredCorrespondenceMethod> PreferredCorrespondenceMethods { get;  }
        public IList<PreferredLanguage> PreferredLanguages { get;  }
        public IList<PreferredSponsorAgency> PreferredSponsorAgencies { get;  }
        public IList<PrivacyStatement> PrivacyStatements { get;  }
        public IList<Product> Products { get;  }
        public IList<ProgramCategory> ProgramCategories { get; }
        public IList<ProgramIntakeAvailability> ProgramIntakeAvailabilities { get;  }
        public IList<ProgramIntakeStatus> ProgramIntakeStatuses { get;  }
        public IList<ProgramLanguage> ProgramLanguages { get; }
        public IList<ProgramLevel> ProgramLevels { get; }
        public IList<ProgramSpecialCode> ProgramSpecialCodes { get; }
        public IList<ProgramSubCategory> ProgramSubCategories { get; }
        public IList<ProgramType> ProgramTypes { get; }
        public IList<Promotion> Promotions { get; }
        public IList<ProvinceState> ProvinceStates { get;  }
        public IList<ReferralPartner> ReferralPartners { get; }
        public IList<SchoolStatus> SchoolStatuses { get;  }
        public IList<SchoolType> SchoolTypes { get;  }
        public IList<ShsmCompletion> ShsmCompletions { get;  }
        public IList<Source> Sources { get;  }
        public IList<StatusOfVisa> StatusOfVisas { get;  }
        public IList<StudyArea> StudyAreas { get; }
        public IList<SupportingDocumentType> SupportingDocumentTypes { get; }
        public IList<TestType> TestTypes { get;  }
        public IList<Title> Titles { get;  }
        public IList<TranscriptRequestException> TranscriptRequestExceptions { get;  }
        public IList<TranscriptRequestStatus> TranscriptRequestStatuses { get;  }
        public IList<TranscriptSource> TranscriptSources { get; }
        public IList<TranscriptTransmission> TranscriptTransmissions { get;  }
        public IList<UnitOfMeasure> UnitOfMeasures { get; }
        public IList<University> Universities { get; }

        private static T Read<T>(string name)
            where T : class
        {
            var json = Read(name);
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings { DateTimeZoneHandling = DateTimeZoneHandling.Utc });
        }

        private static string Read(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Ocas.Domestic.Data.TestFramework.Resources.{name}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream is null) throw new FileNotFoundException($"Seed data file is missing: {name}");

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
