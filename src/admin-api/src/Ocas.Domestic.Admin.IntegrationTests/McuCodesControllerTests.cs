using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Models;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class McuCodesControllerTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly Faker _faker;

        public McuCodesControllerTests()
                : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [IntegrationTest]
        public async Task McuCode_ShouldPass_With_Partner()
        {
            var code = _faker.PickRandom(_models.AllAdminLookups.McuCodes).Code;
            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);
            var result = await Client.GetMcuCode(code);

            result.Code.Should().Be(code);
        }

        [Fact]
        [IntegrationTest]
        public async Task McuCode_ShouldPass_With_Ocas()
        {
            var code = _faker.PickRandom(_models.AllAdminLookups.McuCodes).Code;
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
            var result = await Client.GetMcuCode(code);

            result.Code.Should().Be(code);
        }

        [Fact]
        [IntegrationTest]
        public async void McuCodes_ShouldPass_With_Partner()
        {
            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);
            var result = await Client.GetMcuCodes(new GetMcuCodeOptions());

            result.Items.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async void McuCodes_ShouldPass_With_Ocas()
        {
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
            var options = new GetMcuCodeOptions();
            var result = await Client.GetMcuCodes(options);

            result.Items.Should().NotBeNullOrEmpty();
        }
    }
}
