using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CollegeApplicationCycleTests : BaseTest
    {
        [Fact]
        public async Task GetCollegeApplicationCycles_ShouldPass()
        {
            var result = await Context.GetCollegeApplicationCycles();
            result.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task GetCollegeApplicationCycles_ShouldPass_WhenCollegeAndCycle()
        {
            var options = new GetCollegeApplicationsOptions
            {
                CollegeId = DataFakerFixture.SeedData.Colleges.Single(x => x.Code == TestConstants.TestCollege.Code).Id,
                ApplicationCycleId = DataFakerFixture.SeedData.ApplicationCycles.Single(x => x.Id == TestConstants.ApplicationCycleIds.Y2017).Id
            };

            var result = await Context.GetCollegeApplicationCycles(options);
            result.Should().HaveCountGreaterOrEqualTo(1)
                .And.OnlyContain(o => o.CollegeId == options.CollegeId)
                .And.OnlyContain(o => o.ApplicationCycleId == options.ApplicationCycleId);
        }

        [Fact]
        public async Task GetCollegeApplicationCycles_ShouldPass_WhenCollege()
        {
            var options = new GetCollegeApplicationsOptions
            {
                CollegeId = DataFakerFixture.SeedData.Colleges.Single(x => x.Code == TestConstants.TestCollege.Code).Id
            };

            var result = await Context.GetCollegeApplicationCycles(options);
            result.Should().HaveCountGreaterOrEqualTo(1)
                .And.OnlyContain(o => o.CollegeId == options.CollegeId);
        }

        [Fact]
        public async Task GetCollegeApplicationCycle_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CollegeApplicationCycles);

            var collegeApplicationCycle = await Context.GetCollegeApplicationCycle(result.Id);
            collegeApplicationCycle.Should().BeOfType<CollegeApplicationCycle>()
                .And.BeEquivalentTo(result);
        }
    }
}
