using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CourseDeliveryTests : BaseTest
    {
        [Fact]
        public async Task GetCourseDeliveries_ShouldPass()
        {
            var enResult = await Context.GetCourseDeliveries(Locale.English);
            var frResult = await Context.GetCourseDeliveries(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetCourseDelivery_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CourseDeliveries);
            var courseDelivery = await Context.GetCourseDelivery(result.Id, Locale.English);

            courseDelivery.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCourseDelivery_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CourseDeliveries);
            var courseDelivery = await Context.GetCourseDelivery(result.Id, Locale.French);

            courseDelivery.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
