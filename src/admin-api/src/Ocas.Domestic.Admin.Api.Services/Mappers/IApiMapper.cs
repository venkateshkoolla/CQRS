using System;
using System.Collections.Generic;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.AppSettings.Extras;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public interface IApiMapper : IApiMapperBase
    {
        IList<AboriginalStatus> MapAboriginalStatus(IList<Dto.AboriginalStatus> list);
        AcademicRecord MapAcademicRecord(Dto.AcademicRecord dbDto, IList<HighSchool> highSchools);
        IList<ApplicantBrief> MapApplicantBriefs(IList<Dto.ApplicantBrief> list);
        IList<ApplicantHistory> MapApplicantHistories(IList<Dto.CustomAudit> customAudits);
        ApplicantUpdateInfo MapApplicantUpdateInfo(Dto.Contact dbDto);
        Application MapApplication(Dto.Application model);
        ApplicationSummary MapApplicationSummary(Dto.ApplicationSummary applicationSummary, Dto.Contact applicant, IList<Dto.ProgramIntake> programIntakes, IList<LookupItem> applicationStatuses, IList<LookupItem> instituteTypes, IList<TranscriptTransmission> transcriptTransmissions, IList<LookupItem> offerStates, IList<LookupItem> programIntakeAvailabilities, IAppSettingsExtras appSettingsExtras);
        IList<ApplicationCycle> MapApplicationCycles(IList<Dto.ApplicationCycle> list, IList<LookupItem> applicationCycleStatuses, IAppSettingsExtras appSettingsExtras);
        IList<Campus> MapCampuses(IList<Dto.Campus> list);
        IList<City> MapCity(IList<Dto.City> list);
        IList<CollegeInformation> MapCollegeInformations(IList<Dto.CollegeInformation> list);
        IList<Country> MapCountry(IList<Dto.Country> list);
        IList<CollegeApplicationCycle> MapCollegeApplicationCycles(IList<Dto.CollegeApplicationCycle> list, IList<ApplicationCycle> applicationCycles, IList<LookupItem> applicationCycleStatuses);
        IList<College> MapColleges(IList<Dto.College> list, IList<Dto.SchoolStatus> schoolStatuses);
        IList<CollegeTransmissionHistory> MapCollegeTransmissionHistories(IList<Dto.CollegeTransmission> filteredDtoCollegeTransmissions, IList<College> colleges, IList<Dto.CollegeTransmission> allDtoCollegeTransmissions, TranslationsDictionary translationsDictionary);
        IList<DocumentPrint> MapDocumentPrints(IList<Dto.DocumentPrint> list);
        IList<HighSchool> MapHighSchools(IList<Dto.HighSchool> list);
        IList<InstituteWarning> MapInstituteWarnings(IList<Dto.TranscriptRequestException> list, IList<Guid> educationExceptions);
        IList<IntakeApplicant> MapIntakeApplicants(IList<Dto.ProgramApplication> list);
        IList<McuCode> MapMcuCodes(IList<Dto.McuCode> list);
        IList<OfferHistory> MapOfferHistories(IList<Dto.OfferAcceptance> offerAcceptances);
        OntarioHighSchoolCourseCode MapOntarioHighSchoolCourseCode(Dto.OntarioHighSchoolCourseCode dbDto);
        OntarioStudentCourseCredit MapOntarioStudentCourseCredit(Dto.OntarioStudentCourseCredit dbDto);
        IList<OntarioStudentCourseCredit> MapOntarioStudentCourseCredits(IList<Dto.OntarioStudentCourseCredit> list);
        PrivacyStatement MapPrivacyStatement(Dto.PrivacyStatement dbDto);
        Program MapProgram(Dto.Program dbDto, IList<McuCode> mcuCodes, IList<Dto.ProgramSpecialCode> specialCodes);
        Program MapProgram(Dto.Program dbDto, IList<McuCode> mcuCodes, IList<Dto.ProgramSpecialCode> specialCodes, IList<Dto.ProgramIntake> dtoProgramIntakes, IList<Dto.ProgramApplication> dtoProgramApplications);
        IList<ProgramBrief> MapProgramBriefs(IList<Dto.Program> list, IList<LookupItem> programDeliveries, IList<College> colleges, IList<Campus> campuses);
        IList<ProgramChoice> MapProgramChoices(IList<Dto.ProgramChoice> dbDtos, IList<Dto.ProgramIntake> intakes, Dto.ShoppingCart shoppingCart);
        ProgramIntake MapProgramIntake(Dto.ProgramIntake dbDto, bool canDelete);
        IList<IntakeExport> MapProgramIntakeExports(IList<Dto.ProgramIntake> dtoList, IList<College> colleges, IList<Campus> campuses, CollegeApplicationCycle collegeApplicationCycle, IList<LookupItem> programDeliveries, IList<LookupItem> programIntakeAvailabilities, IList<LookupItem> programIntakeStatuses, IList<LookupItem> intakeExpiryActions, IList<LookupItem> entryLevels);
        IList<IntakeBrief> MapProgramIntakeBriefs(IList<Dto.ProgramIntake> dbDtos, IList<LookupItem> studyMethods, IList<College> colleges, IList<Campus> campuses, IList<LookupItem> intakeStatuses, IList<LookupItem> intakeAvailabilities, string props);
        IList<ProvinceState> MapProvinceState(IList<Dto.ProvinceState> list);
        SpecialCode MapSpecialCode(Dto.ProgramSpecialCode dbDto);
        IList<SpecialCode> MapSpecialCodes(IList<Dto.ProgramSpecialCode> list);
        IList<SubCategory> MapSubCategory(IList<Dto.ProgramSubCategory> list);
        SupportingDocument MapSupportingDocument(Dto.AcademicRecord academicRecord);
        IList<SupportingDocument> MapSupportingDocuments(IList<Dto.SupportingDocument> list, IList<LookupItem> supportingDocumentTypes, IList<LookupItem> officials, IList<LookupItem> institutes);
        IList<SupportingDocument> MapSupportingDocuments(IList<Dto.Transcript> transcripts, IList<College> colleges, IList<University> universities);
        IList<SupportingDocument> MapSupportingDocuments(IList<Dto.Test> tests, IList<LookupItem> testTypes);
        IList<ReferralPartner> MapReferralPartners(IList<Dto.ReferralPartner> list);
        TranscriptRequest MapTranscriptRequest(Dto.TranscriptRequest model, IList<LookupItem> instituteTypes, IList<TranscriptTransmission> transmissions);
        IList<TranscriptTransmission> MapTranscriptTransmissions(IList<Dto.TranscriptTransmission> list, IList<Dto.InstituteType> instituteTypes);
        IList<University> MapUniversity(IList<Dto.University> list);

        // lookups
        IList<LookupItem> MapLookupItem(IList<Dto.AccountStatus> list);
        IList<LookupItem> MapLookupItem(IList<Dto.AdultTraining> list);
        IList<LookupItem> MapLookupItem(IList<Dto.ApplicationCycleStatus> list);
        IList<LookupItem> MapLookupItem(IList<Dto.ApplicationStatus> list);
        IList<LookupItem> MapLookupItem(IList<Dto.BasisForAdmission> list);
        IList<LookupItem> MapLookupItem(IList<Dto.CanadianStatus> list);
        IList<LookupItem> MapLookupItem(IList<Dto.CommunityInvolvement> list);
        IList<LookupItem> MapLookupItem(IList<Dto.CourseDelivery> list);
        IList<LookupItem> MapLookupItem(IList<Dto.CourseStatus> list);
        IList<LookupItem> MapLookupItem(IList<Dto.CourseType> list);
        IList<LookupItem> MapLookupItem(IList<Dto.Credential> list);
        IList<LookupItem> MapLookupItem(IList<Dto.CredentialEvaluationAgency> list);
        IList<LookupItem> MapLookupItem(IList<Dto.Currency> list);
        IList<LookupItem> MapLookupItem(IList<Dto.Current> list);
        IList<LookupItem> MapLookupItem(IList<Dto.EntryLevel> list);
        IList<LookupItem> MapLookupItem(IList<Dto.ExpiryAction> list);
        IList<LookupItem> MapLookupItem(IList<Dto.FirstGenerationApplicant> list);
        IList<LookupItem> MapLookupItem(IList<Dto.FirstLanguage> list);
        IList<LookupItem> MapLookupItem(IList<Dto.Gender> list);
        IList<LookupItem> MapLookupItem(IList<Dto.GradeType> list);
        IList<LookupItem> MapLookupItem(IList<Dto.HighestEducation> list);
        IList<LookupItem> MapLookupItem(IList<Dto.HighlyCompetitive> list);
        IList<LookupItem> MapLookupItem(IList<Dto.HighSkillsMajor> list);
        IList<LookupItem> MapLookupItem(IList<Dto.Institute> list);
        IList<LookupItem> MapLookupItem(IList<Dto.InstituteType> list);
        IList<LookupItem> MapLookupItem(IList<Dto.InternationalCreditAssessmentStatus> list);
        IList<LookupItem> MapLookupItem(IList<Dto.LastGradeCompleted> list);
        IList<LookupItem> MapLookupItem(IList<Dto.LevelAchieved> list);
        IList<LookupItem> MapLookupItem(IList<Dto.LevelOfStudy> list);
        IList<LookupItem> MapLookupItem(IList<Dto.LiteracyTest> list);
        IList<LookupItem> MapLookupItem(IList<Dto.MinistryApproval> list);
        IList<LookupItem> MapLookupItem(IList<Dto.OfferState> list);
        IList<LookupItem> MapLookupItem(IList<Dto.OfferStatus> list);
        IList<LookupItem> MapLookupItem(IList<Dto.OfferStudyMethod> list);
        IList<LookupItem> MapLookupItem(IList<Dto.OfferType> list);
        IList<LookupItem> MapLookupItem(IList<Dto.Official> list);
        IList<LookupItem> MapLookupItem(IList<Dto.OstNote> list);
        IList<LookupItem> MapLookupItem(IList<Dto.PaymentMethod> list);
        IList<LookupItem> MapLookupItem(IList<Dto.PaymentResult> list);
        IList<LookupItem> MapLookupItem(IList<Dto.PreferredCorrespondenceMethod> list);
        IList<LookupItem> MapLookupItem(IList<Dto.PreferredLanguage> list);
        IList<LookupItem> MapLookupItem(IList<Dto.PreferredSponsorAgency> list);
        IList<LookupItem> MapLookupItem(IList<Dto.ProgramCategory> list);
        IList<LookupItem> MapLookupItem(IList<Dto.ProgramIntakeAvailability> list);
        IList<LookupItem> MapLookupItem(IList<Dto.ProgramIntakeStatus> list);
        IList<LookupItem> MapLookupItem(IList<Dto.ProgramLanguage> list);
        IList<LookupItem> MapLookupItem(IList<Dto.ProgramLevel> list);
        IList<LookupItem> MapLookupItem(IList<Dto.ProgramType> list);
        IList<LookupItem> MapLookupItem(IList<Dto.Promotion> list);
        IList<LookupItem> MapLookupItem(IList<Dto.ShsmCompletion> list);
        IList<LookupItem> MapLookupItem(IList<Dto.Source> list);
        IList<LookupItem> MapLookupItem(IList<Dto.StatusOfVisa> list);
        IList<LookupItem> MapLookupItem(IList<Dto.StudyArea> list);
        IList<LookupItem> MapLookupItem(IList<Dto.SupportingDocumentType> list);
        IList<LookupItem> MapLookupItem(IList<Dto.TestType> list);
        IList<LookupItem> MapLookupItem(IList<Dto.Title> list);
        IList<LookupItem> MapLookupItem(IList<Dto.TranscriptRequestStatus> list);
        IList<LookupItem> MapLookupItem(IList<Dto.TranscriptSource> list);
        IList<LookupItem> MapLookupItem(IList<Dto.UnitOfMeasure> list);

        // Report Export
        IList<ProgramExport> MapProgramExports(
            IList<Dto.Program> list,
            CollegeApplicationCycle collegeApplicationCycle,
            IList<College> colleges,
            IList<Campus> campuses,
            IList<LookupItem> studyMethods,
            IList<McuCode> mcuCodes,
            IList<SpecialCode> specialCodes,
            IList<LookupItem> programTypes,
            IList<LookupItem> promotions,
            IList<LookupItem> programLengths,
            IList<LookupItem> adultTrainings,
            IList<LookupItem> credentials,
            IList<LookupItem> studyAreas,
            IList<LookupItem> highlyComptitives,
            IList<LookupItem> programLanguages,
            IList<LookupItem> entryLevels,
            IList<LookupItem> ministryApprovals,
            IList<LookupItem> programCategories,
            IList<SubCategory> programSubCategories);
    }
}
