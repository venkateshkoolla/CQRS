using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class PaymentMethodTests : BaseTest
    {
        [Fact]
        public async Task GetPaymentMethods_ShouldPass()
        {
            var enResult = await Context.GetPaymentMethods(Locale.English);
            var frResult = await Context.GetPaymentMethods(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetPaymentMethod_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PaymentMethods);
            var offerType = await Context.GetPaymentMethod(result.Id, Locale.English);

            offerType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetPaymentMethod_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PaymentMethods);
            var offerType = await Context.GetPaymentMethod(result.Id, Locale.French);

            offerType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
