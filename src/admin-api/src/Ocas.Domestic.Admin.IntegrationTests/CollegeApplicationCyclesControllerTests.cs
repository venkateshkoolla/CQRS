using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class CollegeApplicationCyclesControllerTests : BaseTest
    {
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly Faker _faker;
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly CrmDatabaseFixture _crmDatabaseFixture;

        public CollegeApplicationCyclesControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _crmDatabaseFixture = XunitInjectionCollection.CrmDatabaseFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetCollegeApplicationCycles_ShouldPass_When_OcasUser()
        {
            // Arrange
            var collegeId = _faker.PickRandom(_models.AllAdminLookups.Colleges.Where(c => c.IsOpen)).Id;
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var result = await Client.GetCollegeApplicationCycles(collegeId);

            // Assert
            result.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.OnlyContain(c => c.CollegeId == collegeId);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetCollegeApplicationCycles_ShouldPass_When_CollegeUser()
        {
            // Arrange
            var collegeId = _models.AllAdminLookups.Colleges.First(c => c.Code == "SENE").Id;
            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);

            // Act
            var result = await Client.GetCollegeApplicationCycles(collegeId);

            // Assert
            result.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.OnlyContain(c => c.CollegeId == collegeId);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetSpecialCode_ShouldPass_When_OcasUser()
        {
            // Arrange
            var codes = new List<string>() as IList<string>;
            Guid? collegeAppCycleId = null;
            foreach (var college in _models.AllAdminLookups.Colleges.Where(c => c.IsOpen))
            {
                (collegeAppCycleId, codes) = await _crmDatabaseFixture.GetSpecialCodes(college.Id);
                if (collegeAppCycleId.HasValue && codes.Any()) break;
            }
            var code = _faker.PickRandom(codes);

            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var result = await Client.GetSpecialCode(collegeAppCycleId.Value, code);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<SpecialCode>();
            result.ApplicationCycleId.Should().Be(collegeAppCycleId.Value);
            result.Code.Should().Be(code);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetSpecialCode_ShouldPass_When_CollegeUser()
        {
            // Arrange
            var collegeId = _models.AllAdminLookups.Colleges.First(c => c.Code == "SENE").Id;
            var(collegeAppCycleId, codes) = await _crmDatabaseFixture.GetSpecialCodes(collegeId);
            if (!collegeAppCycleId.HasValue || !codes.Any()) throw new Exception("No college application cycle for SENE has special codes");
            var code = _faker.PickRandom(codes);

            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);

            // Act
            var result = await Client.GetSpecialCode(collegeAppCycleId.Value, code);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<SpecialCode>();
            result.ApplicationCycleId.Should().Be(collegeAppCycleId.Value);
            result.Code.Should().Be(code);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetSpecialCodes_ShouldPass_When_OcasUser()
        {
            // Arrange
            var codes = new List<string>() as IList<string>;
            Guid? collegeAppCycleId = null;
            foreach (var college in _models.AllAdminLookups.Colleges.Where(c => c.IsOpen))
            {
                (collegeAppCycleId, codes) = await _crmDatabaseFixture.GetSpecialCodes(college.Id);
                if (collegeAppCycleId.HasValue && codes.Any()) break;
            }

            var searchCriteria = new GetSpecialCodeOptions { Search = _faker.PickRandom(codes) };
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var result = await Client.GetSpecialCodes(collegeAppCycleId.Value, searchCriteria);

            // Assert
            result.TotalCount.Should().BePositive();
            result.Items.Should()
                .OnlyContain(x => x.ApplicationCycleId == collegeAppCycleId.Value)
                .And.OnlyContain(x => x.Description.IndexOf(searchCriteria.Search, StringComparison.OrdinalIgnoreCase) >= 0
                                     || x.Code.IndexOf(searchCriteria.Search, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetSpecialCodes_ShouldPass_When_CollegeUser()
        {
            // Arrange
            var collegeId = _models.AllAdminLookups.Colleges.First(c => c.Code == "SENE").Id;
            var(collegeAppCycleId, codes) = await _crmDatabaseFixture.GetSpecialCodes(collegeId);
            if (!collegeAppCycleId.HasValue || !codes.Any()) throw new Exception("No college application cycle for SENE has special codes");

            var searchCriteria = new GetSpecialCodeOptions { Search = _faker.PickRandom(codes) };
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var result = await Client.GetSpecialCodes(collegeAppCycleId.Value, searchCriteria);

            // Assert
            result.TotalCount.Should().BePositive();
            result.Items.Should()
                .OnlyContain(x => x.ApplicationCycleId == collegeAppCycleId.Value)
                .And.OnlyContain(x => x.Description.IndexOf(searchCriteria.Search, StringComparison.OrdinalIgnoreCase) >= 0
                                     || x.Code.IndexOf(searchCriteria.Search, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
