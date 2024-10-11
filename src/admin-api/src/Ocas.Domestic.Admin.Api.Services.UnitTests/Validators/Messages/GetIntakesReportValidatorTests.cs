using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class GetIntakesReportValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly GetIntakesReportValidator _validator;

        public GetIntakesReportValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _validator = new GetIntakesReportValidator(_lookupsCache);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesReportValidator_ShouldPass()
        {
            // Arrange
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var model = new GetIntakesReport
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = college.Id,
                Params = new GetIntakesOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesReportValidator_ShouldFail_When_ApplicationCycleId_Empty()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var model = new GetIntakesReport
            {
                ApplicationCycleId = Guid.Empty,
                CollegeId = college.Id,
                Params = new GetIntakesOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Application Cycle Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesReportValidator_ShouldFail_When_ApplicationCycleId_NotFound()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var model = new GetIntakesReport
            {
                ApplicationCycleId = Guid.NewGuid(),
                CollegeId = college.Id,
                Params = new GetIntakesOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Application Cycle Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakessReportValidator_ShouldFail_When_CollegeId_Empty()
        {
            // Arrange
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var model = new GetIntakesReport
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = Guid.Empty,
                Params = new GetIntakesOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'College Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesReportValidator_ShouldFail_When_CollegeId_NotFound()
        {
            // Arrange
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var model = new GetIntakesReport
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = Guid.NewGuid(),
                Params = new GetIntakesOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'College Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesReportValidator_ShouldFail_When_CampusId_NotFound()
        {
            // Arrange
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var campuses = _models.AllAdminLookups.Campuses;

            var invalidCampusId = _dataFakerFixture.Faker.PickRandom(campuses.Where(x => x.CollegeId != college.Id))?.Id;

            var model = new GetIntakesReport
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = college.Id,
                Params = new GetIntakesOptions { CampusId = invalidCampusId },
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Campus Id' is not in requested college");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesOptionsValidator_ShouldFail_When_StartDate_InValid()
        {
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);

            // Arrange
            var model = new GetIntakesReport
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = college.Id,
                Params = new GetIntakesOptions { StartDate = _dataFakerFixture.Faker.Random.String2(5) },
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Start Date' must be within the application cycle");
        }
    }
}
