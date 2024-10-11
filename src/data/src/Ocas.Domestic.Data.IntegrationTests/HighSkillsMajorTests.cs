using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class HighSkillsMajorTests : BaseTest
    {
        [Fact]
        public async Task GetHighSkillsMajors_ShouldPass()
        {
            // Act
            var enResult = await Context.GetHighSkillsMajors(Locale.English);
            var frResult = await Context.GetHighSkillsMajors(Locale.French);

            // Assert
            enResult.Should().NotBeNullOrEmpty();
            frResult.Should().NotBeNullOrEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetHighSkillsMajor_ShouldPass()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.HighSkillsMajors);

            // Act
            var highSkillsMajor = await Context.GetHighSkillsMajor(result.Id, Locale.English);

            // Assert
            highSkillsMajor.Should().NotBeNull();
            highSkillsMajor.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetHighSkillsMajor_ShouldPass_WhenLanguageDiffers()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.HighSkillsMajors);

            // Act
            var highSkillsMajor = await Context.GetHighSkillsMajor(result.Id, Locale.French);

            // Assert
            highSkillsMajor.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}