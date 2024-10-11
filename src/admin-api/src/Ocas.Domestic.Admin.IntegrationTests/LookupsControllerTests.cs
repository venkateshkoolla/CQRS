using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class LookupsControllerTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;

        public LookupsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetLookups_ShouldPass()
        {
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
            var result = await Client.GetLookups();
            result.Should().NotBeNull();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetLookups_ShouldPass_When_Filtered()
        {
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
            var result = await Client.GetLookups("applicationStatuses", "transcriptTransmissions");
            result.Should().NotBeNull();
            result.ApplicationStatuses.Should().NotBeNullOrEmpty();
            result.TranscriptTransmissions.Should().NotBeNullOrEmpty();
        }
    }
}
