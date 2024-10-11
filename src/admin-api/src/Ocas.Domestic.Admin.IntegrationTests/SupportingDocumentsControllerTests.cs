using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Enums;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class SupportingDocumentsControllerTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;

        public SupportingDocumentsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetSupportingDocumentFile_ShouldPass()
        {
            // Arrange
            var testUser = await IdentityUserFixture.GetApplicantUser(TestConstants.Identity.Providers.OcasApplicants.ApplicantWithSupportingDocuments, TestConstants.Identity.Providers.OcasApplicants.AlternatePassword);
            ApplyApiClient.WithAccessToken(testUser.AccessToken);
            var applicant = await ApplyApiClient.GetCurrentApplicant();
            var supportingDocuments = await ApplyApiClient.GetSupportingDocuments(applicant.Id);
            var supportingDocumentId = supportingDocuments.First(x => x.Type == SupportingDocumentType.Other && !x.Processing).Id;

            // Act
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
            var supportingDocument = await Client.GetSupportingDocumentFile(supportingDocumentId);

            // Assert
            supportingDocument.Should().NotBeNull();
            supportingDocument.Data.Should().NotBeNull();
            supportingDocument.Name.Should().NotBeNull();
            supportingDocument.MimeType.Should().NotBeNull();
        }
    }
}
