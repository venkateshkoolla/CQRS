using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework.RuleCollections;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class IntakesControllerTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly Faker _faker;

        public IntakesControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetIntakes_ShouldPass_With_OcasUser()
        {
            // Arrange
            var collegeId = _models.AllAdminLookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycle = _faker.PickRandom(_models.AllAdminLookups.ApplicationCycles.Where(x => x.Status == Constants.ApplicationCycleStatuses.Active));
            var collgeApplicationCycle = _faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(x => x.MasterId == applicationCycle.Id && x.CollegeId == collegeId));

            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var intakes = await Client.GetIntakes(collgeApplicationCycle.MasterId, collgeApplicationCycle.CollegeId, null);

            // Assert
            intakes.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task GetIntakes_ShouldPass_With_Partner()
        {
            // Arrange
            var colleges = _models.AllAdminLookups.Colleges;
            var collegeId = _models.AllAdminLookups.Colleges.First(x => x.IsOpen && x.Id == colleges.FirstOrDefault(c => c.Code == "SENE").Id).Id;
            var applicationCycle = _faker.PickRandom(_models.AllAdminLookups.ApplicationCycles.Where(x => x.Status == Constants.ApplicationCycleStatuses.Active));
            var collgeApplicationCycle = _faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(x => x.MasterId == applicationCycle.Id && x.CollegeId == collegeId));

            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);

            // Act
            var intakes = await Client.GetIntakes(collgeApplicationCycle.MasterId, collgeApplicationCycle.CollegeId, null);

            // Assert
            intakes.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task IntakeApplicants_ShouldPass_With_CollegeUser()
        {
            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);
            var result = await Client.GetIntakeApplicants(TestConstants.Intake.IntakeIdWithApplicants, new GetIntakeApplicantOptions());

            result.Items.Should().NotBeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task IntakeApplicants_ShouldPass_With_OcasUser()
        {
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
            var result = await Client.GetIntakeApplicants(TestConstants.Intake.IntakeIdWithApplicants, new GetIntakeApplicantOptions());

            result.Items.Should().NotBeEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task IntakeExport_ShouldPass_When_OcasUser()
        {
            // Arrange
            var applicationCycle = _faker.PickRandom(_models.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active));
            var collegeApplicationCycles = _models.AllAdminLookups.CollegeApplicationCycles.Where(a => a.MasterId == applicationCycle.Id).ToList();
            var college = _faker.PickRandom(_models.AllAdminLookups.Colleges.WithAppCycle(collegeApplicationCycles, applicationCycle.Id));

            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var result = await Client.IntakeExport(applicationCycle.Id, college.Id, null);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<BinaryDocument>();
            result.Data.Should().NotBeNullOrEmpty();
        }

        [Fact]
        [IntegrationTest]
        public async Task UpdateIntakeAvailability_ShouldPass()
        {
            // Arrange
            var collegeId = _faker.PickRandom(_models.AllAdminLookups.Colleges.Where(c => c.IsOpen)).Id;
            var appCycleStatus = _models.AllAdminLookups.ApplicationCycleStatuses.FirstOrDefault(s => s.Code == Constants.ApplicationCycleStatuses.Active);
            var collegeApplicationCycle = _faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(a => a.CollegeId == collegeId && a.StatusId == appCycleStatus.Id));

            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            var intakes = await Client.GetIntakes(collegeApplicationCycle.MasterId, collegeApplicationCycle.CollegeId, null);
            var intakesToUpdate = _faker.PickRandom(intakes, 2).Select(x => x.Id).ToList();

            // Act
            Func<Task> func = () => Client.UpdateIntakeAvailability(_faker.PickRandom(_models.AllAdminLookups.IntakeAvailabilities).Id, intakesToUpdate);

            // Assert
            await func.Should().NotThrowAsync();
        }
    }
}
