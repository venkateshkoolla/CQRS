using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Messages
{
    public class UpdateApplicationEffectiveDateValidatorTests : IClassFixture<UpdateApplicationEffectiveDateValidator>
    {
        private readonly UpdateApplicationEffectiveDateValidator _validator;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly AllLookups _lookups;

        public UpdateApplicationEffectiveDateValidatorTests(UpdateApplicationEffectiveDateValidator validator)
        {
            _validator = validator;
            _lookups = XunitInjectionCollection.ModelFakerFixture.AllAdminLookups;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateApplicationValidator_ShouldPass_When_EffectiveDate()
        {
            // Arrange
            var model = new Faker<UpdateApplicationEffectiveDate>()
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.EffectiveDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
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
            var model = new Faker<UpdateApplicationEffectiveDate>()
                .RuleFor(x => x.ApplicationId, _ => Guid.Empty)
                .RuleFor(x => x.EffectiveDate, f => f.Date.Past().AsUtc().ToStringOrDefault())
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
        public async Task UpdateApplicationValidator_ShouldFail_When_EffectiveDate_NotValid()
        {
            // Arrange
            var model = new Faker<UpdateApplicationEffectiveDate>()
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.EffectiveDate, string.Empty)
                .RuleFor(x => x.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Effective Date' must be a valid date.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task UpdateApplicationValidator_ShouldFail_When_Future_EffectiveDate()
        {
            // Arrange
            var model = new Faker<UpdateApplicationEffectiveDate>()
                .RuleFor(x => x.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.EffectiveDate, f => f.Date.Future().AsUtc().ToStringOrDefault())
                .RuleFor(x => x.User, _ => _user)
                .Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Effective Date' must be less than or equal to current date");
        }
    }
}
