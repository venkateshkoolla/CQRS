using System;
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
    public class GetProgramsReportValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly GetProgramsReportValidator _validator;

        public GetProgramsReportValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _validator = new GetProgramsReportValidator(_lookupsCache);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramsReportValidator_ShouldPass()
        {
            // Arrange
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var model = new GetProgramsReport
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = college.Id,
                Params = new GetProgramOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramsReportValidator_ShouldFail_When_ApplicationCycleId_Empty()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var model = new GetProgramsReport
            {
                ApplicationCycleId = Guid.Empty,
                CollegeId = college.Id,
                Params = new GetProgramOptions(),
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
        public async Task GetProgramsReportValidator_ShouldFail_When_ApplicationCycleId_NotFound()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var model = new GetProgramsReport
            {
                ApplicationCycleId = Guid.NewGuid(),
                CollegeId = college.Id,
                Params = new GetProgramOptions(),
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
        public async Task GetProgramsReportValidator_ShouldFail_When_CollegeId_Empty()
        {
            // Arrange
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var model = new GetProgramsReport
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = Guid.Empty,
                Params = new GetProgramOptions(),
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
        public async Task GetProgramsReportValidator_ShouldFail_When_CollegeId_NotFound()
        {
            // Arrange
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var model = new GetProgramsReport
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = Guid.NewGuid(),
                Params = new GetProgramOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'College Id' does not exist.");
        }
    }
}
