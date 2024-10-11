using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class CreateProgramChoiceValidatorTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly TestFramework.ModelFakerFixture _modelFaker;
        private readonly CreateProgramChoiceValidator _validator;

        private readonly CreateProgramChoice _model;

        public CreateProgramChoiceValidatorTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _validator = new CreateProgramChoiceValidator(_lookupsCache);

            _model = new Faker<CreateProgramChoice>()
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.User, _ => Mock.Of<IPrincipal>())
                .RuleFor(x => x.ProgramChoice, (_, o) =>
                {
                    var model = _modelFaker.GetCreateProgramChoiceRequest().Generate();
                    model.ApplicationId = o.ApplicationId;
                    return model;
                })
                .Generate();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceValidator_ShouldPass()
        {
            // Arrange & Act
            var result = await _validator.ValidateAsync(_model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceValidator_ShouldFail_When_ApplicationId_Empty()
        {
            // Arrange
            var model = _model;
            model.ApplicationId = Guid.Empty;
            model.ProgramChoice.ApplicationId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2)
                .And.OnlyContain(x => x.ErrorMessage == "'Application Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceValidator_ShouldFail_When_ProgramChoice_Empty()
        {
            // Arrange
            var model = _model;
            model.ProgramChoice = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Program Choice' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceValidator_ShouldFail_When_ApplicationId_ProgramChoice_NotMatch()
        {
            // Arrange
            var model = _model;
            model.ApplicationId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Program Choice' must be for requested application.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task CreateProgramChoiceValidator_ShouldFail_When_ProgramChoice_ApplicationId_NotMatch()
        {
            // Arrange
            var model = _model;
            model.ProgramChoice.ApplicationId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.OnlyContain(x => x.ErrorMessage == "'Program Choice' must be for requested application.");
        }
    }
}
