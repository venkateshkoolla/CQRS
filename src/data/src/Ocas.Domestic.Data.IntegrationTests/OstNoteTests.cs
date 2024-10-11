using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class OstNoteTests : BaseTest
    {
        [Fact]
        public async Task GetOstNotes_ShouldPass()
        {
            // Act
            var enResult = await Context.GetOstNotes(Locale.English);
            var frResult = await Context.GetOstNotes(Locale.French);

            // Assert
            enResult.Should().NotBeNullOrEmpty();
            frResult.Should().NotBeNullOrEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetOstNote_ShouldPass()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.OstNotes);

            // Act
            var ostNote = await Context.GetOstNote(result.Id, Locale.English);

            // Assert
            ostNote.Should().NotBeNull();
            ostNote.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetOstNote_ShouldPass_WhenLanguageDiffers()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.OstNotes);

            // Act
            var ostNote = await Context.GetOstNote(result.Id, Locale.French);

            // Assert
            ostNote.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}