using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class TranscriptRequestBaseValidatorTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly TranscriptRequestBaseValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;

        public TranscriptRequestBaseValidatorTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _validator = new TranscriptRequestBaseValidator(_lookupsCache);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task TranscriptRequestBaseValidator_ShouldPass()
        {
            // Arrange
            var model = _modelFakerFixture.GetTranscriptRequestBase().Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task TranscriptRequestBaseValidator_ShouldFail_When_IdsEmpty()
        {
            // Arrange
            var model = _modelFakerFixture.GetTranscriptRequestBase().Generate();
            model.ApplicationId = Guid.Empty;
            model.FromInstituteId = Guid.Empty;
            model.TransmissionId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task TranscriptRequestBaseValidator_ShouldFail_When_TransmissionIdInvalid()
        {
            // Arrange
            var model = _modelFakerFixture.GetTranscriptRequestBase().Generate();
            model.TransmissionId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == $"'Transmission Id' does not exist: {model.TransmissionId}");
        }
    }
}
