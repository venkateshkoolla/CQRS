using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CourseLanguageTests : BaseTest
    {
        [Fact]
        public async Task GetCourseLanguages_ShouldPass()
        {
            var enResult = await Context.GetCourseLanguages(Locale.English);
            var frResult = await Context.GetCourseLanguages(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetCourseLanguage_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CourseLanguages);
            var courseLanguage = await Context.GetCourseLanguage(result.Id, Locale.English);

            courseLanguage.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCourseLanguage_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CourseLanguages);
            var courseLanguage = await Context.GetCourseLanguage(result.Id, Locale.French);

            courseLanguage.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
