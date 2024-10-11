using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OfferStateTests : BaseTest
    {
        [Fact]
        public async Task GetOfferStates_ShouldPass()
        {
            var enResult = await Context.GetOfferStates(Locale.English);
            var frResult = await Context.GetOfferStates(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetOfferState_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.OfferStates);
            var offerState = await Context.GetOfferState(result.Id, Locale.English);

            offerState.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetOfferState_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.OfferStates);
            var offerState = await Context.GetOfferState(result.Id, Locale.French);

            offerState.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
