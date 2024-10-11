using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class DocumentDistributionTests : BaseTest
    {
        [Fact]
        public async Task GetDocumentDistribution_ShouldPass()
        {
            // Arrange & Act
            var documentDistributions = await Context.GetDocumentDistributions(TestConstants.DocumentDistributions.ApplicationId);

            // Assert
            documentDistributions.Should().NotBeNullOrEmpty();
        }
    }
}
