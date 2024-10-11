using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Admin.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class CreateProgramChoiceRequestValidatorTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _modelFaker;
        private readonly CreateProgramChoiceRequestValidator _validator;
        private readonly CreateProgramChoiceRequest _model;

        public CreateProgramChoiceRequestValidatorTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _validator = new CreateProgramChoiceRequestValidator(_lookupsCache);

            _model = _modelFaker.GetCreateProgramChoiceRequest().Generate();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceRequestValidator_ShouldPass()
        {
            // Arrange & Act
            var result = await _validator.ValidateAsync(_model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceRequestValidator_ShouldFail_When_ApplicationId_Empty()
        {
            // Arrange
            var model = _model;
            model.ApplicationId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Application Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceRequestValidator_ShouldFail_When_EntryLevelId_Empty()
        {
            // Arrange
            var model = _model;
            model.EntryLevelId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Entry Level Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceRequestValidator_ShouldFail_When_EntryLevelId_Invalid()
        {
            // Arrange
            var model = _model;
            model.EntryLevelId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == $"'Entry Level Id' does not exist: {model.EntryLevelId}");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceRequestValidator_ShouldFail_When_StartDate_Empty()
        {
            // Arrange
            var model = _model;
            model.StartDate = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Start Date' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceRequestValidator_ShouldFail_When_StartDate_NotADate()
        {
            // Arrange
            var model = _model;
            model.StartDate = "NOTADATE";

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Start Date' must be a valid date.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceRequestValidator_ShouldFail_When_StartDate_IncorrectFormart()
        {
            // Arrange
            var model = _model;
            model.StartDate = DateTime.UtcNow.ToString(Constants.DateFormat.CompletedDate);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Start Date' must be a valid date.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceRequestValidator_ShouldFail_When_ProgramId_Empty()
        {
            // Arrange
            var model = _model;
            model.ProgramId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Program Id' must not be empty.");
        }
    }
}
