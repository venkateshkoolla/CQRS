using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class OsapTokensControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly IdentityUserFixture _identityUserFixture;

        public OsapTokensControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetOsapToken_ShouldPass()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var applicant = await Client.GetCurrentApplicant();

            // Act
            var token = await Client.GetOsapToken(applicant.Id);

            // Assert
            token.Should().NotBeNull();
            token.AccessToken.Should().NotBeNullOrEmpty();
            token.ExpiresIn.Should().BePositive();
            token.RefreshToken.Should().NotBeNullOrEmpty();
        }
    }
}
