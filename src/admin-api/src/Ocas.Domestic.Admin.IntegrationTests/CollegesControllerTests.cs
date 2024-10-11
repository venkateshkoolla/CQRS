using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class CollegesControllerTests
        : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;

        public CollegesControllerTests()
                : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _models = XunitInjectionCollection.ModelFakerFixture;
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task Colleges_ShouldPass_With_OcasUser()
        {
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            var result = await Client.GetColleges();

            result.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems();
        }

        [Fact]
        [IntegrationTest]
        public async Task Colleges_ShouldPass_With_Partner()
        {
            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);

            var result = await Client.GetColleges();

            var colleges = _models.AllAdminLookups.Colleges;
            result.Should().OnlyContain(x => x.Id == colleges.FirstOrDefault(c => c.Code == "SENE").Id);
        }
    }
}
