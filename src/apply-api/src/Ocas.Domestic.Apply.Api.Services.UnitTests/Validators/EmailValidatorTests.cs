using System.Threading.Tasks;
using Bogus.DataSets;
using FluentAssertions;
using Ocas.Domestic.Apply.Services.Validators;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators
{
    public class EmailValidatorTests
    {
        private readonly TestValidatorCollection _validator;

        public EmailValidatorTests()
        {
            _validator = new TestValidatorCollection(v => v.RuleFor(o => o.GenericString).OcasEmailAddress());
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass()
        {
            var email = new Internet().Email();

            var result = await _validator.ValidateAsync(new TestObject(email));

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_Empty()
        {
            var email = string.Empty;

            var result = await _validator.ValidateAsync(new TestObject(email));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Generic String' must not be empty."
                                                    && x.ErrorCode == "NotEmptyValidator");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_Length()
        {
            var email = new Internet().Email(provider: "Loremaipsumadolorasitaametaconsecteturadipiscing.ca");

            var result = await _validator.ValidateAsync(new TestObject(email));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Generic String' must be between 5 and 50 characters. You entered {email.Length} characters."
                                                    && x.ErrorCode == "LengthValidator");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_Regex()
        {
            const string email = "IamNotanEmail";

            var result = await _validator.ValidateAsync(new TestObject(email));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "Please enter a valid Email Address"
                                                    && x.ErrorCode == "RegularExpressionValidator");
        }
    }
}
