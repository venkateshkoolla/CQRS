using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramCategoryTests : BaseTest
    {
        [Fact]
        public async Task GetProgramCategories_ShouldPass()
        {
            var enResult = await Context.GetProgramCategories(Locale.English);
            var frResult = await Context.GetProgramCategories(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetProgramCategory_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramCategories);

            var programCategory = await Context.GetProgramCategory(result.Id, Locale.English);

            programCategory.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetProgramCategory_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramCategories);

            var programCategory = await Context.GetProgramCategory(result.Id, Locale.French);

            programCategory.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
