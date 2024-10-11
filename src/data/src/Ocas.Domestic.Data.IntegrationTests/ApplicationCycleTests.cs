using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ApplicationCycleTests : BaseTest
    {
        [Fact]
        public async Task GetApplicationCycles_ShouldPass()
        {
            var result = await Context.GetApplicationCycles();
            result.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task GetApplicationCycle_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ApplicationCycles);
            var applicationCycle = await Context.GetApplicationCycle(result.Id);
            applicationCycle.Should().BeOfType<ApplicationCycle>()
                .And.BeEquivalentTo(result);
        }
    }
}
