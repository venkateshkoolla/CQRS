using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class LevelAchievedTests : BaseTest
    {
        [Fact]
        public async Task GetLevelAchieveds_ShouldPass()
        {
            var enResult = await Context.GetLevelAchieveds(Locale.English);
            var frResult = await Context.GetLevelAchieveds(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetLevelAchieved_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.LevelAchieveds);
            var getLevelAchieved = await Context.GetLevelAchieved(result.Id, Locale.English);

            getLevelAchieved.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetLanguage_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.LevelAchieveds);
            var getLevelAchieved = await Context.GetLevelAchieved(result.Id, Locale.French);

            getLevelAchieved.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
