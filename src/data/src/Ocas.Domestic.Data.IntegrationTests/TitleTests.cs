using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class TitleTests : BaseTest
    {
        [Fact]
        public async Task GetTitles_ShouldPass()
        {
            var enResult = await Context.GetTitles(Locale.English);
            var frResult = await Context.GetTitles(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetTitle_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Titles);
            var testType = await Context.GetTitle(result.Id, Locale.English);

            testType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetTitle_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.Titles);
            var testType = await Context.GetTitle(result.Id, Locale.French);

            testType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
