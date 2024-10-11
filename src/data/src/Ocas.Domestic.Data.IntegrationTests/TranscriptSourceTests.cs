using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class TranscriptSourceTests : BaseTest
    {
        [Fact]
        public async Task GetTranscriptSources_ShouldPass()
        {
            // Act
            var enResult = await Context.GetTranscriptSources(Locale.English);
            var frResult = await Context.GetTranscriptSources(Locale.French);

            // Assert
            enResult.Should().NotBeNullOrEmpty();
            frResult.Should().NotBeNullOrEmpty();
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetTranscriptSource_ShouldPass()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.TranscriptSources);

            // Act
            var transcriptSource = await Context.GetTranscriptSource(result.Id, Locale.English);

            // Assert
            transcriptSource.Should().NotBeNull();
            transcriptSource.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetTranscriptSource_ShouldPass_WhenLanguageDiffers()
        {
            // Arrange
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.TranscriptSources);

            // Act
            var transcriptSource = await Context.GetTranscriptSource(result.Id, Locale.French);

            // Assert
            transcriptSource.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}