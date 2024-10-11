using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class LiteracyTestTests : BaseTest
    {
        [Fact]
        public async Task GetLiteracyTests_ShouldPass()
        {
            // Act
            var enResult = await Context.GetLiteracyTests(Locale.English);
            var frResult = await Context.GetLiteracyTests(Locale.French);

            // Assert
            enResult.Should().NotBeNullOrEmpty();
            frResult.Should().NotBeNullOrEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetLiteracyTest_ShouldPass()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.LiteracyTests);

            // Act
            var literacyTest = await Context.GetLiteracyTest(result.Id, Locale.English);

            // Assert
            literacyTest.Should().NotBeNull();
            literacyTest.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetLiteracyTest_ShouldPass_WhenLanguageDiffers()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.LiteracyTests);

            // Act
            var literacyTest = await Context.GetLiteracyTest(result.Id, Locale.French);

            // Assert
            literacyTest.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}