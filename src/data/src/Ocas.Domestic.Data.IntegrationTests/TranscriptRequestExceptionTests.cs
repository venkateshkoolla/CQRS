using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class TranscriptRequestExceptionTests : BaseTest
    {
        [Fact]
        public async Task GetTranscriptRequestExceptions_ShouldPass()
        {
            var enResult = await Context.GetTranscriptRequestExceptions(Locale.English);
            var frResult = await Context.GetTranscriptRequestExceptions(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetTranscriptRequestException_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.TranscriptRequestExceptions);
            var transcriptRequestException = await Context.GetTranscriptRequestException(result.Id, Locale.English);

            transcriptRequestException.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetTranscriptRequestException_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.TranscriptRequestExceptions);
            var transcriptRequestException = await Context.GetTranscriptRequestException(result.Id, Locale.French);

            transcriptRequestException.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
