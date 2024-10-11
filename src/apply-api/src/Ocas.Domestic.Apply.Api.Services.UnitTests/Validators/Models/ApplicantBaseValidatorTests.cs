using System;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class ApplicantBaseValidatorTests
    {
        private readonly ApplicantBaseValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly Faker _faker;

        public ApplicantBaseValidatorTests()
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _validator = new ApplicantBaseValidator();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldPass()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldPass_When_MiddleName_Empty()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.MiddleName = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_FirstName_Empty()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.FirstName = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'First Name' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_FirstName_TooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.FirstName = _faker.Random.String2(40);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"The length of 'First Name' must be 30 characters or fewer. You entered {model.FirstName.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_FirstName_NotRegexMatch()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.FirstName = _faker.Random.AlphaNumeric(25);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'First Name' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_MiddleName_TooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.MiddleName = _faker.Random.String2(40);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"The length of 'Middle Name' must be 30 characters or fewer. You entered {model.MiddleName.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_MiddleName_NotRegexMatch()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.MiddleName = _faker.Random.AlphaNumeric(25);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Middle Name' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_LastName_Empty()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.LastName = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Last Name' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_LastName_TooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.LastName = _faker.Random.String2(40);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"The length of 'Last Name' must be 30 characters or fewer. You entered {model.LastName.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_LastName_NotRegexMatch()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.LastName = _faker.Random.AlphaNumeric(25);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Last Name' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_BirthDate_Empty()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.BirthDate = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Birth Date' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_BirthDate_NotADate()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.BirthDate = "NotADate";

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Birth Date' must be a date.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantBaseValidator_ShouldFail_When_BirthDate_InvalidRange()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicantBase().Generate();
            model.BirthDate = DateTime.UtcNow.AddYears(-100).ToStringOrDefault();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"Age must be within 16 and 90 years old: {model.BirthDate}");
        }
    }
}
