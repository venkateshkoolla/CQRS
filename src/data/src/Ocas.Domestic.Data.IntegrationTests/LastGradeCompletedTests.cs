using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class LastGradeCompletedTests : BaseTest
    {
        [Fact]
        public async Task GetLastGradeCompleteds_ShouldPass()
        {
            var enResult = await Context.GetLastGradeCompleteds(Locale.English);
            var frResult = await Context.GetLastGradeCompleteds(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetLastGradeCompleted_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.LastGradeCompleteds);
            var lastGradeCompleted = await Context.GetLastGradeCompleted(result.Id, Locale.English);

            lastGradeCompleted.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetLanguage_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.LastGradeCompleteds);
            var lastGradeCompleted = await Context.GetLastGradeCompleted(result.Id, Locale.French);

            lastGradeCompleted.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
