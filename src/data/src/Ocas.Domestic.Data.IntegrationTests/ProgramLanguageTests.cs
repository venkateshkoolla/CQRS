using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramLanguageTests : BaseTest
    {
        [Fact]
        public async Task GetProgramLanguages_ShouldPass()
        {
            var enResult = await Context.GetProgramLanguages(Locale.English);
            var frResult = await Context.GetProgramLanguages(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetProgramLanguage_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramLanguages);
            var programLanguage = await Context.GetProgramLanguage(result.Id, Locale.English);

            programLanguage.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetProgramIntakeStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramLanguages);
            var programLanguage = await Context.GetProgramLanguage(result.Id, Locale.French);

            programLanguage.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
