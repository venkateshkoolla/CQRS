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
    public class ProgramEntryLevelTests : BaseTest
    {
        [Fact]
        public async Task GetProgramEntryLevel_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var entryLevel = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels);

                var programEntryLevelBase = new ProgramEntryLevelBase
                {
                    Name = $"{program.Title} - {entryLevel.Name}",
                    EntryLevelId = entryLevel.Id,
                    ProgramId = program.Id
                };
                var programEntryLevel = await Context.CreateProgramEntryLevel(programEntryLevelBase);
                id = programEntryLevel.Id;

                // Act
                var result = await Context.GetProgramEntryLevel(programEntryLevel.Id);

                // Assert
                programEntryLevel.Id.Should().NotBeEmpty();
                result.Should().BeEquivalentTo(programEntryLevel);
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramEntryLevel(id.Value);
            }
        }

        [Fact]
        public async Task CreateProgramEntryLevel_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var entryLevel = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels);

                var programEntryLevelBase = new ProgramEntryLevelBase
                {
                    Name = $"{program.Title} - {entryLevel.Name}",
                    EntryLevelId = entryLevel.Id,
                    ProgramId = program.Id
                };

                // Act
                var programEntryLevel = await Context.CreateProgramEntryLevel(programEntryLevelBase);

                id = programEntryLevel.Id;

                // Assert
                programEntryLevel.Id.Should().NotBeEmpty();
                programEntryLevel.Should().BeEquivalentTo(programEntryLevelBase);
                programEntryLevel.CreatedOn.Value.Should().BeCloseTo(DateTime.UtcNow, TestConstants.Config.CommandTimeout.Seconds());
                programEntryLevel.ModifiedOn.Value.Should().BeCloseTo(DateTime.UtcNow, TestConstants.Config.CommandTimeout.Seconds());
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramEntryLevel(id.Value);
            }
        }

        [Fact]
        public async Task DeleteProgramEntryLevel_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var entryLevel = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels);

                var programEntryLevelBase = new ProgramEntryLevelBase
                {
                    Name = $"{program.Title} - {entryLevel.Name}",
                    EntryLevelId = entryLevel.Id,
                    ProgramId = program.Id
                };
                var programEntryLevel = await Context.CreateProgramEntryLevel(programEntryLevelBase);
                id = programEntryLevel.Id;

                // Act
                await Context.DeleteProgramEntryLevel(programEntryLevel.Id);

                // Assert
                var result = await Context.GetProgramEntryLevel(programEntryLevel.Id);
                id = null;
                result.Should().BeNull();
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramEntryLevel(id.Value);
            }
        }

        [Fact]
        public async Task UpdateProgramEntryLevel_ShouldPass()
        {
            Guid? id = null;
            try
            {
                // Arrange
                var program = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                var entryLevel = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels);

                var programEntryLevelBase = new ProgramEntryLevelBase
                {
                    Name = $"{program.Title} - {entryLevel.Name}",
                    EntryLevelId = entryLevel.Id,
                    ProgramId = program.Id
                };
                var programEntryLevel = await Context.CreateProgramEntryLevel(programEntryLevelBase);
                id = programEntryLevel.Id;

                var updatedEntryLevel = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.EntryLevels.Where(x => x.Id != entryLevel.Id));
                programEntryLevel.EntryLevelId = updatedEntryLevel.Id;
                programEntryLevel.Name = $"{program.Title} - {updatedEntryLevel.Name}";

                // Act
                var updatedProgramEntryLevel = await Context.UpdateProgramEntryLevel(programEntryLevel);

                // Assert
                programEntryLevel.Id.Should().NotBeEmpty();
                programEntryLevel.Should().BeEquivalentTo(updatedProgramEntryLevel, opt =>
                                opt.Excluding(z => z.ModifiedOn));
                programEntryLevel.ModifiedOn.Value.Should().BeCloseTo(DateTime.UtcNow, TestConstants.Config.CommandTimeout.Seconds());
            }
            finally
            {
                if (id.HasValue)
                    await Context.DeleteProgramEntryLevel(id.Value);
            }
        }
    }
}
