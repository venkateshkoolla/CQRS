using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class PreferredLanguageTests : BaseTest
    {
        [Fact]
        public async Task GetPreferredLanguages_ShouldPass()
        {
            var enResult = await Context.GetPreferredLanguages(Locale.English);
            var frResult = await Context.GetPreferredLanguages(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetPreferredLanguage_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PreferredLanguages);
            var preferredLanguage = await Context.GetPreferredLanguage(result.Id, Locale.English);

            preferredLanguage.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetPreferredLanguage_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.PreferredLanguages);
            var preferredLanguage = await Context.GetPreferredLanguage(result.Id, Locale.French);

            preferredLanguage.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
