using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Services.Validators.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class ProgramChoiceBaseValidatorTests
    {
        private readonly ProgramChoiceBaseValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;

        public ProgramChoiceBaseValidatorTests()
        {
            _validator = new ProgramChoiceBaseValidator(XunitInjectionCollection.LookupsCache);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ProgramChoiceBaseValidator_ShouldPass()
        {
            // Arrange
            var model = _modelFakerFixture.GetProgramChoiceBase().Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ProgramChoiceBaseValidator_ShouldThrow_When_EntryLevelInvalid()
        {
            // Arrange
            var model = _modelFakerFixture.GetProgramChoiceBase().Generate();
            model.EntryLevelId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == $"'Entry Level Id' does not exist: {model.EntryLevelId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ProgramChoiceBaseValidator_ShouldThrow_When_EntryLevelEmpty()
        {
            // Arrange
            var model = _modelFakerFixture.GetProgramChoiceBase().Generate();
            model.EntryLevelId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "'Entry Level Id' must not be empty.");
        }
    }
}
