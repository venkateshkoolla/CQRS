using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class SourceTests : BaseTest
    {
        [Fact]
        public async Task GetSources_ShouldPass()
        {
            var enResult = await Context.GetSources(Locale.English);
            var frResult = await Context.GetSources(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetSource_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Sources);
            var source = await Context.GetSource(result.Id, Locale.English);

            source.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetSource_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Sources);
            var source = await Context.GetSource(result.Id, Locale.French);

            source.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
