using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CommunityInvolvementTests : BaseTest
    {
        [Fact]
        public async Task GetCommunityInvolvements_ShouldPass()
        {
            // Act
            var enResult = await Context.GetCommunityInvolvements(Locale.English);
            var frResult = await Context.GetCommunityInvolvements(Locale.French);

            // Assert
            enResult.Should().NotBeNullOrEmpty();
            frResult.Should().NotBeNullOrEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetCommunityInvolvement_ShouldPass()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CommunityInvolvements);

            // Act
            var communityInvolvement = await Context.GetCommunityInvolvement(result.Id, Locale.English);

            // Assert
            communityInvolvement.Should().NotBeNull();
            communityInvolvement.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetCommunityInvolvement_ShouldPass_WhenLanguageDiffers()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.CommunityInvolvements);

            // Act
            var communityInvolvement = await Context.GetCommunityInvolvement(result.Id, Locale.French);

            // Assert
            communityInvolvement.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}