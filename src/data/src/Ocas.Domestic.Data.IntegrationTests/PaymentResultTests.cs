using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class PaymentResultTests : BaseTest
    {
        [Fact]
        public async Task GetPaymentResults_ShouldPass()
        {
            var enResult = await Context.GetPaymentResults();
            enResult.Should().HaveCountGreaterOrEqualTo(DataFakerFixture.SeedData.PaymentResults.Count());
        }

        [Fact]
        public async Task GetPaymentResult_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PaymentResults);
            var paymentResult = await Context.GetPaymentResult(result.Id);

            paymentResult.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetPaymentResult_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PaymentResults);
            var paymentResult = await Context.GetPaymentResult(result.Id);

            paymentResult.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
