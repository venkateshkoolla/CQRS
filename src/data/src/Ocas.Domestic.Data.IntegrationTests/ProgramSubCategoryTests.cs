using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ProgramSubCategoryTests : BaseTest
    {
        [Fact]
        public async Task GetProgramSubCategories_ShouldPass()
        {
            var enResult = await Context.GetProgramSubCategories(Locale.English);
            var frResult = await Context.GetProgramSubCategories(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetProgramSubCategoriesByProgramCategory_ShouldPass()
        {
            //Get a program category
            var programCategoryId = DataFakerFixture.Models.GenerateProgramCategoryWithSubCategory();

            var enResult = await Context.GetProgramSubCategories(programCategoryId, Locale.English);
            enResult.Should().HaveCountGreaterOrEqualTo(1);
            enResult.Should().OnlyContain(o => o.ProgramCategoryId == programCategoryId);

            var frResult = await Context.GetProgramSubCategories(programCategoryId, Locale.French);
            frResult.Should().HaveCountGreaterOrEqualTo(1);
            enResult.Should().OnlyContain(o => o.ProgramCategoryId == programCategoryId);
        }

        [Fact]
        public async Task GetProgramSubCategory_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramSubCategories);

            var programSubCategory = await Context.GetProgramSubCategory(result.Id, Locale.English);

            programSubCategory.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetProgramSubCategory_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ProgramSubCategories);

            var programSubCategory = await Context.GetProgramSubCategory(result.Id, Locale.French);

            programSubCategory.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
