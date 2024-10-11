using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CountryTests : BaseTest
    {
        [Fact]
        public async Task GetCountries_ShouldPass()
        {
            var enResult = await Context.GetCountries(Locale.English);
            var frResult = await Context.GetCountries(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetCountry_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Countries);
            var country = await Context.GetCountry(result.Id, Locale.English);

            country.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCountry_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Countries);
            var country = await Context.GetCountry(result.Id, Locale.French);

            country.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
