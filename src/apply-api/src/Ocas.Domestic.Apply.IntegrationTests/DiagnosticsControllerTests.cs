using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class DiagnosticsControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly IdentityUserFixture _identityUserFixture;

        public DiagnosticsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task ServerTime_ShouldPass()
        {
            var result = await Client.GetDiagnosticServerTime();

            var utcNow = DateTime.UtcNow;
            var now = DateTime.Now;

            result.Should().NotBeNull();
            result.Utc.Should().BeCloseTo(utcNow, 2000);
            result.Local.Should().BeCloseTo(now, 2000);
        }

        [Fact]
        [IntegrationTest]
        public async Task Authorization_ShouldPass_WhenOcasWithAccess()
        {
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
            var result = await Client.GetDiagnosticAuthorization();
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task Authorization_ShouldThrow_WhenOcasWithoutAccess()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.OcasNoAccessUser.AccessToken);

            // Act
            Func<Task> action = () => Client.GetDiagnosticAuthorization();

            // Assert
            var result = await action.Should().ThrowAsync<StatusCodeException>();
            result.Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [IntegrationTest]
        public async Task Authorization_ShouldPass_WhenApplicant()
        {
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);
            var result = await Client.GetDiagnosticAuthorization();
            result.Should().NotBeNullOrEmpty();
        }
    }
}
