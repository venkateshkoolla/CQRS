using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class HighlyCompetitiveTests : BaseTest
    {
        [Fact]
        public async Task GetHighlyCompetitives_ShouldPass()
        {
            var enResult = await Context.GetHighlyCompetitives(Locale.English);
            var frResult = await Context.GetHighlyCompetitives(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetHighlyCompetitive_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.HighlyCompetitives);
            var highlyCompetitive = await Context.GetHighlyCompetitive(result.Id, Locale.English);

            highlyCompetitive.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetHighlyCompetitive_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.HighlyCompetitives);
            var highlyCompetitive = await Context.GetHighlyCompetitive(result.Id, Locale.French);

            highlyCompetitive.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
