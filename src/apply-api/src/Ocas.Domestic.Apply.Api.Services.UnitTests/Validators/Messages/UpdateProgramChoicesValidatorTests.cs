using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Messages;
using Ocas.Domestic.Apply.Services.Validators.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class UpdateProgramChoicesValidatorTests
    {
        private readonly UpdateProgramChoicesValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly IPrincipal _user = new Mock<IPrincipal>().Object;

        public UpdateProgramChoicesValidatorTests()
        {
            _validator = new UpdateProgramChoicesValidator(XunitInjectionCollection.LookupsCache);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoicesValidator_ShouldPass()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var programChoiceBases = _modelFakerFixture.GetProgramChoiceBase().Generate(3);
            programChoiceBases.ForEach(c => c.ApplicationId = applicationId);

            // Act
            var model = new Faker<UpdateProgramChoices>()
                .RuleFor(o => o.User, _ => _user)
                .RuleFor(o => o.ApplicationId, applicationId)
                .RuleFor(o => o.ProgramChoices, programChoiceBases)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoicesValidator_ShouldPass_When_ProgramChoicesEmpty()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var programChoiceBases = new List<ProgramChoiceBase>();

            // Act
            var model = new Faker<UpdateProgramChoices>()
                .RuleFor(o => o.User, _ => _user)
                .RuleFor(o => o.ApplicationId, applicationId)
                .RuleFor(o => o.ProgramChoices, programChoiceBases)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoicesValidator_ShouldFail_When_ApplicationIdEmpty()
        {
            // Arrange
            var applicationId = Guid.Empty;
            var programChoiceBases = _modelFakerFixture.GetProgramChoiceBase().Generate(3);
            programChoiceBases.ForEach(c => c.ApplicationId = applicationId);

            // Act
            var model = new Faker<UpdateProgramChoices>()
                .RuleFor(o => o.User, _ => _user)
                .RuleFor(o => o.ApplicationId, applicationId)
                .RuleFor(o => o.ProgramChoices, programChoiceBases)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "'Application Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoicesValidator_ShouldFail_When_ProgramChoicesNull()
        {
            // Arrange
            var applicationId = Guid.NewGuid();

            // Act
            var model = new Faker<UpdateProgramChoices>()
                .RuleFor(o => o.User, _ => _user)
                .RuleFor(o => o.ApplicationId, applicationId)
                .RuleFor(o => o.ProgramChoices, _ => null)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Program Choices' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoicesValidator_ShouldFail_When_ProgramChoicesOverMax()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var programChoiceBases = _modelFakerFixture.GetProgramChoiceBase().Generate(Constants.ProgramChoices.MaxTotalChoices + 1);
            programChoiceBases.ForEach(c => c.ApplicationId = applicationId);

            // Act
            var model = new Faker<UpdateProgramChoices>()
                .RuleFor(o => o.User, _ => _user)
                .RuleFor(o => o.ApplicationId, applicationId)
                .RuleFor(o => o.ProgramChoices, programChoiceBases)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == $"No more than {Constants.ProgramChoices.MaxTotalChoices} choices.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoicesValidator_ShouldFail_When_ProgramChoicesApplicationIdNotAllEqual()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var programChoiceBases = _modelFakerFixture.GetProgramChoiceBase().Generate(3);

            // Act
            var model = new Faker<UpdateProgramChoices>()
                .RuleFor(o => o.User, _ => _user)
                .RuleFor(o => o.ApplicationId, applicationId)
                .RuleFor(o => o.ProgramChoices, programChoiceBases)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "'Program Choices' must be for same application.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoicesValidator_ShouldFail_When_ProgramChoicesSame()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var programChoiceBase = new ProgramChoiceBase
            {
                ApplicantId = Guid.NewGuid(),
                ApplicationId = applicationId,
                IntakeId = Guid.NewGuid(),
                EntryLevelId = Guid.NewGuid()
            };
            var programChoiceBases = new List<ProgramChoiceBase>
            {
                programChoiceBase,
                programChoiceBase,
                programChoiceBase
            };

            // Act
            var model = new Faker<UpdateProgramChoices>()
                .RuleFor(o => o.User, _ => _user)
                .RuleFor(o => o.ApplicationId, applicationId)
                .RuleFor(o => o.ProgramChoices, programChoiceBases)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Program Choices' must be for different intakes and entry level pairing.");
        }
    }
}
