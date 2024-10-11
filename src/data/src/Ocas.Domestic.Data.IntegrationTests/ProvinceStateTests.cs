using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProvinceStateTests : BaseTest
    {
        [Fact]
        public async Task GetProvinceStates_ShouldPass()
        {
            var enResult = await Context.GetProvinceStates(Locale.English);
            var frResult = await Context.GetProvinceStates(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetProvinceStates_ShouldHaveCountryId()
        {
            var provinces = await Context.GetProvinceStates(Locale.English);
            var canada = await Context.GetCountry("Canada", Locale.English);
            var unitedStates = await Context.GetCountry("United States", Locale.English);

            provinces.Should().OnlyContain(x => x.CountryId == canada.Id || x.CountryId == unitedStates.Id);
        }

        [Fact]
        public async Task GetProvinceState_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProvinceStates);
            var provinceState = await Context.GetProvinceState(result.Id, Locale.English);

            provinceState.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetProvinceState_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProvinceStates);
            var provinceState = await Context.GetProvinceState(result.Id, Locale.French);

            provinceState.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
