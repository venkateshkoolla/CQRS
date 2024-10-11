using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Api.Client;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class DiagnosticsControllerTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;

        public DiagnosticsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
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
        public void Authorization_ShouldThrow_WhenOcasWithoutAccess()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.OcasNoAccessUser.AccessToken);

            // Act
            Func<Task> action = () => Client.GetDiagnosticAuthorization();

            // Assert
            var result = action.Should().Throw<StatusCodeException>();
            result.Which.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        [IntegrationTest]
        public async Task Authorization_ShouldPass_WhenPartner()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);

            // Act
            var result = await Client.GetDiagnosticAuthorization();
            // Assert
            result.Should().NotBeNullOrEmpty();
        }
    }
}
