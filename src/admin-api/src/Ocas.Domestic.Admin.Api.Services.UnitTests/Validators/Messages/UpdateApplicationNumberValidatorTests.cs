using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework.RuleCollections;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class UpdateApplicationNumberValidatorTests : IClassFixture<UpdateApplicationNumberValidator>
    {
        private const string EightNumbers = "########";
        private readonly UpdateApplicationNumberValidator _validator;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly AllLookups _lookups;

        public UpdateApplicationNumberValidatorTests(UpdateApplicationNumberValidator validator)
        {
            _validator = validator;
            _lookups = XunitInjectionCollection.ModelFakerFixture.AllAdminLookups;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateApplicationValidator_ShouldPass_When_NineNumbers()
        {
            // Arrange
            var applicationCycle = _lookups.ApplicationCycles.OrderByDescending(x => x.Year).First();
            var model = new Faker<UpdateApplicationNumber>()
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.Number, f => f.GenerateApplicationNumber(applicationCycle.Year))
                .RuleFor(x => x.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateApplicationValidator_ShouldPass_When_EightNumbers()
        {
            // Arrange
            var applicationCycle = _lookups.ApplicationCycles.OrderByDescending(x => x.Year).First();
            var model = new Faker<UpdateApplicationNumber>()
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.Number, f => f.GenerateApplicationNumber(applicationCycle.Year, EightNumbers))
                .RuleFor(x => x.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateApplicationValidator_ShouldFail_When_ApplicationId_Empty()
        {
            // Arrange
            var applicationCycle = _lookups.ApplicationCycles.OrderByDescending(x => x.Year).First();
            var model = new Faker<UpdateApplicationNumber>()
                .RuleFor(x => x.ApplicationId, _ => Guid.Empty)
                .RuleFor(x => x.Number, f => f.GenerateApplicationNumber(applicationCycle.Year, EightNumbers))
                .RuleFor(x => x.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Application Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateApplicationValidator_ShouldFail_When_Number_Empty()
        {
            // Arrange
            var model = new Faker<UpdateApplicationNumber>()
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.Number, _ => string.Empty)
                .RuleFor(x => x.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Number' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateApplicationValidator_ShouldFail_When_Number_TooShort()
        {
            // Arrange
            const string format = "####";
            var model = new Faker<UpdateApplicationNumber>()
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.Number, f => f.Random.ReplaceNumbers(format))
                .RuleFor(x => x.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == $"'Number' must be between 8 and 9 characters. You entered {format.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateApplicationValidator_ShouldFail_When_Number_TooLong()
        {
            // Arrange
            const string format = "##########";
            var model = new Faker<UpdateApplicationNumber>()
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.Number, f => f.Random.ReplaceNumbers(format))
                .RuleFor(x => x.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == $"'Number' must be between 8 and 9 characters. You entered {format.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateApplicationValidator_ShouldFail_When_Number_NotNumbers()
        {
            // Arrange
            var model = new Faker<UpdateApplicationNumber>()
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.Number, f => f.Random.AlphaNumeric(8))
                .RuleFor(x => x.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Number' is not in the correct format.");
        }
    }
}
