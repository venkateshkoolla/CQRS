using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CustomAuditDetailTests : BaseTest
    {
        [Fact]
        public async Task GetCustomAuditDetail_ShouldPass()
        {
            // Arrange & Act
            var customAuditDetail = await Context.GetCustomAuditDetail(TestConstants.CustomAuditDetails.Id);

            // Assert
            customAuditDetail.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCustomAuditDetails_ShouldPass_When_AuditId()
        {
            // Arrange & Act
            var auditId = TestConstants.CustomAudits.Id;
            var customAuditDetails = await Context.GetCustomAuditDetails(auditId);

            // Assert
            customAuditDetails.Should().NotBeNull();
            customAuditDetails.Should().OnlyContain(x => x.CustomAuditId == auditId);
        }
    }
}
