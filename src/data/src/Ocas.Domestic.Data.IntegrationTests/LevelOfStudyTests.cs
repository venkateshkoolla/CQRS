using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class LevelOfStudyTests : BaseTest
    {
        [Fact]
        public async Task GetLevelOfStudies_ShouldPass()
        {
            var enResult = await Context.GetLevelOfStudies(Locale.English);
            var frResult = await Context.GetLevelOfStudies(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetLevelOfStudy_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.LevelOfStudies);
            var levelOfStudy = await Context.GetLevelOfStudy(result.Id, Locale.English);

            levelOfStudy.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetLevelOfStudy_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.LevelOfStudies);
            var levelOfStudy = await Context.GetLevelOfStudy(result.Id, Locale.French);

            levelOfStudy.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
