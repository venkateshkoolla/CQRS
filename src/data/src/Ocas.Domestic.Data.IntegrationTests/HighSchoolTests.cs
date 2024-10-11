using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class HighSchoolTests : BaseTest
    {
        [Fact]
        public async Task GetHighSchools_ShouldPass()
        {
            var enResult = await Context.GetHighSchools(Locale.English);
            var frResult = await Context.GetHighSchools(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetHighSchool_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.HighSchools);
            var highSchool = await Context.GetHighSchool(result.Id, Locale.English);

            highSchool.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetHighSchool_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.HighSchools);
            var highSchool = await Context.GetHighSchool(result.Id, Locale.French);

            highSchool.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.SchoolStatus).Excluding(p => p.SchoolType));
        }
    }
}
