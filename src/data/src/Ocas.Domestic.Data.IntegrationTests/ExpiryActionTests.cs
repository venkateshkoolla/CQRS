using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ExpiryActionTests : BaseTest
    {
        [Fact]
        public async Task GetExpiryActions_ShouldPass()
        {
            var enResult = await Context.GetExpiryActions(Locale.English);
            var frResult = await Context.GetExpiryActions(Locale.French);

            enResult.Should().NotBeNullOrEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetExpiryAction_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ExpiryActions);
            var expiryAction = await Context.GetExpiryAction(result.Id, Locale.English);

            expiryAction.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetLanguage_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ExpiryActions);
            var frResult = await Context.GetExpiryAction(result.Id, Locale.French);

            frResult.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
