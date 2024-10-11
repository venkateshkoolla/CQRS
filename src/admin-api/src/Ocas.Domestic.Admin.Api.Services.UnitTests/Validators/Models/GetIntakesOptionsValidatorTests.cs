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
    public class GetIntakesOptionsValidatorTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly GetIntakesOptionsValidator _validator;
        private readonly AdminTestFramework.ModelFakerFixture _models;

        public GetIntakesOptionsValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _validator = new GetIntakesOptionsValidator(_lookupsCache);
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesOptionsValidator_ShouldPass()
        {
            // Arrange
            var model = new GetIntakesOptions
            {
                CampusId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses).Id,
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                ProgramCode = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                ProgramTitle = _dataFakerFixture.Faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesOptionsValidator_ShouldFail_When_CampusId_NotFound()
        {
            // Arrange
            var model = new GetIntakesOptions
            {
                CampusId = Guid.NewGuid(),
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                ProgramCode = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                ProgramTitle = _dataFakerFixture.Faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Campus Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesOptionsValidator_ShouldFail_When_ProgramDeliveryId_NotFound()
        {
            // Arrange
            var model = new GetIntakesOptions
            {
                CampusId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses).Id,
                DeliveryId = Guid.NewGuid(),
                ProgramCode = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                ProgramTitle = _dataFakerFixture.Faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Delivery Id' does not exist.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesOptionsValidator_ShouldFail_When_ProgramCode_TooShort()
        {
            // Arrange
            var model = new GetIntakesOptions
            {
                CampusId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses).Id,
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                ProgramCode = _dataFakerFixture.Faker.Random.AlphaNumeric(1).ToUpperInvariant(),
                ProgramTitle = _dataFakerFixture.Faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"'Program Code' must be between 2 and 8 characters. You entered {model.ProgramCode.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesOptionsValidator_ShouldFail_When_ProgramCode_TooLong()
        {
            // Arrange
            var model = new GetIntakesOptions
            {
                CampusId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses).Id,
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                ProgramCode = _dataFakerFixture.Faker.Random.AlphaNumeric(10).ToUpperInvariant(),
                ProgramTitle = _dataFakerFixture.Faker.Random.Words(5)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"'Program Code' must be between 2 and 8 characters. You entered {model.ProgramCode.Length} characters.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task GetIntakesOptionsValidator_ShouldFail_When_ProgramTitle_TooLong()
        {
            // Arrange
            var model = new GetIntakesOptions
            {
                CampusId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.Campuses).Id,
                DeliveryId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.StudyMethods).Id,
                ProgramCode = _dataFakerFixture.Faker.Random.AlphaNumeric(8).ToUpperInvariant(),
                ProgramTitle = _dataFakerFixture.Faker.Random.Words(55)
            };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"The length of 'Program Title' must be 200 characters or fewer. You entered {model.ProgramTitle.Length} characters.");
        }
    }
}
