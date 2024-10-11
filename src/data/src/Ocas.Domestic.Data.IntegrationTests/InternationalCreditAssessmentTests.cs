using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    public class InternationalCreditAssessmentTests : BaseTest
    {
        [Fact]
        public async Task GetInternationalCreditAssessments_ShouldPass()
        {
            // Act
            var intlCredAssessments = await Context.GetInternationalCreditAssessments(TestConstants.InternationalCreditAssessment.ApplicantId);

            // Assert
            intlCredAssessments.Should().HaveCountGreaterThan(0);
            intlCredAssessments.Should().OnlyContain(x => x.ApplicantId == TestConstants.InternationalCreditAssessment.ApplicantId);
        }

        [Fact]
        public async Task GetInternationalCreditAssessment_ShouldPass()
        {
            // Act
            var intlCredAssessment = await Context.GetInternationalCreditAssessment(TestConstants.InternationalCreditAssessment.Id);

            // Assert
            intlCredAssessment.Should().NotBeNull();
        }
    }
}
