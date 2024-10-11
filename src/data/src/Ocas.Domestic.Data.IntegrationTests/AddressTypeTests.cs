using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class AddressTypeTests : BaseTest
    {
        [Fact]
        public async Task GetAddressTypes_ShouldPass()
        {
            var enResult = await Context.GetAddressTypes(Locale.English);
            var frResult = await Context.GetAddressTypes(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetAddressType_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.AddressTypes);
            var addressType = await Context.GetAddressType(result.Id, Locale.English);

            addressType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetAddressType_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.AddressTypes);
            var addressType = await Context.GetAddressType(result.Id, Locale.French);

            addressType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
