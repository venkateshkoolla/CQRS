using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class UniversityTests : BaseTest
    {
        [Fact]
        public async Task GetUniversities_ShouldPass()
        {
            var enResult = await Context.GetUniversities();

            enResult.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetUniversity_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Universities);
            var university = await Context.GetUniversity(result.Id);

            university.Should().BeEquivalentTo(result);
        }
    }
}
