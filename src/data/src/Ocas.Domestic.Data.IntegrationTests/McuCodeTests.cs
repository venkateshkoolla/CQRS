using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class McuCodeTests : BaseTest
    {
        [Fact]
        public async Task GetMcuCodes_ShouldPass()
        {
            var enResult = await Context.GetMcuCodes(Locale.English);
            var frResult = await Context.GetMcuCodes(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetMcuCode_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.McuCodes);

            var mcuCode = await Context.GetMcuCode(result.Id, Locale.English);

            mcuCode.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetMcuCode_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.McuCodes);

            var mcuCode = await Context.GetMcuCode(result.Id, Locale.French);

            mcuCode.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
