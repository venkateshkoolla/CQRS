using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class IntlCredentialAssessmentValidatorTests
    {
        private readonly IntlCredentialAssessmentValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;

        public IntlCredentialAssessmentValidatorTests()
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _validator = new IntlCredentialAssessmentValidator(XunitInjectionCollection.LookupsCache);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass()
        {
            // Arrange
            var model = _modelFakerFixture.GetIntlCredentialAssessment().Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_Without_Reference()
        {
            // Arrange
            var model = new IntlCredentialAssessment();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldThrow_When_Reference_TooShort()
        {
            // Arrange
            var model = _modelFakerFixture.GetIntlCredentialAssessment().Generate();
            model.IntlReferenceNumber = "01234";

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "The length of 'Intl Reference Number' must be at least 6 characters. You entered 5 characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldThrow_When_Reference_TooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetIntlCredentialAssessment().Generate();
            model.IntlReferenceNumber = "0123456789";

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "The length of 'Intl Reference Number' must be 8 characters or fewer. You entered 10 characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldThrow_When_Reference_NotNumeric()
        {
            // Arrange
            var model = _modelFakerFixture.GetIntlCredentialAssessment().Generate();
            model.IntlReferenceNumber = "ASDFQWE";

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "'Intl Reference Number' must be numeric");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldThrow_When_IntlEvaluatorId_Empty()
        {
            // Arrange
            var model = _modelFakerFixture.GetIntlCredentialAssessment().Generate();
            model.IntlEvaluatorId = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "'Intl Evaluator Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldThrow_When_IntlEvaluatorId_Invalid()
        {
            // Arrange
            var model = _modelFakerFixture.GetIntlCredentialAssessment().Generate();
            model.IntlEvaluatorId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == $"'Intl Evaluator Id' does not exist: {model.IntlEvaluatorId}");
        }
    }
}
