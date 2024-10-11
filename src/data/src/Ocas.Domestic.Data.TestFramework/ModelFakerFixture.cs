using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Ocas.Domestic.Data.TestFramework.RuleCollections;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.TestFramework
{
    public class ModelFakerFixture
    {
        private readonly SeedDataFixture _seedDataFixture;

        public Faker<AcademicRecordBase> AcademicRecordBase { get; set; }
        public Faker<Address> Address { get; set; }
        public Faker<ApplicationBase> ApplicationBase { get; set; }
        public Faker<Contact> Contact { get; set; }
        public Faker<ContactBase> ContactBase { get; set; }
        public Faker<EducationBase> EducationBase { get; set; }
        public Faker<OntarioStudentCourseCreditBase> OntarioStudentCourseCreditBase { get; set; }
        public Faker<ProgramBase> ProgramBase { get; set; }
        public Faker<ProgramChoiceBase> ProgramChoiceBase { get; set; }
        public Faker<ProgramEntryLevelBase> ProgramEntryLevelBase { get; set; }
        public Faker<ProgramIntakeBase> ProgramIntakeBase { get; set; }
        public Faker<ProgramIntakeEntrySemesterBase> ProgramIntakeEntrySemesterBase { get; set; }
        public Faker<ProgramIntake> ProgramIntake { get; set; }
        public Faker<GetProgramsOptions> GetProgramsOptions { get; }
        public Faker<TestBase> TestBase { get; set; }
        public Faker<Test> Test { get; set; }
        public Faker<TranscriptBase> TranscriptBase { get; set; }
        public Faker<Transcript> Transcript { get; set; }
        public Faker<TranscriptRequestBase> TranscriptRequestBase { get; set; }
        public Faker<TranscriptRequest> TranscriptRequest { get; set; }
        public Faker<TranscriptRequestLogBase> TranscriptRequestLogBase { get; set; }
        public Faker<TranscriptRequestLog> TranscriptRequestLog { get; set; }

        public Program Program { get; }

        public ModelFakerFixture()
        {
            _seedDataFixture = new SeedDataFixture();

            AcademicRecordBase = new Faker<AcademicRecordBase>()
                .ApplyAcademicRecordBaseRules(_seedDataFixture);

            Address = new Faker<Address>()
                .ApplyAddressRules(_seedDataFixture);

            ApplicationBase = new Faker<ApplicationBase>()
                .ApplyApplicationBaseRules(_seedDataFixture);

            Contact = new Faker<Contact>()
                .ApplyContactRules(_seedDataFixture);

            ContactBase = new Faker<ContactBase>()
                .ApplyContactBaseRules(_seedDataFixture);

            EducationBase = new Faker<EducationBase>()
                .ApplyEducationBaseRules(_seedDataFixture);

            OntarioStudentCourseCreditBase = new Faker<OntarioStudentCourseCreditBase>()
                .ApplyOntarioStudentCourseCreditBaseRules(_seedDataFixture);

            ProgramBase = new Faker<ProgramBase>()
                .ApplyProgramBaseRules(_seedDataFixture);

            ProgramChoiceBase = new Faker<ProgramChoiceBase>()
                .ApplyProgramChoiceBaseRules(_seedDataFixture);

            ProgramEntryLevelBase = new Faker<ProgramEntryLevelBase>()
                .ApplyProgramEntryLevelBaseRules(_seedDataFixture);

            ProgramIntakeBase = new Faker<ProgramIntakeBase>()
                .ApplyProgramIntakeBaseRules(_seedDataFixture);

            ProgramIntakeEntrySemesterBase = new Faker<ProgramIntakeEntrySemesterBase>()
                .ApplyProgramIntakeEntrySemesterBaseRules(_seedDataFixture);

            ProgramIntake = new Faker<ProgramIntake>()
                .ApplyProgramIntakeBaseRules(_seedDataFixture)
                .RuleFor(o => o.Id, _ => Guid.NewGuid());

            Program = new Faker<Program>()
                .ApplyProgramBaseRules(_seedDataFixture)
                .RuleFor(o => o.Id, _ => Guid.NewGuid());

            GetProgramsOptions = new Faker<GetProgramsOptions>()
                .ApplyGetProgramsOptionsRules(_seedDataFixture);

            Test = new Faker<Test>()
                .ApplyTestBaseRules(_seedDataFixture)
                .RuleFor(o => o.Id, _ => Guid.NewGuid());

            TestBase = new Faker<TestBase>()
                .ApplyTestBaseRules(_seedDataFixture);

            Transcript = new Faker<Transcript>()
                .ApplyTranscriptBaseRules()
                .RuleFor(o => o.Id, _ => Guid.NewGuid());

            TranscriptBase = new Faker<TranscriptBase>()
                .ApplyTranscriptBaseRules();

            TranscriptRequestBase = new Faker<TranscriptRequestBase>()
                .ApplyTranscriptRequestBaseRules(_seedDataFixture);

            TranscriptRequest = new Faker<TranscriptRequest>()
                .ApplyTranscriptRequestBaseRules(_seedDataFixture)
                .RuleFor(o => o.Id, _ => Guid.NewGuid());

            TranscriptRequestLogBase = new Faker<TranscriptRequestLogBase>()
                .ApplyTranscriptRequestBaseRules();

            TranscriptRequestLog = new Faker<TranscriptRequestLog>()
                .ApplyTranscriptRequestBaseRules()
                .RuleFor(o => o.Id, _ => Guid.NewGuid());
        }

        public Guid GenerateProgramCategoryWithSubCategory()
        {
            var rand = new Random();
            var counter = 0;

            while (counter < 10)
            {
                var categoryId = _seedDataFixture.ProgramCategories.ElementAt(rand.Next(_seedDataFixture.ProgramCategories.Count())).Id;
                if (_seedDataFixture.ProgramSubCategories.Any(s => s.ProgramCategoryId == categoryId))
                {
                    return categoryId;
                }

                counter++;
            }

            throw new Exception("A program category with sub-categories could not be found");
        }

        public Guid GenerateCollegeApplicationCycleWithProgramSpecialCode()
        {
            var rand = new Random();
            var counter = 0;

            while (counter < 10)
            {
                var applicationId = _seedDataFixture.CollegeApplicationCycles.ElementAt(rand.Next(_seedDataFixture.CollegeApplicationCycles.Count())).Id;
                if (_seedDataFixture.ProgramSpecialCodes.Any(s => s.CollegeApplicationId == applicationId))
                {
                    return applicationId;
                }

                counter++;
            }

            throw new Exception("A program college application with program special code could not be found");
        }

        public List<string> GenerateStartDates(string appCycleYear)
        {
            var validStartDates = new List<string>();
            int.TryParse(appCycleYear, out var iAppCycleYear);
            var nextAppCycleYear = (iAppCycleYear + 1).ToString();

            for (var i = 8; i <= 19; i++)
            {
                var year = i > 12 ? nextAppCycleYear : appCycleYear;
                var month = i > 12 ? i - 12 : i;
                var paddedMonth = month.ToString("#00");
                validStartDates.Add(year.Substring(2) + paddedMonth);
            }
            return validStartDates;
        }
    }
}