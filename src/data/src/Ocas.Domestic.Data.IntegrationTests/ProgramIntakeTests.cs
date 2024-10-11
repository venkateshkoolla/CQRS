using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramIntakeTests : BaseTest
    {
        [Fact]
        public async Task CreateProgramIntake_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                programEntryLevel.EntryLevelId = program.DefaultEntryLevelId;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var applicationCycle = await Context.GetApplicationCycle(DataFakerFixture.SeedData.CollegeApplicationCycles.First(x => x.Id == program.CollegeApplicationCycleId).ApplicationCycleId);
                var intakeBase = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                intakeBase.ProgramId = program.Id;
                intakeBase.StartDate = DataFakerFixture.Faker.PickRandom(DataFakerFixture.Models.GenerateStartDates(applicationCycle.Year));
                intakeBase.Name = $"{program.Code}-{intakeBase.StartDate}";
                intakeBase.HasSemesterOverride = null;
                intakeBase.DefaultEntrySemesterId = null;

                // Act
                var intake = await Context.CreateProgramIntake(intakeBase);
                id = intake.Id;

                // Assert
                intake.Should().NotBeNull();
                intake.Id.Should().NotBeEmpty();
                intake.Should().BeEquivalentTo(intakeBase, opt =>
                        opt.Excluding(z => z.ExpiryDate)
                        .Excluding(z => z.CreatedOn)
                        .Excluding(z => z.ModifiedOn)
                        .Excluding(z => z.Status)
                        .Excluding(z => z.State)
                        .Excluding(z => z.ModifiedBy)
                        .Excluding(z => z.DefaultEntrySemesterId));
                intake.ApplicationCycleId.Should().Be(applicationCycle.Id);
                intake.PromotionId.Should().Be(program.PromotionId);
                intake.EntryLevels.Should().ContainSingle(e => e == programEntryLevel.EntryLevelId);
                intake.ExpiryDate.Value.Should().BeCloseTo(intakeBase.ExpiryDate.Value, TestConstants.Config.CommandTimeout.Seconds());
                intake.CreatedOn.Value.Should().BeCloseTo(DateTime.UtcNow, TestConstants.Config.CommandTimeout.Seconds());
                intake.ModifiedOn.Value.Should().BeCloseTo(DateTime.UtcNow, TestConstants.Config.CommandTimeout.Seconds());
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntake(id.Value);
            }
        }

        [Fact]
        public async Task DeleteProgramIntake_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                programEntryLevel.EntryLevelId = program.DefaultEntryLevelId;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var applicationCycle = await Context.GetApplicationCycle(DataFakerFixture.SeedData.CollegeApplicationCycles.First(x => x.Id == program.CollegeApplicationCycleId).ApplicationCycleId);
                var intakeBase = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                intakeBase.ProgramId = program.Id;
                intakeBase.StartDate = DataFakerFixture.Faker.PickRandom(DataFakerFixture.Models.GenerateStartDates(applicationCycle.Year));
                intakeBase.Name = $"{program.Code}-{intakeBase.StartDate}";
                intakeBase.HasSemesterOverride = null;
                intakeBase.DefaultEntrySemesterId = null;
                var newIntake = await Context.CreateProgramIntake(intakeBase);
                id = newIntake.Id;

                // Act
                await Context.DeleteProgramIntake(newIntake.Id);
                id = null;

                // Assert
                var intake = await Context.GetProgramIntake(newIntake.Id);
                intake.Should().BeNull();
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntake(id.Value);
            }
        }

        [Fact]
        public async Task GetProgramIntake_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                programEntryLevel.EntryLevelId = program.DefaultEntryLevelId;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var applicationCycle = await Context.GetApplicationCycle(DataFakerFixture.SeedData.CollegeApplicationCycles.First(x => x.Id == program.CollegeApplicationCycleId).ApplicationCycleId);
                var intakeBase = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                intakeBase.ProgramId = program.Id;
                intakeBase.StartDate = DataFakerFixture.Faker.PickRandom(DataFakerFixture.Models.GenerateStartDates(applicationCycle.Year));
                intakeBase.Name = $"{program.Code}-{intakeBase.StartDate}";
                intakeBase.HasSemesterOverride = null;
                intakeBase.DefaultEntrySemesterId = null;
                var newIntake = await Context.CreateProgramIntake(intakeBase);
                id = newIntake.Id;

                // Act
                var intake = await Context.GetProgramIntake(newIntake.Id);

                // Assert
                intake.Should().NotBeNull();
                intake.Should().BeEquivalentTo(newIntake);
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntake(id.Value);
            }
        }

        [Fact]
        public async Task GetProgramIntake_ShouldPass_When_EntrySemesterOverride()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                programEntryLevel.EntryLevelId = program.DefaultEntryLevelId;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var applicationCycle = await Context.GetApplicationCycle(DataFakerFixture.SeedData.CollegeApplicationCycles.First(x => x.Id == program.CollegeApplicationCycleId).ApplicationCycleId);
                var intakeBase = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                intakeBase.ProgramId = program.Id;
                intakeBase.StartDate = DataFakerFixture.Faker.PickRandom(DataFakerFixture.Models.GenerateStartDates(applicationCycle.Year));
                intakeBase.Name = $"{program.Code}-{intakeBase.StartDate}";
                intakeBase.HasSemesterOverride = null;
                intakeBase.DefaultEntrySemesterId = null;
                var newIntake = await Context.CreateProgramIntake(intakeBase);
                var intakeEntrySemster = DataFakerFixture.Models.ProgramIntakeEntrySemesterBase.Generate();
                intakeEntrySemster.ProgramIntakeId = newIntake.Id;
                await Context.CreateProgramIntakeEntrySemester(intakeEntrySemster);
                newIntake.HasSemesterOverride = true;
                newIntake.DefaultEntrySemesterId = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels).Id;
                var updateIntake = await Context.UpdateProgramIntake(newIntake);

                id = updateIntake.Id;

                // Act
                var intake = await Context.GetProgramIntake(updateIntake.Id);

                // Assert
                intake.Should().NotBeNull();
                intake.HasSemesterOverride.Should().BeTrue();
                intake.DefaultEntrySemesterId.Should().NotBeNull()
                    .And.Be(updateIntake.DefaultEntrySemesterId);
                intake.Should().BeEquivalentTo(updateIntake);
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntake(id.Value);
            }
        }

        [Fact]
        public async Task GetProgramIntakes_ShouldPass_When_By_ProgramId()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                programEntryLevel.EntryLevelId = program.DefaultEntryLevelId;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var applicationCycle = await Context.GetApplicationCycle(DataFakerFixture.SeedData.CollegeApplicationCycles.First(x => x.Id == program.CollegeApplicationCycleId).ApplicationCycleId);
                var intakeBase = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                intakeBase.ProgramId = program.Id;
                intakeBase.StartDate = DataFakerFixture.Faker.PickRandom(DataFakerFixture.Models.GenerateStartDates(applicationCycle.Year));
                intakeBase.Name = $"{program.Code}-{intakeBase.StartDate}";
                intakeBase.HasSemesterOverride = null;
                intakeBase.DefaultEntrySemesterId = null;
                var newIntake = await Context.CreateProgramIntake(intakeBase);
                id = newIntake.Id;

                // Act
                var programIntakes = await Context.GetProgramIntakes(program.Id);

                // Assert
                programIntakes.Should().NotBeNullOrEmpty()
                    .And.ContainSingle();
                var intake = programIntakes.First();
                intake.Should().BeEquivalentTo(newIntake);
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntake(id.Value);
            }
        }

        [Fact]
        public async Task GetProgramIntakes_ShouldPass()
        {
            var programIntakes = await Context.GetProgramIntakes(new Models.GetProgramIntakeOptions { Ids = TestConstants.ProgramIntake.Ids });

            programIntakes.Should().NotBeNullOrEmpty();
            programIntakes.Should().HaveSameCount(TestConstants.ProgramIntake.Ids);
        }

        [Fact]
        public async Task GetProgramIntakes_ShouldPass_When_Options()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                programEntryLevel.EntryLevelId = program.DefaultEntryLevelId;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var applicationCycle = await Context.GetApplicationCycle(DataFakerFixture.SeedData.CollegeApplicationCycles.First(x => x.Id == program.CollegeApplicationCycleId).ApplicationCycleId);
                var intakeBase = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                intakeBase.ProgramId = program.Id;
                intakeBase.StartDate = DataFakerFixture.Faker.PickRandom(DataFakerFixture.Models.GenerateStartDates(applicationCycle.Year));
                intakeBase.Name = $"{program.Code}-{intakeBase.StartDate}";
                intakeBase.HasSemesterOverride = null;
                intakeBase.DefaultEntrySemesterId = null;
                var newIntake = await Context.CreateProgramIntake(intakeBase);
                id = newIntake.Id;

                var programIntakes = await Context.GetProgramIntakes(new Models.GetProgramIntakeOptions
                {
                    Ids = new List<Guid>
                            {
                                newIntake.Id
                            },
                    ApplicationCycleId = newIntake.ApplicationCycleId,
                    CollegeId = program.CollegeId,
                    ProgramTitle = newIntake.ProgramTitle,
                    FromDate = intakeBase.StartDate,
                    ProgramCode = newIntake.ProgramCode,
                    CampusId = newIntake.CampusId,
                    ProgramDeliveryId = program.DeliveryId
                });

                programIntakes.Should().NotBeNullOrEmpty();
                programIntakes.Should().OnlyContain(x => x.Id == newIntake.Id);
                programIntakes.Should().OnlyContain(x => x.CampusId == newIntake.CampusId);
                programIntakes.Should().OnlyContain(x => x.CollegeApplicationCycleId == newIntake.CollegeApplicationCycleId);
                programIntakes.Should().OnlyContain(x => x.ApplicationCycleId == newIntake.ApplicationCycleId);
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntake(id.Value);
            }
        }

        [Fact]
        public async Task UpdateProgramIntake_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                programEntryLevel.EntryLevelId = program.DefaultEntryLevelId;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var applicationCycle = await Context.GetApplicationCycle(DataFakerFixture.SeedData.CollegeApplicationCycles.First(x => x.Id == program.CollegeApplicationCycleId).ApplicationCycleId);
                var intakeBase = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                intakeBase.ProgramId = program.Id;
                intakeBase.StartDate = DataFakerFixture.Faker.PickRandom(DataFakerFixture.Models.GenerateStartDates(applicationCycle.Year));
                intakeBase.Name = $"{program.Code}-{intakeBase.StartDate}";
                intakeBase.HasSemesterOverride = null;
                intakeBase.DefaultEntrySemesterId = null;
                var newIntake = await Context.CreateProgramIntake(intakeBase);
                id = newIntake.Id;

                var updateIntake = DataFakerFixture.Models.ProgramIntake.Generate();
                updateIntake.Id = newIntake.Id;
                updateIntake.ProgramId = program.Id;
                updateIntake.StartDate = DataFakerFixture.Faker.PickRandom(DataFakerFixture.Models.GenerateStartDates(applicationCycle.Year));
                updateIntake.Name = $"{program.Code}-{updateIntake.StartDate}";
                updateIntake.HasSemesterOverride = newIntake.HasSemesterOverride;
                updateIntake.DefaultEntrySemesterId = newIntake.DefaultEntrySemesterId;
                updateIntake.ProgramCode = newIntake.ProgramCode;
                updateIntake.ProgramTitle = newIntake.ProgramTitle;
                updateIntake.CampusId = newIntake.CampusId;
                updateIntake.CollegeId = newIntake.CollegeId;
                updateIntake.ModifiedBy = newIntake.ModifiedBy;

                // Act
                var updatedIntake = await Context.UpdateProgramIntake(updateIntake);

                // Assert
                updatedIntake.Should().BeEquivalentTo(updateIntake, opt =>
                        opt.Excluding(z => z.CollegeApplicationCycleId)
                        .Excluding(z => z.ApplicationCycleId)
                        .Excluding(z => z.PromotionId)
                        .Excluding(z => z.DefaultEntrySemesterId)
                        .Excluding(z => z.EntryLevels)
                        .Excluding(z => z.ExpiryDate)
                        .Excluding(z => z.CreatedOn)
                        .Excluding(z => z.ModifiedOn)
                        .Excluding(z => z.Status)
                        .Excluding(z => z.ProgramDeliveryId)
                        .Excluding(z => z.State));
                updatedIntake.CollegeApplicationCycleId.Should().Be(program.CollegeApplicationCycleId);
                updatedIntake.PromotionId.Should().Be(newIntake.PromotionId);
                updatedIntake.EntryLevels.Should().ContainSingle(e => e == programEntryLevel.EntryLevelId);
                updatedIntake.CreatedOn.Value.Should().BeCloseTo(newIntake.CreatedOn.Value, TestConstants.Config.CommandTimeout.Seconds());
                updatedIntake.ExpiryDate.Value.Should().BeCloseTo(updateIntake.ExpiryDate.Value, TestConstants.Config.CommandTimeout.Seconds());
                updatedIntake.ModifiedOn.Value.Should().BeCloseTo(DateTime.UtcNow, TestConstants.Config.CommandTimeout.Seconds());
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntake(id.Value);
            }
        }
    }
}
