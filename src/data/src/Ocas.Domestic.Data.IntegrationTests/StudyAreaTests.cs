using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class StudyAreaTests : BaseTest
    {
        [Fact]
        public async Task GetStudyAreas_ShouldPass()
        {
            var enResult = await Context.GetStudyAreas(Locale.English);
            var frResult = await Context.GetStudyAreas(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetStudyArea_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.StudyAreas);

            var studyArea = await Context.GetStudyArea(result.Id, Locale.English);

            studyArea.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetStudyArea_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.StudyAreas);

            var studyArea = await Context.GetStudyArea(result.Id, Locale.French);

            studyArea.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
