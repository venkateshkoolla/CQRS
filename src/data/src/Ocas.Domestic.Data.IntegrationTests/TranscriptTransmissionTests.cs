using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class TranscriptTransmissionTests : BaseTest
    {
        [Fact]
        public async Task GetTranscriptTransmissionDates_ShouldPass()
        {
            var enResult = await Context.GetTranscriptTransmissionDates(Locale.English);
            var frResult = await Context.GetTranscriptTransmissionDates(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveSameCount(enResult);
        }

        [Fact]
        public async Task GetTranscriptTransmissionDate_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.TranscriptTransmissions);
            var transcriptRequestStatus = await Context.GetTranscriptTransmissionDate(result.Id, Locale.English);

            transcriptRequestStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetTranscriptTransmissionDate_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.TranscriptTransmissions);
            var transcriptRequestStatus = await Context.GetTranscriptTransmissionDate(result.Id, Locale.French);

            transcriptRequestStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
