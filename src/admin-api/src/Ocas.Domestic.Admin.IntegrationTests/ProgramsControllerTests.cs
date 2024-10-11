using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class ProgramsControllerTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly Faker _faker;

        public ProgramsControllerTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [IntegrationTest]
        public async Task GetProgram_ShouldPass_When_OcasUser()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
            var expected = await CreateNewProgram();

            // Act
            var result = await Client.GetProgram(expected.Id);

            // Assert
            result.Id.Should().Be(expected.Id);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetProgram_ShouldPass_When_CollegeUser()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.CollegeUserSene.AccessToken);
            var expected = await CreateNewProgram();

            // Act
            var result = await Client.GetProgram(expected.Id);

            // Assert
            result.Id.Should().Be(expected.Id);
        }

        [Fact]
        [IntegrationTest]
        public async Task GetProgramBriefs_ShouldPass_When_OcasUser()
        {
            // Arrange
            var collegeId = _faker.PickRandom(_models.AllAdminLookups.Colleges.Where(c => c.IsOpen)).Id;
            var appCycleStatus =
                _models.AllAdminLookups.ApplicationCycleStatuses.FirstOrDefault(s =>
                    s.Code == Constants.ApplicationCycleStatuses.Active);
            var collegeApplicationCycle =
                _faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(a =>
                    a.CollegeId == collegeId && a.StatusId == appCycleStatus.Id));

            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var result = await Client.GetProgramBriefs(collegeApplicationCycle.MasterId, collegeApplicationCycle.CollegeId, new GetProgramBriefOptions());

            // Assert
            result.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.OnlyContain(c => c.CollegeId == collegeApplicationCycle.CollegeId);
        }

        [Fact]
        [IntegrationTest]
        public async Task ProgramExport_ShouldPass_When_OcasUser()
        {
            // Arrange
            var collegeId = _faker.PickRandom(_models.AllAdminLookups.Colleges.Where(c => c.IsOpen)).Id;
            var appCycleStatus =
                _models.AllAdminLookups.ApplicationCycleStatuses.FirstOrDefault(s =>
                    s.Code == Constants.ApplicationCycleStatuses.Active);
            var collegeApplicationCycle =
                _faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(a =>
                    a.CollegeId == collegeId && a.StatusId == appCycleStatus.Id));

            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

            // Act
            var result = await Client.ProgramExport(collegeApplicationCycle.MasterId, collegeApplicationCycle.CollegeId, new GetProgramOptions());

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<BinaryDocument>();
            result.Data.Should().NotBeNullOrEmpty();
        }
    }

    public class ProgramsControllerCreateTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;

        public ProgramsControllerCreateTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task ProgramCreate_ShouldPass()
        {
            Guid? createResultId = null;
            try
            {
                // Arrange
                Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);

                var programBase = _models.GetProgramBase().Generate();
                programBase.Intakes = _models.GetProgramIntake(programBase).Generate(3);

                // Act
                var createResult = await Client.CreateProgram(programBase);

                // Assert
                createResult.Id.Should().NotBeEmpty();
                createResultId = createResult.Id;
                createResult.Should().BeEquivalentTo(programBase, options => options
                    .Excluding(x => x.Intakes)
                    .Excluding(x => x.ModifiedBy)
                    .Excluding(x => x.ModifiedDate));
                createResult.Intakes.Should().NotBeNullOrEmpty()
                    .And.BeInAscendingOrder(i => i.StartDate);

                var expectedIntakes = programBase.Intakes.OrderBy(x => x.StartDate).ToList();
                var actualIntakes = createResult.Intakes;

                expectedIntakes.ForEach(x => x.EntryLevelIds = x.EntryLevelIds ?? new List<Guid>());

                actualIntakes.Should().BeEquivalentTo(expectedIntakes, options => options
                    .Excluding(x => x.Id)
                    .Excluding(x => x.ModifiedBy)
                    .Excluding(x => x.ModifiedDate)
                    .Excluding(x => x.CanDelete));
            }
            finally
            {
                if (createResultId.HasValue)
                    await Client.DeleteProgram(createResultId.Value);
            }
        }
    }

    public class ProgramsControllerDeleteTests : BaseTest
    {
        private readonly IdentityUserFixture _identityUserFixture;

        public ProgramsControllerDeleteTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task ProgramDelete_ShouldPass()
        {
            // Arrange
            Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
            var createdProgram = await CreateNewProgram();

            // Act
            Func<Task> a = () => Client.DeleteProgram(createdProgram.Id);

            // Assert
            a.Should().NotThrow();
        }
    }

    public class ProgramsControllerUpdateTests : BaseTest
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly IdentityUserFixture _identityUserFixture;

        public ProgramsControllerUpdateTests()
            : base(XunitInjectionCollection.TestServerFixture, XunitInjectionCollection.DataFakerFixture, XunitInjectionCollection.IdentityUserFixture, XunitInjectionCollection.ModelFakerFixture)
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _identityUserFixture = XunitInjectionCollection.IdentityUserFixture;
        }

        [Fact]
        [IntegrationTest]
        public async Task ProgramUpdate_ShouldPass()
        {
            Guid? createResultId = null;
            try
            {
                // Arrange
                Client.WithAccessToken(_identityUserFixture.OcasCccUser.AccessToken);
                var createdProgram = await CreateNewProgram();
                var newProgram = _models.GetProgram().Generate();
                newProgram.Id = createdProgram.Id;
                newProgram.ApplicationCycleId = createdProgram.ApplicationCycleId;
                newProgram.CampusId = createdProgram.CampusId;
                newProgram.Code = createdProgram.Code;
                newProgram.CollegeId = createdProgram.CollegeId;
                newProgram.DeliveryId = createdProgram.DeliveryId;
                newProgram.Intakes = _models.GetProgramIntake(newProgram).Generate(3);

                // Act
                var updateResult = await Client.UpdateProgram(newProgram.Id, newProgram);

                // Assert
                updateResult.Id.Should().NotBeEmpty();
                updateResult.Should().BeEquivalentTo(newProgram, options => options
                    .Excluding(x => x.Intakes)
                    .Excluding(x => x.ModifiedBy)
                    .Excluding(x => x.ModifiedDate));

                var expectedIntakes = newProgram.Intakes.OrderBy(x => x.StartDate).ToList();
                var actualIntakes = updateResult.Intakes.OrderBy(x => x.StartDate).ToList();

                expectedIntakes.ForEach(x => x.EntryLevelIds = x.EntryLevelIds ?? new List<Guid>());

                actualIntakes.Should().BeEquivalentTo(expectedIntakes, options => options
                    .Excluding(x => x.Id)
                    .Excluding(x => x.ModifiedBy)
                    .Excluding(x => x.ModifiedDate)
                    .Excluding(x => x.CanDelete));

                createResultId = updateResult.Id;
            }
            finally
            {
                if (createResultId.HasValue)
                    await Client.DeleteProgram(createResultId.Value);
            }
        }
    }
}
