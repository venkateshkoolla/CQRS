using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class StatusOfVisaTests : BaseTest
    {
        [Fact]
        public async Task GetStatusOfVisas_ShouldPass()
        {
            var enResult = await Context.GetStatusOfVisas(Locale.English);
            var frResult = await Context.GetStatusOfVisas(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetStatusOfVisa_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.StatusOfVisas);
            var statusOfVisa = await Context.GetStatusOfVisa(result.Id, Locale.English);

            statusOfVisa.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetStatusOfVisa_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.StatusOfVisas);
            var statusOfVisa = await Context.GetStatusOfVisa(result.Id, Locale.French);

            statusOfVisa.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
