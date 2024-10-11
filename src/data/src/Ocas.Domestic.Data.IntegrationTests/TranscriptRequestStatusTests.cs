using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class TranscriptRequestStatusTests : BaseTest
    {
        [Fact]
        public async Task GetTranscriptRequestStatuses_ShouldPass()
        {
            var enResult = await Context.GetTranscriptRequestStatuses(Locale.English);
            var frResult = await Context.GetTranscriptRequestStatuses(Locale.French);

            enResult.Should().HaveCountGreaterThan(0);
            frResult.Should().HaveCount(enResult.Count);
        }

        [Fact]
        public async Task GetTranscriptRequestStatus_ShouldPass()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.TranscriptRequestStatuses);
            var transcriptRequestStatus = await Context.GetTranscriptRequestStatus(result.Id, Locale.English);

            transcriptRequestStatus.Should().BeEquivalentTo(result);
        }

        [Fact]
        public async Task GetTranscriptRequestStatus_ShouldPass_WhenLanguageDiffers()
        {
            var result = DataFakerFixture.Faker.PickRandom(DataFakerFixture.SeedData.TranscriptRequestStatuses);
            var transcriptRequestStatus = await Context.GetTranscriptRequestStatus(result.Id, Locale.French);

            transcriptRequestStatus.Should().BeEquivalentTo(result, opts => opts.Excluding(p => p.LocalizedName));
        }
    }
}
