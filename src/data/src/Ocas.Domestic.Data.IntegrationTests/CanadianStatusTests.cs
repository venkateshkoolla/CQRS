using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CanadianStatusTests : BaseTest
    {
        [Fact]
        public async Task GetCanadianStatuses_ShouldPass()
        {
            var enResult = await Context.GetCanadianStatuses(Locale.English);
            var frResult = await Context.GetCanadianStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetCanadianStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CanadianStatuses);
            var canadianStatus = await Context.GetCanadianStatus(result.Id, Locale.English);

            canadianStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCanadianStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CanadianStatuses);
            var canadianStatus = await Context.GetCanadianStatus(result.Id, Locale.French);

            canadianStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
