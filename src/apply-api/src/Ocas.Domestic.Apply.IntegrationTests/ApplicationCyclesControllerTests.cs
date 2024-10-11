using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Core;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class ApplicationCyclesControllerTests : BaseTest<ApplyApiClient>
    {
        private readonly IdentityUserFixture _identityUserFixture;

        public ApplicationCyclesControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.ModelFakerFixture, XunitInjectionCollection.IdentityUserFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetApplicationCycles_ShouldPass()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.Applicant.AccessToken);

            // Act
            var result = await Client.GetApplicationCycles();

            // Assert
            result.Should().NotBeNullOrEmpty()
                .And.BeInDescendingOrder(x => x.Year)
                .And.OnlyHaveUniqueItems()
                .And.OnlyContain(x => x.Status == Constants.ApplicationCycleStatuses.Active || x.Status == Constants.ApplicationCycleStatuses.Previous);
        }
    }
}
