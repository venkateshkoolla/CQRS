using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OfficialTests : BaseTest
    {
        [Fact]
        public async Task GetOfficials_ShouldPass()
        {
            var enResult = await Context.GetOfficials(Locale.English);
            var frResult = await Context.GetOfficials(Locale.French);

            enResult.Should().NotBeNullOrEmpty();
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetOfficial_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Officials);
            var official = await Context.GetOfficial(result.Id, Locale.English);

            official.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetOfficial_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Officials);
            var official = await Context.GetOfficial(result.Id, Locale.French);

            official.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
