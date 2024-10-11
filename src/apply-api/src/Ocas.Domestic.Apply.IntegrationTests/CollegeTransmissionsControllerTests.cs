using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Enums;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class CollegeTransmissionsControllerTests : BaseTest<ApplyApiClient>
    {
        public CollegeTransmissionsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
        }

        [Fact]
        [IntegrationTest]
        public async Task GetCollegeTransmissions_ShouldPass()
        {
            // Arrange
            var testUser = await IdentityUserFixture.GetApplicantUser(TestConstants.Identity.Providers.OcasApplicants.ApplicantWithCollegeTransmissions, TestConstants.Identity.Providers.OcasApplicants.ApplicantWithCollegeTransmissionsPw);
            Client.WithAccessToken(testUser.AccessToken);
            var applicant = await Client.GetCurrentApplicant();
            var applications = await Client.GetApplications(applicant.Id);

            var application = applications.FirstOrDefault(a => a.ApplicationNumber == "200953926");
            application.Should().NotBeNull();

            // Act
            var result = await Client.GetCollegeTransmissions(application.Id);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().SatisfyRespectively(
                first => first.Type.Should().Be(CollegeTransmissionType.ProgramChoice),
                second => second.Type.Should().Be(CollegeTransmissionType.Education),
                third => third.Type.Should().Be(CollegeTransmissionType.Offer),
                fourth => fourth.Type.Should().Be(CollegeTransmissionType.ProfileData));
        }
    }
}
