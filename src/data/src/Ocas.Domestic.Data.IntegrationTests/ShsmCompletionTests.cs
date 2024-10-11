using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ShsmCompletionTests : BaseTest
    {
        [Fact]
        public async Task GetShsmCompletions_ShouldPass()
        {
            // Act
            var enResult = await Context.GetShsmCompletions();

            // Assert
            enResult.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetShsmCompletion_ShouldPass()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.ShsmCompletions);

            // Act
            var shsmCompletion = await Context.GetShsmCompletion(result.Id);

            // Assert
            shsmCompletion.Should().NotBeNull();
            shsmCompletion.Should().BeEquivalentTo(result);
        }
    }
}