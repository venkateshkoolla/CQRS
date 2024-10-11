using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Validators;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators
{
    public class OenValidatorTests
    {
        private readonly TestValidatorCollection _validator;
        private readonly ModelFakerFixture _modelFakerFixture;

        public OenValidatorTests()
        {
            _validator = new TestValidatorCollection(v =>
            v.RuleFor(o => o.GenericString)
                .OenValid());
            _modelFakerFixture = new ModelFakerFixture();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OenValidator_Should_Pass()
        {
            var result = await _validator.ValidateAsync(new TestObject(_modelFakerFixture.Oen));

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OenValidator_Should_Fail_When_InvalidChecksum()
        {
            var result = await _validator.ValidateAsync(new TestObject("123456789"));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Generic String' must match checksum.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OenValidator_Should_Fail_When_Empty()
        {
            var result = await _validator.ValidateAsync(new TestObject());

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Generic String' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OenValidator_Should_Fail_When_TooLong()
        {
            var result = await _validator.ValidateAsync(new TestObject("12345678912"));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Generic String' must be 9 characters in length. You entered 11 characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OenValidator_Should_Fail_When_NotANumber()
        {
            var result = await _validator.ValidateAsync(new TestObject("12 456 89"));

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Generic String' is not in the correct format.");
        }
    }
}
