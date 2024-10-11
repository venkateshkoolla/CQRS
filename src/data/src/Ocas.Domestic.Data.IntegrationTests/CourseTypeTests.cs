using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CourseTypeTests : BaseTest
    {
        [Fact]
        public async Task GetCourseTypes_ShouldPass()
        {
            var enResult = await Context.GetCourseTypes(Locale.English);
            var frResult = await Context.GetCourseTypes(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetCourseType_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CourseTypes);
            var courseType = await Context.GetCourseType(result.Id, Locale.English);

            courseType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCourseType_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CourseTypes);
            var courseType = await Context.GetCourseType(result.Id, Locale.French);

            courseType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
