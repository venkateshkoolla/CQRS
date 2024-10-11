using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class AccountStatusTests : BaseTest
    {
        [Fact]
        public async Task GetAccountStatuses_ShouldPass()
        {
            var enResult = await Context.GetAccountStatuses(Locale.English);
            var frResult = await Context.GetAccountStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetAccountStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.AccountStatuses);
            var accountStatus = await Context.GetAccountStatus(result.Id, Locale.English);

            accountStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetAccountStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.AccountStatuses);
            var accountStatus = await Context.GetAccountStatus(result.Id, Locale.French);

            accountStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
