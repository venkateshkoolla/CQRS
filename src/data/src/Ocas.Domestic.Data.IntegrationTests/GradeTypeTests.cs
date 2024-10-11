using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class GradeTypeTests : BaseTest
    {
        [Fact]
        public async Task GetGradeTypes_ShouldPass()
        {
            // Act
            var enResult = await Context.GetGradeTypes(Locale.English);
            var frResult = await Context.GetGradeTypes(Locale.French);

            // Assert
            enResult.Should().NotBeNullOrEmpty();
            frResult.Should().NotBeNullOrEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetGradeType_ShouldPass()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.GradeTypes);

            // Act
            var gradeType = await Context.GetGradeType(result.Id, Locale.English);

            // Assert
            gradeType.Should().NotBeNull();
            gradeType.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetGradeType_ShouldPass_WhenLanguageDiffers()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.GradeTypes);

            // Act
            var gradeType = await Context.GetGradeType(result.Id, Locale.French);

            // Assert
            gradeType.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}