using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class PrivacyStatementsTests : BaseTest<ApplyApiClient>
    {
        private readonly IdentityUserFixture _identityUserFixture;

        public PrivacyStatementsTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetLatestPrivacyStatement_ShouldPass()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);

            // Act
            var result = await Client.GetLatestPrivacyStatement();

            // Assert
            result.Id.Should().NotBeEmpty();
            result.Content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetPrivacyStatement_ShouldThrow_WhenIdIsEmpty()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);

            // Act
            Func<Task> action = () => Client.GetPrivacyStatement(Guid.Empty);

            // Assert
            var result = await action.Should().ThrowAsync<StatusCodeException>();
            result.Which.StatusCode.Should().Be(HttpStatusCode.NotFound);
            result.Which.OcasHttpResponseMessage.Should().NotBeNull();
            result.Which.OcasHttpResponseMessage.Errors.Should().NotBeEmpty()
                .And.Contain(x => x.Message == $"PrivacyStatement {Guid.Empty} not found");
        }
    }
}
