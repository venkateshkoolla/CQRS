using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class TranscriptTests : BaseTest
    {
        [Fact]
        public async Task GetTranscript_ShouldPass()
        {
            var result = await Context.GetTranscript(TestConstants.Transcripts.Id);

            result.Should().BeOfType<Transcript>()
                .And.NotBeNull();
        }

        [Fact]
        public async Task GetTranscripts_ShouldPass_When_ContactId()
        {
            var results = await Context.GetTranscripts(new GetTranscriptOptions { ContactId = TestConstants.Transcripts.ContactId });

            results.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ContactId == TestConstants.Transcripts.ContactId);
        }

        [Fact]
        public async Task GetTranscripts_ShouldPass_When_ContactIdAndPartnerId()
        {
            var options = new GetTranscriptOptions
            {
                ContactId = TestConstants.Transcripts.ContactId,
                PartnerId = TestConstants.Transcripts.PartnerId
            };
            var results = await Context.GetTranscripts(options);

            results.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ContactId == TestConstants.Transcripts.ContactId && x.PartnerId == TestConstants.Transcripts.PartnerId);
        }

        [Fact]
        public async Task GetTranscripts_ShouldPass_When_PartnerId()
        {
            var results = await Context.GetTranscripts(new GetTranscriptOptions { PartnerId = TestConstants.Transcripts.PartnerId });

            results.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.PartnerId == TestConstants.Transcripts.PartnerId);
        }

        [Fact]
        public async Task CreateTranscript_ShouldPass()
        {
            Transcript transcript = null;
            try
            {
                // Arrange
                var transcriptBase = DataFakerFixture.Models.TranscriptBase.Generate("default, OntarioHighSchool");
                transcriptBase.ContactId = TestConstants.Transcripts.ContactId;
                transcriptBase.TranscriptType = Enums.TranscriptType.OntarioHighSchoolTranscript;
                transcriptBase.PartnerId = TestConstants.Transcripts.PartnerId;

                //Act
                transcript = await Context.CreateTranscript(transcriptBase);

                //Assert
                CheckTranscriptFields(transcript, transcriptBase);
            }
            finally
            {
                // Cleanup
                if (transcript?.Id != null)
                {
                    await Context.DeleteTranscript(transcript.Id);
                }
            }
        }

        [Fact]
        public async Task UpdateTranscript_ShouldPass()
        {
            Transcript transcript = null;
            try
            {
                // Arrange
                var transcriptBase = DataFakerFixture.Models.TranscriptBase.Generate("default, OntarioHighSchool");
                transcriptBase.ContactId = TestConstants.Transcripts.ContactId;
                transcriptBase.TranscriptType = Enums.TranscriptType.OntarioHighSchoolTranscript;
                transcriptBase.PartnerId = TestConstants.Transcripts.PartnerId;

                transcript = await Context.CreateTranscript(transcriptBase);

                //Act
                var transcriptUpdates = DataFakerFixture.Models.Transcript.Generate("default, OntarioHighSchool");
                transcriptUpdates.ContactId = TestConstants.Transcripts.ContactId;
                transcriptUpdates.Id = transcript.Id;
                transcriptUpdates.PartnerId = TestConstants.Transcripts.PartnerId;
                var transcriptUpdated = await Context.UpdateTranscript(transcriptUpdates);

                //Assert
                CheckTranscriptFields(transcriptUpdated, transcriptUpdates);
                transcriptUpdated.Id.Should().Be(transcript.Id);
            }
            finally
            {
                // Cleanup
                if (transcript?.Id != null)
                {
                    await Context.DeleteTranscript(transcript.Id);
                }
            }
        }

        [Fact]
        public async Task DeleteTranscript_ShouldPass()
        {
            // Arrange
            Transcript transcript = null;
            var transcriptBase = DataFakerFixture.Models.TranscriptBase.Generate("default, OntarioHighSchool");
            transcriptBase.ContactId = TestConstants.Transcripts.ContactId;
            transcriptBase.TranscriptType = Enums.TranscriptType.OntarioHighSchoolTranscript;
            transcriptBase.PartnerId = TestConstants.Transcripts.PartnerId;

            transcript = await Context.CreateTranscript(transcriptBase);

            //Act
            await Context.DeleteTranscript(transcript.Id);

            //Assert
            var transcriptDelete = await Context.GetTranscript(transcript.Id);
            transcriptDelete.Should().BeNull();
        }

        private void CheckTranscriptFields(Transcript transcript, TranscriptBase transcriptBase)
        {
            transcript.Id.Should().NotBeEmpty();
            transcript.OriginalStudentId.Should().Be(transcriptBase.OriginalStudentId);
            transcript.TranscriptType.Should().Be(transcriptBase.TranscriptType);
            transcript.Name.Should().Be(transcriptBase.Name);
            transcript.PartnerId.Should().Be(transcriptBase.PartnerId);
            transcript.ContactId.Should().Be(transcriptBase.ContactId);
            transcript.Credentials.Should().Be(transcriptBase.Credentials);
            transcript.EtmsTranscriptId.Should().Be(transcriptBase.EtmsTranscriptId);
            transcript.SupportingDocumentId.Should().Be(transcriptBase.SupportingDocumentId);
            transcript.TranscriptSourceId.Should().Be(transcriptBase.TranscriptSourceId);
            transcript.ModifiedBy.Should().Be(transcriptBase.ModifiedBy);
        }
    }
}
