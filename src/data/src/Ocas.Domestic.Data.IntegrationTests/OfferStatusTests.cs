using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OfferStatusTests : BaseTest
    {
        [Fact]
        public async Task GetOfferStatuses_ShouldPass()
        {
            var enResult = await Context.GetOfferStatuses(Locale.English);
            var frResult = await Context.GetOfferStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetOfferStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.OfferStatuses);
            var offerStatus = await Context.GetOfferStatus(result.Id, Locale.English);

            offerStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetOfferStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.OfferStatuses);
            var offerStatus = await Context.GetOfferStatus(result.Id, Locale.French);

            offerStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
