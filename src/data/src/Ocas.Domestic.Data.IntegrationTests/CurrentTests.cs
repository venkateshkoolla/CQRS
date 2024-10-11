using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CurrentTests : BaseTest
    {
        [Fact]
        public async Task GetCurrents_ShouldPass()
        {
            var enResult = await Context.GetCurrents(Locale.English);
            var frResult = await Context.GetCurrents(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetCurrent_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Currents);
            var current = await Context.GetCurrent(result.Id, Locale.English);

            current.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCurrent_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Currents);
            var current = await Context.GetCurrent(result.Id, Locale.French);

            current.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
