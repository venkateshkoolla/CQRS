using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OfferTypeTests : BaseTest
    {
        [Fact]
        public async Task GetOfferTypes_ShouldPass()
        {
            var enResult = await Context.GetOfferTypes(Locale.English);
            var frResult = await Context.GetOfferTypes(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetOfferType_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.OfferTypes);
            var offerType = await Context.GetOfferType(result.Id, Locale.English);

            offerType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetOfferType_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.OfferTypes);
            var offerType = await Context.GetOfferType(result.Id, Locale.French);

            offerType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
