using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class GetProgramOptionsValidatorTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly GetProgramOptionsValidator _validator;

        public GetProgramOptionsValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _validator = new GetProgramOptionsValidator(_lookupsCache);
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramOptionsValidator_ShouldPass()
        {
            // Arrange
            var model = new GetProgramOptions
            {
                CampusId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses).Id,
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                Title = _dataFakerFixture.Faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramOptionsValidator_ShouldFail_When_CampusId_NotFound()
        {
            // Arrange
            var model = new GetProgramOptions
            {
                CampusId = Guid.NewGuid(),
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                Title = _dataFakerFixture.Faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Campus Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramOptionsValidator_ShouldFail_When_DeliveryId_NotFound()
        {
            // Arrange
            var model = new GetProgramOptions
            {
                CampusId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses).Id,
                DeliveryId = Guid.NewGuid(),
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                Title = _dataFakerFixture.Faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Delivery Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramOptionsValidator_ShouldFail_When_Code_TooShort()
        {
            // Arrange
            var model = new GetProgramOptions
            {
                CampusId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses).Id,
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(1).ToUpperInvariant(),
                Title = _dataFakerFixture.Faker.Random.Words(5)
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
            var model = new GetProgramOptions
            {
                CampusId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses).Id,
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(10).ToUpperInvariant(),
                Title = _dataFakerFixture.Faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"'Code' must be between 2 and 8 characters. You entered {model.Code.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetProgramOptionsValidator_ShouldFail_When_Title_TooLong()
        {
            // Arrange
            var model = new GetProgramOptions
            {
                CampusId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses).Id,
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                Code = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                Title = _dataFakerFixture.Faker.Random.Words(55)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"The length of 'Title' must be 200 characters or fewer. You entered {model.Title.Length} characters.");
        }
    }
}
