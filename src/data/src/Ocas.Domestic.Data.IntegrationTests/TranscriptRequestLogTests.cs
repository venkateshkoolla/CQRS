using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class TranscriptRequestLogTests : BaseTest
    {
        [Fact]
        public async Task CreateTranscriptRequestLog_ShouldPass()
        {
            TranscriptRequestLog transcriptRequestLog = null;
            try
            {
                // Arrange
                var transcriptRequestLogBase = DataFakerFixture.Models.TranscriptRequestLogBase.Generate();

                //Act
                transcriptRequestLog = await Context.CreateTranscriptRequestLog(transcriptRequestLogBase);

                //Assert
                transcriptRequestLog.Should().BeEquivalentTo(transcriptRequestLogBase, opts => opts
                    .Excluding(t => t.CreatedOn)
                    .Excluding(t => t.ModifiedOn));
                transcriptRequestLog.Id.Should().NotBeEmpty();
            }
            finally
            {
                // Cleanup
                if (transcriptRequestLog?.Id != null)
                    await Context.DeleteTranscriptRequestLog(transcriptRequestLog.Id);
            }
        }

        [Fact]
        public async Task DeleteTranscriptRequestLog_ShouldPass_WhenId()
        {
            TranscriptRequestLog transcriptRequestLog = null;
            try
            {
                // Arrange
                var transcriptRequestLogBase = DataFakerFixture.Models.TranscriptRequestLogBase.Generate();
                transcriptRequestLog = await Context.CreateTranscriptRequestLog(transcriptRequestLogBase);

                //Act
                await Context.DeleteTranscriptRequestLog(transcriptRequestLog.Id);

                //Assert
                var transcriptRequestDelete = await Context.GetTranscriptRequestLog(transcriptRequestLog.Id);
                transcriptRequestDelete.Should().BeNull();
            }
            finally
            {
                // Cleanup
                if (transcriptRequestLog?.Id != null)
                    await Context.DeleteTranscriptRequestLog(transcriptRequestLog.Id);
            }
        }

        [Fact]
        public async Task DeleteTranscriptRequestLog_ShouldPass_WhenObject()
        {
            TranscriptRequestLog transcriptRequestLog = null;
            try
            {
                // Arrange
                var transcriptRequestLogBase = DataFakerFixture.Models.TranscriptRequestLogBase.Generate();
                transcriptRequestLog = await Context.CreateTranscriptRequestLog(transcriptRequestLogBase);

                //Act
                await Context.DeleteTranscriptRequestLog(transcriptRequestLog);

                //Assert
                var transcriptRequestDelete = await Context.GetTranscriptRequestLog(transcriptRequestLog.Id);
                transcriptRequestDelete.Should().BeNull();
            }
            finally
            {
                // Cleanup
                if (transcriptRequestLog?.Id != null)
                    await Context.DeleteTranscriptRequestLog(transcriptRequestLog.Id);
            }
        }

        [Fact]
        public async Task GetTranscriptRequestLog_ShouldPass()
        {
            TranscriptRequestLog transcriptRequestLog = null;
            try
            {
                // Arrange
                var transcriptRequestLogBase = DataFakerFixture.Models.TranscriptRequestLogBase.Generate();

                //Act
                transcriptRequestLog = await Context.CreateTranscriptRequestLog(transcriptRequestLogBase);
                var transcriptRequestLogRetrieved = await Context.GetTranscriptRequestLog(transcriptRequestLog.Id);

                //Assert
                transcriptRequestLogRetrieved.Should().BeEquivalentTo(transcriptRequestLog);
            }
            finally
            {
                // Cleanup
                if (transcriptRequestLog?.Id != null)
                    await Context.DeleteTranscriptRequestLog(transcriptRequestLog.Id);
            }
        }

        [Fact]
        public async Task UpdateTranscriptRequestLog_ShouldPass()
        {
            TranscriptRequestLog transcriptRequestLog = null;
            try
            {
                // Arrange
                var transcriptRequestLogBase = DataFakerFixture.Models.TranscriptRequestLogBase.Generate();
                transcriptRequestLog = await Context.CreateTranscriptRequestLog(transcriptRequestLogBase);

                //Act
                var transcriptRequestLogUpdates = DataFakerFixture.Models.TranscriptRequestLog.Generate();
                transcriptRequestLogUpdates.Id = transcriptRequestLog.Id;
                var transcriptRequestLogUpdated = await Context.UpdateTranscriptRequestLog(transcriptRequestLogUpdates);

                //Assert
                transcriptRequestLogUpdated.Should().BeEquivalentTo(transcriptRequestLog, opts => opts
                    .Excluding(t => t.ModifiedOn));
                transcriptRequestLogUpdated.ModifiedOn.Value.Should().BeCloseTo(DateTime.UtcNow, TestConstants.Config.CommandTimeout.Seconds());
            }
            finally
            {
                // Cleanup
                if (transcriptRequestLog?.Id != null)
                    await Context.DeleteTranscriptRequestLog(transcriptRequestLog);
            }
        }
    }
}
