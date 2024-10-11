using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramTests : BaseTest
    {
        [Fact]
        public async Task GetPrograms_ShouldPass()
        {
            var programOptions = DataFakerFixture.Models.GetProgramsOptions.Generate();

            var result = await Context.GetPrograms(programOptions);

            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetProgramApplications_ShouldPass()
        {
            // Arrange
            var applicationStatusId = DataFakerFixture.SeedData.ApplicationStatuses.First(a => a.Code == Constants.ApplicationStatuses.Active).Id;
            var options = new GetProgramApplicationsOptions { ProgramId = TestConstants.Program.IdWithApplications, ApplicationStatusId = applicationStatusId };

            // Act
            var results = await Context.GetProgramApplications(options);

            // Assert
            results.Should().NotBeEmpty()
                .And.OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task CreateProgram_ShouldPass()
        {
            Guid? id = null;
            try
            {
                var program = DataFakerFixture.Models.ProgramBase.Generate();

                await Context.BeginTransaction();
                var result = await Context.CreateProgram(program);
                await Context.CommitTransaction();

                id = result.Id;
                result.Id.Should().NotBeEmpty();
            }
            finally
            {
                if (id.HasValue)
                {
                    await Context.BeginTransaction();
                    var result = await Context.GetProgram(id.Value);
                    await Context.DeleteProgram(result);
                    await Context.CommitTransaction();
                }
            }
        }

        [Fact]
        public async Task UpdateProgram_ShouldPass()
        {
            Guid? id = null;
            try
            {
                var programName = DataFakerFixture.Faker.Name.JobTitle();
                await Context.BeginTransaction();
                var newProgram = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                await Context.CommitTransaction();

                await Context.BeginTransaction();
                newProgram.Title = programName;
                var result = await Context.UpdateProgram(newProgram);
                await Context.CommitTransaction();

                id = result.Id;
                result.Should()
                    .BeEquivalentTo(newProgram, config => config
                        .Using<DateTime>(x => x.Subject.Should().BeAfter(DateTime.Now)).WhenTypeIs<DateTime>()
                        .Excluding(x => x.Title)
                        .Excluding(x => x.Name));
                result.Title.Should().Be(programName);
                result.Name.Should().Be(programName);
            }
            finally
            {
                if (id.HasValue)
                {
                    await Context.BeginTransaction();
                    var result = await Context.GetProgram(id.Value);
                    await Context.DeleteProgram(result);
                    await Context.CommitTransaction();
                }
            }
        }

        [Fact]
        public async Task DeleteProgram_ShouldPass()
        {
            Guid? id = null;
            try
            {
                await Context.BeginTransaction();
                var newProgram = await Context.CreateProgram(DataFakerFixture.Models.ProgramBase.Generate());
                id = newProgram.Id;
                await Context.CommitTransaction();

                await Context.BeginTransaction();
                await Context.DeleteProgram(newProgram);
                await Context.CommitTransaction();

                var result = await Context.GetProgram(id.Value);
                id = null;
                result.Should().BeNull();
            }
            finally
            {
                if (id.HasValue)
                {
                    await Context.BeginTransaction();
                    var result = await Context.GetProgram(id.Value);
                    await Context.DeleteProgram(result);
                    await Context.CommitTransaction();
                }
            }
        }
    }
}