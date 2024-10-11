using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class FirstLanguageTests : BaseTest
    {
        [Fact]
        public async Task GetFirstLanguages_ShouldPass()
        {
            var enResult = await Context.GetFirstLanguages(Locale.English);
            var frResult = await Context.GetFirstLanguages(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetFirstLanguage_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.FirstLanguages);
            var firstLanguage = await Context.GetFirstLanguage(result.Id, Locale.English);

            firstLanguage.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetFirstLanguage_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.FirstLanguages);
            var firstLanguage = await Context.GetFirstLanguage(result.Id, Locale.French);

            firstLanguage.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
