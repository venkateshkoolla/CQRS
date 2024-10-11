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
    public class EtmsTranscriptRequestTests : BaseTest
    {
        [Fact]
        public async Task GetEtmsTranscriptRequest_ShouldPass()
        {
            // Arrange & Act
            var etmsTranscriptRequest = await Context.GetEtmsTranscriptRequest(TestConstants.EtmsTranscriptRequest.TestEtmsTranscriptRequest);

            // Assert
            etmsTranscriptRequest.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateEtmsTranscriptRequest_ShouldPass()
        {
            // Arrange
            var etmsTranscriptRequest = await Context.GetEtmsTranscriptRequest(TestConstants.EtmsTranscriptRequest.TestEtmsTranscriptRequest);
            var modifiedBy = DataFakerFixture.Faker.Person.LastName;

            // Act
            etmsTranscriptRequest.ModifiedBy = modifiedBy;
            var result = await Context.UpdateEtmsTranscriptRequest(etmsTranscriptRequest);

            // Assert
            result.Should().NotBeNull();
            result.ModifiedBy.Should().Be(modifiedBy);
        }
    }
}
