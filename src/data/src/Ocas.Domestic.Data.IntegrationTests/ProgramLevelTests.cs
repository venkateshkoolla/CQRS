using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramLevelTests : BaseTest
    {
        [Fact]
        public async Task GetProgramLevels_ShouldPass()
        {
            var enResult = await Context.GetProgramLevels(Locale.English);
            var frResult = await Context.GetProgramLevels(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetProgramLevel_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramLevels);

            var programLevel = await Context.GetProgramLevel(result.Id, Locale.English);

            programLevel.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetProgramLevel_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramLevels);

            var programLevel = await Context.GetProgramLevel(result.Id, Locale.French);

            programLevel.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
