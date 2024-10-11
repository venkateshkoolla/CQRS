using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Validators.Messages;
using Ocas.Domestic.Apply.Core.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Messages
{
    public class GetPartnerBrandingValidatorTests : IClassFixture<GetPartnerBrandingValidator>
    {
        private readonly GetPartnerBrandingValidator _validator;

        public GetPartnerBrandingValidatorTests(GetPartnerBrandingValidator validator)
        {
            _validator = validator;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetPartnerBrandingValidator_ShouldPass()
        {
            // Arrange
            var model = new GetPartnerBranding { Code = "ASDF" };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetPartnerBrandingValidator_ShouldFail_When_CodeEmpty()
        {
            // Arrange
            var model = new GetPartnerBranding { Code = string.Empty };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Code' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task GetPartnerBrandingValidator_ShouldFail_When_CodeTooLong()
        {
            // Arrange
            var model = new GetPartnerBranding { Code = "ASDFGHJKLQW" };

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"The length of 'Code' must be 10 characters or fewer. You entered {model.Code.Length} characters.");
        }
    }
}
