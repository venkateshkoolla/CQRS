using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramIntakeEntrySemesterTests : BaseTest
    {
        [Fact]
        public async Task GetProgramIntakeEntrySemester_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var crmIntake = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                crmIntake.ProgramId = program.Id;
                crmIntake.HasSemesterOverride = null;
                crmIntake.DefaultEntrySemesterId = null;
                var programIntake = await Context.CreateProgramIntake(crmIntake);
                var entryLevel = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels);

                var programIntakeEntrySemesterBase = new ProgramIntakeEntrySemesterBase
                {
                    Name = $"{programIntake.Name} - {entryLevel.Name}",
                    EntrySemesterId = entryLevel.Id,
                    ProgramIntakeId = programIntake.Id
                };
                var intakeEntrySemester = await Context.CreateProgramIntakeEntrySemester(programIntakeEntrySemesterBase);
                id = intakeEntrySemester.Id;

                // Act
                var result = await Context.GetProgramIntakeEntrySemester(intakeEntrySemester.Id);

                // Assert
                intakeEntrySemester.Id.Should().NotBeEmpty();
                result.Should().BeEquivalentTo(intakeEntrySemester);
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntakeEntrySemester(id.Value);
            }
        }

        [Fact]
        public async Task CreateProgramIntakeEntrySemester_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var crmIntake = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                crmIntake.ProgramId = program.Id;
                crmIntake.HasSemesterOverride = null;
                crmIntake.DefaultEntrySemesterId = null;
                var programIntake = await Context.CreateProgramIntake(crmIntake);
                var entryLevel = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels);

                var programIntakeEntrySemesterBase = new ProgramIntakeEntrySemesterBase
                {
                    Name = $"{programIntake.Name} - {entryLevel.Name}",
                    EntrySemesterId = entryLevel.Id,
                    ProgramIntakeId = programIntake.Id
                };

                // Act
                var intakeEntrySemester = await Context.CreateProgramIntakeEntrySemester(programIntakeEntrySemesterBase);

                id = intakeEntrySemester.Id;

                // Assert
                intakeEntrySemester.Id.Should().NotBeEmpty();
                intakeEntrySemester.Should().BeEquivalentTo(programIntakeEntrySemesterBase);
                intakeEntrySemester.CreatedOn.Value.Should().BeCloseTo(DateTime.UtcNow, TestConstants.Config.CommandTimeout.Seconds());
                intakeEntrySemester.ModifiedOn.Value.Should().BeCloseTo(DateTime.UtcNow, TestConstants.Config.CommandTimeout.Seconds());
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntakeEntrySemester(id.Value);
            }
        }

        [Fact]
        public async Task DeleteProgramIntakeEntrySemester_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var crmIntake = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                crmIntake.ProgramId = program.Id;
                crmIntake.HasSemesterOverride = null;
                crmIntake.DefaultEntrySemesterId = null;
                var programIntake = await Context.CreateProgramIntake(crmIntake);
                var entryLevel = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels);

                var programIntakeEntrySemesterBase = new ProgramIntakeEntrySemesterBase
                {
                    Name = $"{programIntake.Name} - {entryLevel.Name}",
                    EntrySemesterId = entryLevel.Id,
                    ProgramIntakeId = programIntake.Id
                };
                var intakeEntrySemester = await Context.CreateProgramIntakeEntrySemester(programIntakeEntrySemesterBase);
                id = intakeEntrySemester.Id;

                // Act
                await Context.DeleteProgramIntakeEntrySemester(intakeEntrySemester.Id);

                // Assert
                var result = await Context.GetProgramIntakeEntrySemester(intakeEntrySemester.Id);
                id = null;
                result.Should().BeNull();
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntakeEntrySemester(id.Value);
            }
        }

        [Fact]
        public async Task UpdateProgramIntakeEntrySemester_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var programEntryLevel = DataFakerFixture.Models.ProgramEntryLevelBase.Generate();
                programEntryLevel.ProgramId = program.Id;
                await Context.CreateProgramEntryLevel(programEntryLevel);
                var crmIntake = DataFakerFixture.Models.ProgramIntakeBase.Generate();
                crmIntake.ProgramId = program.Id;
                crmIntake.HasSemesterOverride = null;
                crmIntake.DefaultEntrySemesterId = null;
                var programIntake = await Context.CreateProgramIntake(crmIntake);
                var entryLevel = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels);

                var programIntakeEntrySemesterBase = new ProgramIntakeEntrySemesterBase
                {
                    Name = $"{programIntake.Name} - {entryLevel.Name}",
                    EntrySemesterId = entryLevel.Id,
                    ProgramIntakeId = programIntake.Id
                };
                var intakeEntrySemester = await Context.CreateProgramIntakeEntrySemester(programIntakeEntrySemesterBase);
                id = intakeEntrySemester.Id;

                var updatedEntryLevel = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels.Where(x => x.Id != entryLevel.Id));
                intakeEntrySemester.EntrySemesterId = updatedEntryLevel.Id;
                intakeEntrySemester.Name = $"{programIntake.Name} - {updatedEntryLevel.Name}";

                // Act
                var updatedProgramIntakeEntrySemester = await Context.UpdateProgramIntakeEntrySemester(intakeEntrySemester);

                // Assert
                intakeEntrySemester.Id.Should().NotBeEmpty();
                intakeEntrySemester.Should().BeEquivalentTo(updatedProgramIntakeEntrySemester, opt =>
                                opt.Excluding(z => z.ModifiedOn));
                intakeEntrySemester.ModifiedOn.Value.Should().BeCloseTo(DateTime.UtcNow, TestConstants.Config.CommandTimeout.Seconds());
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramIntakeEntrySemester(id.Value);
            }
        }
    }
}
