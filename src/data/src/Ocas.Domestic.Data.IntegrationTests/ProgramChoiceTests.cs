using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramChoiceTests : BaseTest
    {
        [Fact]
        public async Task CreateProgramChoice_ShouldPass()
        {
            ProgramChoice model = null;
            Contact applicant = null;
            Application application = null;
            ProgramIntake intake = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var applicationCycle = GetApplicationCycle();
                application = await CreateApplication(applicant, applicationCycle);
                intake = await CreateProgramIntake(applicationCycle);

                var programChoiceBase = DataFakerFixture.Models.ProgramChoiceBase.Generate();
                programChoiceBase.ApplicantId = applicant.Id;
                programChoiceBase.ApplicationId = application.Id;
                programChoiceBase.ProgramIntakeId = intake.Id;
                programChoiceBase.ModifiedBy = applicant.Username;

                // Act
                model = await Context.CreateProgramChoice(programChoiceBase);

                // Assert
                model.Id.Should().NotBeEmpty();
                model.Should().BeEquivalentTo(programChoiceBase, opt => opt
                    .Excluding(z => z.ModifiedBy)
                    .Excluding(z => z.CreatedOn)
                    .Excluding(z => z.ModifiedOn)
                    .Excluding(z => z.CreatedOn));
            }
            finally
            {
                // Cleanup
                if (model?.Id != null)
                    await Context.DeleteProgramChoice(model.Id);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (intake?.Id != null)
                    await Context.DeleteProgramIntake(intake.Id);
            }
        }

        [Fact]
        public async Task DeleteProgramChoice_ShouldPass_When_Id()
        {
            ProgramChoice model = null;
            Contact applicant = null;
            Application application = null;
            ProgramIntake intake = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var applicationCycle = GetApplicationCycle();
                application = await CreateApplication(applicant, applicationCycle);
                intake = await CreateProgramIntake(applicationCycle);

                var programChoiceBase = DataFakerFixture.Models.ProgramChoiceBase.Generate();
                programChoiceBase.ApplicantId = applicant.Id;
                programChoiceBase.ApplicationId = application.Id;
                programChoiceBase.ModifiedBy = applicant.Username;
                programChoiceBase.ProgramIntakeId = intake.Id;
                model = await Context.CreateProgramChoice(programChoiceBase);

                // Act
                await Context.DeleteProgramChoice(model.Id);

                // Assert
                var after = await Context.GetProgramChoice(model.Id);
                after.Should().BeNull();
                model = null;
            }
            finally
            {
                // Cleanup
                if (model?.Id != null)
                    await Context.DeleteProgramChoice(model.Id);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (intake?.Id != null)
                    await Context.DeleteProgramIntake(intake.Id);
            }
        }

        [Fact]
        public async Task DeleteProgramChoice_ShouldPass_When_Model()
        {
            ProgramChoice model = null;
            Contact applicant = null;
            Application application = null;
            ProgramIntake intake = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var applicationCycle = GetApplicationCycle();
                application = await CreateApplication(applicant, applicationCycle);
                intake = await CreateProgramIntake(applicationCycle);

                var programChoiceBase = DataFakerFixture.Models.ProgramChoiceBase.Generate();
                programChoiceBase.ApplicantId = applicant.Id;
                programChoiceBase.ApplicationId = application.Id;
                programChoiceBase.ModifiedBy = applicant.Username;
                programChoiceBase.ProgramIntakeId = intake.Id;
                model = await Context.CreateProgramChoice(programChoiceBase);

                // Act
                await Context.DeleteProgramChoice(model);

                // Assert
                var after = await Context.GetProgramChoice(model.Id);
                after.Should().BeNull();
                model = null;
            }
            finally
            {
                // Cleanup
                if (model?.Id != null)
                    await Context.DeleteProgramChoice(model.Id);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (intake?.Id != null)
                    await Context.DeleteProgramIntake(intake.Id);
            }
        }

        [Fact]
        public async Task GetProgramChoice_ShouldPass()
        {
            ProgramChoice model = null;
            Contact applicant = null;
            Application application = null;
            ProgramIntake intake = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var applicationCycle = GetApplicationCycle();
                application = await CreateApplication(applicant, applicationCycle);
                intake = await CreateProgramIntake(applicationCycle);

                var programChoiceBase = DataFakerFixture.Models.ProgramChoiceBase.Generate();
                programChoiceBase.ApplicantId = applicant.Id;
                programChoiceBase.ApplicationId = application.Id;
                programChoiceBase.ModifiedBy = applicant.Username;
                programChoiceBase.ProgramIntakeId = intake.Id;
                model = await Context.CreateProgramChoice(programChoiceBase);

                // Act
                var choice = await Context.GetProgramChoice(model.Id);

                // Assert
                choice.Id.Should().NotBeEmpty();
                choice.Should().BeEquivalentTo(programChoiceBase, opt => opt
                    .Excluding(z => z.ApplicantId)
                    .Excluding(z => z.ApplicationId)
                    .Excluding(z => z.ModifiedBy)
                    .Excluding(z => z.CreatedOn)
                    .Excluding(z => z.ModifiedOn)
                    .Excluding(z => z.CreatedOn));
            }
            finally
            {
                // Cleanup
                if (model?.Id != null)
                    await Context.DeleteProgramChoice(model.Id);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (intake?.Id != null)
                    await Context.DeleteProgramIntake(intake.Id);
            }
        }

        [Fact]
        public async Task GetProgramChoices_ShouldPass_When_ApplicantId()
        {
            ProgramChoice model = null;
            Contact applicant = null;
            Application application = null;
            ProgramIntake intake = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var applicationCycle = GetApplicationCycle();
                application = await CreateApplication(applicant, applicationCycle);
                intake = await CreateProgramIntake(applicationCycle);

                var programChoiceBase = DataFakerFixture.Models.ProgramChoiceBase.Generate();
                programChoiceBase.ApplicantId = applicant.Id;
                programChoiceBase.ApplicationId = application.Id;
                programChoiceBase.ModifiedBy = applicant.Username;
                programChoiceBase.ProgramIntakeId = intake.Id;
                model = await Context.CreateProgramChoice(programChoiceBase);

                // Act
                var choicesOptions = new GetProgramChoicesOptions { ApplicantId = applicant.Id };
                var choices = await Context.GetProgramChoices(choicesOptions);

                // Assert
                choices.Should().OnlyContain(x => x.ApplicantId == applicant.Id);
            }
            finally
            {
                // Cleanup
                if (model?.Id != null)
                    await Context.DeleteProgramChoice(model.Id);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (intake?.Id != null)
                    await Context.DeleteProgramIntake(intake.Id);
            }
        }

        [Fact]
        public async Task GetProgramChoices_ShouldPass_When_ApplicationId()
        {
            ProgramChoice model = null;
            Contact applicant = null;
            Application application = null;
            ProgramIntake intake = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var applicationCycle = GetApplicationCycle();
                application = await CreateApplication(applicant, applicationCycle);
                intake = await CreateProgramIntake(applicationCycle);

                var programChoiceBase = DataFakerFixture.Models.ProgramChoiceBase.Generate();
                programChoiceBase.ApplicantId = applicant.Id;
                programChoiceBase.ApplicationId = application.Id;
                programChoiceBase.ModifiedBy = applicant.Username;
                programChoiceBase.ProgramIntakeId = intake.Id;
                model = await Context.CreateProgramChoice(programChoiceBase);

                // Act
                var choicesOptions = new GetProgramChoicesOptions { ApplicationId = application.Id };
                var choices = await Context.GetProgramChoices(choicesOptions);

                // Assert
                choices.Should().OnlyContain(x => x.ApplicationId == application.Id);
            }
            finally
            {
                // Cleanup
                if (model?.Id != null)
                    await Context.DeleteProgramChoice(model.Id);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (intake?.Id != null)
                    await Context.DeleteProgramIntake(intake.Id);
            }
        }

        [Fact]
        public async Task HasProgramChoices_ShouldPass_WhenNoChoices()
        {
            // Arrange & Act
            var hasChoices = await Context.HasProgramChoices(Guid.NewGuid());

            // Assert
            hasChoices.Should().BeFalse();
        }

        [Fact]
        public async Task HasProgramChoices_ShouldPass()
        {
            ProgramChoice model = null;
            Contact applicant = null;
            Application application = null;
            ProgramIntake intake = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var applicationCycle = GetApplicationCycle();
                application = await CreateApplication(applicant, applicationCycle);
                intake = await CreateProgramIntake(applicationCycle);

                var programChoiceBase = DataFakerFixture.Models.ProgramChoiceBase.Generate();
                programChoiceBase.ApplicantId = applicant.Id;
                programChoiceBase.ApplicationId = application.Id;
                programChoiceBase.ModifiedBy = applicant.Username;
                programChoiceBase.ProgramIntakeId = intake.Id;
                model = await Context.CreateProgramChoice(programChoiceBase);

                // Act
                var hasChoices = await Context.HasProgramChoices(application.Id);

                // Assert
                hasChoices.Should().BeTrue();
            }
            finally
            {
                // Cleanup
                if (model?.Id != null)
                    await Context.DeleteProgramChoice(model.Id);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (intake?.Id != null)
                    await Context.DeleteProgramIntake(intake.Id);
            }
        }

        [Fact]
        public async Task UpdateProgramChoice_ShouldPass()
        {
            ProgramChoice beforeUpdate = null;
            ProgramChoice afterUpdate = null;
            Contact applicant = null;
            Application application = null;
            ProgramIntake intake = null;
            try
            {
                // Arrange
                applicant = await Context.CreateContact(DataFakerFixture.Models.ContactBase.Generate("default,Applicant"));
                var applicationCycle = GetApplicationCycle();
                application = await CreateApplication(applicant, applicationCycle);
                intake = await CreateProgramIntake(applicationCycle);

                var programChoiceBase = DataFakerFixture.Models.ProgramChoiceBase.Generate();
                programChoiceBase.ApplicantId = applicant.Id;
                programChoiceBase.ApplicationId = application.Id;
                programChoiceBase.ModifiedBy = applicant.Username;
                programChoiceBase.ProgramIntakeId = intake.Id;
                beforeUpdate = await Context.CreateProgramChoice(programChoiceBase);

                // Act
                var temp = DataFakerFixture.Models.ProgramChoiceBase.Generate();
                var toBeUpdated = beforeUpdate;
                toBeUpdated.EntryLevelId = temp.EntryLevelId;
                afterUpdate = await Context.UpdateProgramChoice(toBeUpdated);

                // Assert
                afterUpdate.Id.Should().NotBeEmpty();
                afterUpdate.EntryLevelId.Should().Be(temp.EntryLevelId);
                afterUpdate.Should().BeEquivalentTo(toBeUpdated, opt => opt
                    .Excluding(z => z.EntryLevelId)
                    .Excluding(z => z.ModifiedOn));
            }
            finally
            {
                // Cleanup
                if (beforeUpdate?.Id != null)
                    await Context.DeleteProgramChoice(beforeUpdate.Id);

                if (afterUpdate?.Id != null)
                    await Context.DeleteProgramChoice(afterUpdate.Id);

                if (applicant?.Id != null)
                    await Context.DeleteContact(applicant.Id);

                if (application?.Id != null)
                    await Context.DeleteApplication(application.Id);

                if (intake?.Id != null)
                    await Context.DeleteProgramIntake(intake.Id);
            }
        }

        private ApplicationCycle GetApplicationCycle()
        {
            var applicationCycleActiveStatus = DataFakerFixture.SeedData.ApplicationCycleStatuses.Single(x => x.Code == ((char)Enums.ApplicationCycleStatusCode.Active).ToString());
            var applicationCycle = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ApplicationCycles.Where(a => a.StatusId == applicationCycleActiveStatus.Id).ToList());
            return applicationCycle;
        }

        private Task<Application> CreateApplication(Contact applicant, ApplicationCycle applicationCycle)
        {
            var applicationBase = DataFakerFixture.Models.ApplicationBase.Generate();
            applicationBase.ApplicantId = applicant.Id;
            applicationBase.ApplicationCycleId = applicationCycle.Id;
            applicationBase.ModifiedBy = applicant.Username;
            return Context.CreateApplication(applicationBase);
        }

        private async Task<ProgramIntake> CreateProgramIntake(ApplicationCycle applicationCycle)
        {
            var programBase = DataFakerFixture.Models.ProgramBase.Generate();
            programBase.CollegeApplicationCycleId = DataFakerFixture.Faker.PickRandom(
                DataFakerFixture.SeedData.CollegeApplicationCycles.Where(s => s.CollegeId == programBase.CollegeId && s.ApplicationCycleId == applicationCycle.Id)).Id;
            var program = await Context.CreateProgram(programBase);
            var programEntryLevel = new ProgramEntryLevelBase
            {
                ProgramId = program.Id,
                EntryLevelId = program.DefaultEntryLevelId
            };
            await Context.CreateProgramEntryLevel(programEntryLevel);

            var intakeBase = DataFakerFixture.Models.ProgramIntakeBase.Generate();
            intakeBase.ProgramId = program.Id;
            intakeBase.StartDate = DataFakerFixture.Faker.PickRandom(DataFakerFixture.Models.GenerateStartDates(applicationCycle.Year));
            intakeBase.Name = $"{program.Code}-{intakeBase.StartDate}";
            intakeBase.HasSemesterOverride = null;
            intakeBase.DefaultEntrySemesterId = null;
            var intake = await Context.CreateProgramIntake(intakeBase);
            return intake;
        }
    }
}
