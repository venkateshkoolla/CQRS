using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class UpdateProgramChoiceValidatorTests
    {
        private readonly UpdateProgramChoiceValidator _validator;
        private readonly IPrincipal _user = new Mock<IPrincipal>().Object;

        public UpdateProgramChoiceValidatorTests()
        {
            _validator = new UpdateProgramChoiceValidator();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoiceValidator_ShouldPass_When_EffectiveDate_Lessthan_CurrentDate()
        {
            // Arrange
            var model = new Faker<UpdateProgramChoice>()
                 .RuleFor(x => x.User, _user)
                 .RuleFor(x => x.ProgramChoiceId, f => f.Random.Guid())
                 .RuleFor(x => x.EffectiveDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                 .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoiceValidator_ShouldFail_When_ProgramChoiceId_Empty()
        {
            // Arrange
            var model = new Faker<UpdateProgramChoice>()
                 .RuleFor(x => x.ProgramChoiceId, Guid.Empty)
                 .RuleFor(x => x.User, _user)
                 .RuleFor(x => x.EffectiveDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
                 .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == "'Program Choice Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoiceValidator_ShouldFail_When_EffectiveDate_FutureDate()
        {
            // Arrange
            var model = new Faker<UpdateProgramChoice>()
                 .RuleFor(x => x.User, _user)
                 .RuleFor(x => x.ProgramChoiceId, f => f.Random.Guid())
                 .RuleFor(x => x.EffectiveDate, f => f.Date.Future().AsUtc().ToStringOrDefault())
                 .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().
                And.Contain(x => x.ErrorMessage == "'Effective Date' must be less than or equal to current date");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateProgramChoiceValidator_ShouldFail_When_EffectiveDate_Invalid()
        {
            // Arrange
            var model = new Faker<UpdateProgramChoice>()
                 .RuleFor(x => x.User, _user)
                 .RuleFor(x => x.ProgramChoiceId, f => f.Random.Guid())
                 .RuleFor(x => x.EffectiveDate, "InvalidDate")
                 .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Effective Date' must be a valid date.");
        }
    }
}
