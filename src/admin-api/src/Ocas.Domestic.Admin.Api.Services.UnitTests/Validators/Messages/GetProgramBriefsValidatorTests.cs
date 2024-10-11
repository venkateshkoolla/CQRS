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
    public class GetProgramBriefsValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly GetProgramBriefsValidator _validator;

        public GetProgramBriefsValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _validator = new GetProgramBriefsValidator(_lookupsCache);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramBriefsValidator_ShouldPass()
        {
            // Arrange
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var model = new GetProgramBriefs
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = college.Id,
                Params = new GetProgramBriefOptions(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramBriefsValidator_ShouldFail_When_ApplicationCycleId_Empty()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var model = new GetProgramBriefs
            {
                ApplicationCycleId = Guid.Empty,
                CollegeId = college.Id,
                Params = new GetProgramBriefOptions(),
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
        public async Task GetProgramBriefsValidator_ShouldFail_When_ApplicationCycleId_NotFound()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var model = new GetProgramBriefs
            {
                ApplicationCycleId = Guid.NewGuid(),
                CollegeId = college.Id,
                Params = new GetProgramBriefOptions(),
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
        public async Task GetProgramBriefsValidator_ShouldFail_When_CollegeId_Empty()
        {
            // Arrange
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var model = new GetProgramBriefs
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = Guid.Empty,
                Params = new GetProgramBriefOptions(),
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
        public async Task GetProgramBriefsValidator_ShouldFail_When_CollegeId_NotFound()
        {
            // Arrange
            var appCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles);
            var model = new GetProgramBriefs
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = Guid.NewGuid(),
                Params = new GetProgramBriefOptions(),
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
