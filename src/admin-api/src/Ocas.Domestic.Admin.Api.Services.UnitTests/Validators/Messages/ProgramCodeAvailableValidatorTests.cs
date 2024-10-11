using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Apply.TestFramework.RuleCollections;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class ProgramCodeAvailableValidatorTests
    {
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly ProgramCodeAvailableValidator _validator;

        public ProgramCodeAvailableValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _validator = new ProgramCodeAvailableValidator(_lookupsCache);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ProgramCodeAvailableValidator_ShouldPass()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges.WithCampuses(_models.AllAdminLookups.Campuses));
            var collegeAppCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(a => a.CollegeId == college.Id));
            var campus = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses.Where(c => c.CollegeId == college.Id));
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = campus.Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ProgramCodeAvailableValidator_ShouldFail_When_CollegeApplicationCycleId_Empty()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges.WithCampuses(_models.AllAdminLookups.Campuses));
            var campus = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses.Where(c => c.CollegeId == college.Id));
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = Guid.Empty,
                CampusId = campus.Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2)
                .And.ContainSingle(x => x.ErrorMessage == "'College Application Cycle Id' must not be empty.")
                .And.ContainSingle(x => x.ErrorMessage == "'Campus Id' does not exist in college's application cycle.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ProgramCodeAvailableValidator_ShouldFail_When_CollegeApplicationCycleId_NotFound()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges.WithCampuses(_models.AllAdminLookups.Campuses));
            var campus = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses.Where(c => c.CollegeId == college.Id));
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = Guid.NewGuid(),
                CampusId = campus.Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2)
                .And.ContainSingle(x => x.ErrorMessage == "'College Application Cycle Id' does not exist.")
                .And.ContainSingle(x => x.ErrorMessage == "'Campus Id' does not exist in college's application cycle.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ProgramCodeAvailableValidator_ShouldFail_When_CampusId_Empty()
        {
            // Arrange
            var collegeAppCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles);
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = Guid.Empty,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Campus Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ProgramCodeAvailableValidator_ShouldFail_When_CampusId_NotFound()
        {
            // Arrange
            var collegeAppCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles);
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = Guid.NewGuid(),
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Campus Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ProgramCodeAvailableValidator_ShouldFail_When_CampusId_NotInCollegeAppCycle()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges);
            var collegeAppCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(a => a.CollegeId == college.Id));
            var campus = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses.Where(c => c.CollegeId != college.Id));
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = campus.Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Campus Id' does not exist in college's application cycle.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ProgramCodeAvailableValidator_ShouldFail_When_DeliveryId_Empty()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges.WithCampuses(_models.AllAdminLookups.Campuses));
            var collegeAppCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(a => a.CollegeId == college.Id));
            var campus = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses.Where(c => c.CollegeId == college.Id));
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = campus.Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                DeliveryId = Guid.Empty,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Delivery Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ProgramCodeAvailableValidator_ShouldFail_When_DeliveryId_NotFound()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges.WithCampuses(_models.AllAdminLookups.Campuses));
            var collegeAppCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(a => a.CollegeId == college.Id));
            var campus = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses.Where(c => c.CollegeId == college.Id));
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = campus.Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                DeliveryId = Guid.NewGuid(),
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Delivery Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ProgramCodeAvailableValidator_ShouldFail_When_Code_Empty()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges.WithCampuses(_models.AllAdminLookups.Campuses));
            var collegeAppCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(a => a.CollegeId == college.Id));
            var campus = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses.Where(c => c.CollegeId == college.Id));
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = campus.Id,
                Code = string.Empty,
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Code' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ProgramCodeAvailableValidator_ShouldFail_When_Code_TooShort()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges.WithCampuses(_models.AllAdminLookups.Campuses));
            var collegeAppCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(a => a.CollegeId == college.Id));
            var campus = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses.Where(c => c.CollegeId == college.Id));
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = campus.Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(1).ToUpperInvariant(),
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"'Code' must be between 2 and 8 characters. You entered {model.Code.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramOptionsValidator_ShouldFail_When_Code_TooLong()
        {
            // Arrange
            var college = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Colleges.WithCampuses(_models.AllAdminLookups.Campuses));
            var collegeAppCycle = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.CollegeApplicationCycles.Where(a => a.CollegeId == college.Id));
            var campus = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses.Where(c => c.CollegeId == college.Id));
            var model = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = campus.Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(10).ToUpperInvariant(),
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                User = _user
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"'Code' must be between 2 and 8 characters. You entered {model.Code.Length} characters.");
        }
    }
}
