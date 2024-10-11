using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class ApplicantBriefTests : BaseTest
    {
        [Fact]
        public async Task GetApplicantBriefs_ShouldPass()
        {
            // Arrange
            var options = new GetApplicantBriefOptions
            {
                LastName = "Test"
            };

            // Act
            var applicantBriefs = await Context.GetApplicantBriefs(options, UserType.OcasUser, string.Empty);

            // Assert
            applicantBriefs.Items.Count.Should().Be(100);
            applicantBriefs.TotalCount.Should().BePositive();
        }

        [Fact]
        public async Task GetApplicantBriefs_ShouldPass_When_Paging()
        {
            // Arrange
            var options = new GetApplicantBriefOptions
            {
                OntarioEducationNumber = "000000000",
                PageNumber = 1,
                PageSize = 25
            };

            // Act
            var applicantBriefs = await Context.GetApplicantBriefs(options, UserType.OcasUser, string.Empty);

            // Assert
            applicantBriefs.Items.Should().NotBeEmpty();
            applicantBriefs.Items.Count.Should().Be(options.PageSize);
            applicantBriefs.TotalCount.Should().BePositive();
        }
    }
}
