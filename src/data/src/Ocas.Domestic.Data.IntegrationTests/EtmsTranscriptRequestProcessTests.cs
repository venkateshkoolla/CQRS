using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class EtmsTranscriptRequestProcessTests : BaseTest
    {
        [Fact]
        public async Task GetEtmsTranscriptRequestProcess_ShouldPass()
        {
            // Arrange & Act
            var etmsTranscriptRequestProcess = await Context.GetEtmsTranscriptRequestProcess(TestConstants.EtmsTranscriptRequestProcess.TestEtmsTranscriptRequestProcess);

            // Assert
            etmsTranscriptRequestProcess.Should().NotBeNull();
        }

        [Fact]
        public async Task GetEtmsTranscriptRequestProcesses_ShouldPass()
        {
            // Arrange & Act
            var etmsTranscriptRequestProcesses = await Context.GetEtmsTranscriptRequestProcesses(TestConstants.EtmsTranscriptRequest.TestEtmsTranscriptRequest);

            // Assert
            etmsTranscriptRequestProcesses.Should().NotBeNullOrEmpty();
            etmsTranscriptRequestProcesses.Should().OnlyContain(x => x.EtmsTranscriptRequestid == TestConstants.EtmsTranscriptRequest.TestEtmsTranscriptRequest);
        }

        [Fact]
        public async Task UpdateEtmsTranscriptRequestProcess_ShouldPass()
        {
            // Arrange
            var etmsTranscriptRequestProcess = await Context.GetEtmsTranscriptRequestProcess(TestConstants.EtmsTranscriptRequestProcess.TestEtmsTranscriptRequestProcess);
            var modifiedBy = DataFakerFixture.Faker.Person.LastName;

            // Act
            etmsTranscriptRequestProcess.ModifiedBy = modifiedBy;
            var result = await Context.UpdateEtmsTranscriptRequestProcess(etmsTranscriptRequestProcess);

            // Assert
            result.Should().NotBeNull();
            result.ModifiedBy.Should().Be(modifiedBy);
        }
    }
}
