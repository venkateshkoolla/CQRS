using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Data.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class CustomAuditTests : BaseTest
    {
        [Fact]
        public async Task GetCustomAudit_ShouldPass()
        {
            // Arrange & Act
            var customAudit = await Context.GetCustomAudit(TestConstants.CustomAudits.Id, Locale.English);

            // Assert
            customAudit.Should().NotBeNull();
        }

        [Fact]
        public async Task GetCustomAudits_ShouldPass()
        {
            // Arrange & Act
            var customAudits = await Context.GetCustomAudits(
                new GetCustomAuditOptions
                {
                    ApplicantId = TestConstants.CustomAudits.ApplicantId,
                    ApplicationId = TestConstants.CustomAudits.ApplicationId
                },
                Locale.English);

            // Assert
            customAudits.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ApplicantId == TestConstants.CustomAudits.ApplicantId || x.ApplicationId == TestConstants.CustomAudits.ApplicationId)
                .And.OnlyContain(x => (x.CustomAuditType == CustomAuditType.Minimum && !x.Details.Any()) || (x.CustomAuditType == CustomAuditType.Detailed && x.Details.Any()));
        }

        [Fact]
        public async Task GetCustomAudits_ShouldPass_With_DateTime_Filters()
        {
            // Arrange
            var fromDate = DataFakerFixture.Faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(-2));

            // Act
            var customAudits = await Context.GetCustomAudits(
                new GetCustomAuditOptions
                {
                    ApplicantId = TestConstants.CustomAudits.ApplicantId,
                    ApplicationId = TestConstants.CustomAudits.ApplicationId,
                    FromDate = fromDate,
                    ToDate = DateTime.UtcNow
                },
                Locale.English);

            // Assert
            customAudits.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ApplicantId == TestConstants.CustomAudits.ApplicantId || x.ApplicationId == TestConstants.CustomAudits.ApplicationId)
                .And.OnlyContain(x => x.ModifiedOn <= DateTime.Now && x.ModifiedOn >= fromDate);
        }
    }
}
