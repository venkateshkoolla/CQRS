using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class HighestEducationTests : BaseTest
    {
        [Fact]
        public async Task GetHighestEducations_ShouldPass()
        {
            // Act
            var enResult = await Context.GetHighestEducations(Locale.English);
            var frResult = await Context.GetHighestEducations(Locale.French);

            // Assert
            enResult.Should().NotBeNullOrEmpty();
            frResult.Should().NotBeNullOrEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetHighestEducation_ShouldPass()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.HighestEducations);

            // Act
            var highestEducation = await Context.GetHighestEducation(result.Id, Locale.English);

            // Assert
            highestEducation.Should().NotBeNull();
            highestEducation.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetHighestEducation_ShouldPass_WhenLanguageDiffers()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.HighestEducations);

            // Act
            var highestEducation = await Context.GetHighestEducation(result.Id, Locale.French);

            // Assert
            highestEducation.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}