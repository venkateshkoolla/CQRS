using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CityTests : BaseTest
    {
        [Fact]
        public async Task GetCityStatuses_ShouldPass()
        {
            var enResult = await Context.GetCities(Locale.English);
            var frResult = await Context.GetCities(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetCity_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Cities);
            var city = await Context.GetCity(result.Id, Locale.English);

            city.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCity_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Cities);
            var city = await Context.GetCity(result.Id, Locale.French);

            city.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
