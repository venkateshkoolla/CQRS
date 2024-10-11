using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CourseStatusTests : BaseTest
    {
        [Fact]
        public async Task GetCourseStatuses_ShouldPass()
        {
            var enResult = await Context.GetCourseStatuses(Locale.English);
            var frResult = await Context.GetCourseStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetCourseStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CourseStatuses);
            var courseStatus = await Context.GetCourseStatus(result.Id, Locale.English);

            courseStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCourseStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CourseStatuses);
            var courseStatus = await Context.GetCourseStatus(result.Id, Locale.French);

            courseStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
