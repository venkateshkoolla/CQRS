using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class PromotionTests : BaseTest
    {
        [Fact]
        public async Task GetPromotions_ShouldPass()
        {
            var enResult = await Context.GetPromotions(Locale.English);
            var frResult = await Context.GetPromotions(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetPromotion_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Promotions);

            var promotion = await Context.GetPromotion(result.Id, Locale.English);

            promotion.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetPromotion_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Promotions);

            var promotion = await Context.GetPromotion(result.Id, Locale.French);

            promotion.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
