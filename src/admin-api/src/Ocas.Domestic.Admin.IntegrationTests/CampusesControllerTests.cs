using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework.RuleCollections;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class CampusesControllerTests : BaseTest
    {
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly Faker _faker;
        private readonly IdentityUserFixture _identityUserFixture;

        public CampusesControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _models = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task Campuses_ShouldPass_With_OcasUser()
        {
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            var campuses = _models.AllAdminLookups.Campuses;
            var collegeId = _faker.PickRandom(_models.AllAdminLookups.Colleges.WithCampuses(campuses)).Id;

            var result = await Client.GetCampuses(collegeId);

            result.Should().NotBeNullOrEmpty()
                .And.BeOfType<List<Campus>>()
                .And.OnlyHaveUniqueItems();
        }

        [Fact]
        [IntegrationTest]
        public async Task Campuses_ShouldPass_With_Partner()
        {
            var colleges = _models.AllAdminLookups.Colleges;
            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);

            var result = await Client.GetCampuses(colleges.FirstOrDefault(x => x.Code == "SENE").Id);

            result.Should().NotBeNullOrEmpty()
                .And.BeOfType<List<Campus>>()
                .And.OnlyHaveUniqueItems()
                .And.OnlyContain(x => x.CollegeId == colleges.FirstOrDefault(c => c.Code == "SENE").Id);
        }
    }
}
