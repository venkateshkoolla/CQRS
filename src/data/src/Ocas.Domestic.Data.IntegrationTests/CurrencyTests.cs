using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CurrencyTests : BaseTest
    {
        [Fact]
        public async Task GetCurrencies_ShouldPass()
        {
            var result = await Context.GetCurrencies();
            result.Should().HaveCountGreaterOrEqualTo(DataFakerFixture.SeedData.Currencies.Count());
        }

        [Fact]
        public async Task GetCurrency_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Currencies);
            var currency = await Context.GetCurrency(result.Id);

            currency.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCurrency_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Currencies);
            var currency = await Context.GetCurrency(result.Id);

            currency.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
