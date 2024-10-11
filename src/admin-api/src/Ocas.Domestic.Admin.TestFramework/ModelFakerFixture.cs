using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Admin.TestFramework.RuleCollections;
using Ocas.Domestic.Data.TestFramework;
using LookupItem = Ocas.Domestic.Apply.Models.Lookups.LookupItem;

namespace Ocas.Domestic.Apply.Admin.TestFramework
{
    public class ModelFakerFixture : Apply.TestFramework.ModelFakerFixture
    {
        private readonly AllLookups _lookups;

        public ModelFakerFixture()
        {
            _lookups = AllAdminLookups;
        }

        public AllLookups AllAdminLookups
        {
            get
            {
                var adultTrainings = SeedDataFixture.AdultTrainings.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var highlyCompetitives = SeedDataFixture.HighlyCompetitives.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var intakeAvailabilities = SeedDataFixture.ProgramIntakeAvailabilities.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var intakeExpiryActions = SeedDataFixture.ExpiryActions.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var intakeStatuses = SeedDataFixture.ProgramIntakeStatuses.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var mcuCodes = SeedDataFixture.McuCodes.Select(dto =>
                    new McuCode
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Title = dto.LocalizedName
                    }).ToList() as IList<McuCode>;

                var ministryApprovals = SeedDataFixture.MinistryApprovals.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var ostNotes = SeedDataFixture.OstNotes.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var programCategories = SeedDataFixture.ProgramCategories.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var paymentMethods = SeedDataFixture.PaymentMethods.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var programCredentials = SeedDataFixture.Credentials.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var programDeliveries = SeedDataFixture.OfferStudyMethods.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var programLanguages = SeedDataFixture.ProgramLanguages.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var programLengths = SeedDataFixture.UnitOfMeasures.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var programLevels = SeedDataFixture.ProgramLevels.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var programSubCategories = SeedDataFixture.ProgramSubCategories.Select(dto =>
                    new SubCategory
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName,
                        CategoryId = dto.ProgramCategoryId
                    }).ToList() as IList<SubCategory>;

                var programTypes = SeedDataFixture.ProgramTypes.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var supportingDocumentTypes = SeedDataFixture.SupportingDocumentTypes.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var studyAreas = SeedDataFixture.StudyAreas.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                var transcriptSources = SeedDataFixture.TranscriptSources.Select(dto =>
                    new LookupItem
                    {
                        Id = dto.Id,
                        Code = dto.Code,
                        Label = dto.LocalizedName
                    }).ToList() as IList<LookupItem>;

                return new AllLookups
                {
                    AboriginalStatuses = AllApplyLookups.AboriginalStatuses,
                    AdultTrainings = adultTrainings,
                    ApplicationCycles = AllApplyLookups.ApplicationCycles,
                    ApplicationCycleStatuses = AllApplyLookups.ApplicationCycleStatuses,
                    ApplicationStatuses = AllApplyLookups.ApplicationStatuses,
                    Campuses = AllApplyLookups.Campuses,
                    CollegeApplicationCycles = AllApplyLookups.CollegeApplicationCycles,
                    Colleges = AllApplyLookups.Colleges,
                    CommunityInvolvements = AllApplyLookups.CommunityInvolvements,
                    Countries = AllApplyLookups.Countries,
                    CourseDeliveries = AllApplyLookups.CourseDeliveries,
                    CourseStatuses = AllApplyLookups.CourseStatuses,
                    CourseTypes = AllApplyLookups.CourseTypes,
                    Credentials = AllApplyLookups.Credentials,
                    EntryLevels = AllApplyLookups.EntryLevels,
                    GradeTypes = AllApplyLookups.GradeTypes,
                    HighlyCompetitives = highlyCompetitives,
                    HighestEducations = AllApplyLookups.HighestEducations,
                    HighSchools = AllApplyLookups.HighSchools,
                    HighSkillsMajors = AllApplyLookups.HighSkillsMajors,
                    Institutes = AllApplyLookups.Institutes,
                    InstituteTypes = AllApplyLookups.InstituteTypes,
                    IntakeAvailabilities = intakeAvailabilities,
                    IntakeExpiryActions = intakeExpiryActions,
                    IntakeStatuses = intakeStatuses,
                    LiteracyTests = AllApplyLookups.LiteracyTests,
                    McuCodes = mcuCodes,
                    MinistryApprovals = ministryApprovals,
                    OfferStates = AllApplyLookups.OfferStates,
                    Officials = AllApplyLookups.Officials,
                    OstNotes = ostNotes,
                    PaymentMethods = paymentMethods,
                    ProgramCategories = programCategories,
                    ProgramCredentials = programCredentials,
                    ProgramDeliveries = programDeliveries,
                    ProgramLanguages = programLanguages,
                    ProgramLengths = programLengths,
                    ProgramLevels = programLevels,
                    ProgramSubCategories = programSubCategories,
                    ProgramTypes = programTypes,
                    Promotions = AllApplyLookups.Promotions,
                    ProvinceStates = AllApplyLookups.ProvinceStates,
                    Sources = AllApplyLookups.Sources,
                    StandardizedTestTypes = AllApplyLookups.StandardizedTestTypes,
                    StudyAreas = studyAreas,
                    StudyMethods = AllApplyLookups.StudyMethods,
                    SupportingDocumentTypes = AllApplyLookups.SupportingDocumentTypes,
                    Titles = AllApplyLookups.Titles,
                    TranscriptTransmissions = AllApplyLookups.TranscriptTransmissions,
                    TranscriptSources = transcriptSources,
                    Universities = AllApplyLookups.Universities
                };
            }
        }

        public Faker<AcademicRecordBase> GetAcademicRecordBase()
        {
            return new Faker<AcademicRecordBase>()
                 .ApplyAcademicRecordBaseRules(_lookups);
        }

        public Faker<CreateProgramChoiceRequest> GetCreateProgramChoiceRequest()
        {
            return new Faker<CreateProgramChoiceRequest>()
                .ApplyCreateProgramChoiceRequestRules(_lookups);
        }

        public Faker<OntarioStudentCourseCredit> GetOntarioStudentCourseCredit()
        {
            return new Faker<OntarioStudentCourseCredit>()
                   .ApplyOntarioStudentCourseCreditRules(_lookups)
                   .RuleFor(x => x.Id, Guid.NewGuid());
        }

        public Faker<OntarioStudentCourseCreditBase> GetOntarioStudentCourseCreditBase()
        {
            return new Faker<OntarioStudentCourseCreditBase>()
                   .ApplyOntarioStudentCourseCreditRules(_lookups);
        }

        public Faker<Program> GetProgram()
        {
            return new Faker<Program>()
                   .ApplyProgramBaseRules(_lookups)
                   .RuleFor(o => o.Id, Guid.NewGuid());
        }

        public Faker<ProgramBase> GetProgramBase()
        {
            return new Faker<ProgramBase>()
                   .ApplyProgramBaseRules(_lookups);
        }

        public Faker<ProgramIntake> GetProgramIntake(ProgramBase program)
        {
            return new Faker<ProgramIntake>()
                .ApplyProgramIntakeRules(_lookups, program, new List<string>());
        }
    }
}
