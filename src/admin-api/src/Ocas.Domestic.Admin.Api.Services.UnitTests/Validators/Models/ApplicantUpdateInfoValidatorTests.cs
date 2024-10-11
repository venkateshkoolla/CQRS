using System;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class ApplicantUpdateInfoValidatorTests : IClassFixture<ApplicantUpdateInfoValidator>
    {
        private readonly ApplicantUpdateInfoValidator _validator;

        public ApplicantUpdateInfoValidatorTests(ApplicantUpdateInfoValidator validator)
        {
            _validator = validator;
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ApplicantUpdateInfoValidator_ShouldPass()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(50, DateTime.UtcNow.AddYears(-16)).ToUniversalTime().ToStringOrDefault())
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_FirstName_Regex()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(50, DateTime.UtcNow.AddYears(-16)).ToUniversalTime().ToStringOrDefault())
                .Generate();

            model.FirstName = "InvalidUnicode\u0028";

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == "'First Name' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_FirstName_Length()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(50, DateTime.UtcNow.AddYears(-16)).ToUniversalTime().ToStringOrDefault())
                .Generate();
            model.FirstName = "ThisIsFarTooLongToBeAFirstName32";

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == "The length of 'First Name' must be 30 characters or fewer. You entered 32 characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_FirstName_Empty()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(50, DateTime.UtcNow.AddYears(-16)).ToUniversalTime().ToStringOrDefault())
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == "'First Name' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_LastName_Regex()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(50, DateTime.UtcNow.AddYears(-16)).ToUniversalTime().ToStringOrDefault())
                .Generate();

            model.LastName = "InvalidUnicode\u0028";

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == "'Last Name' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_LastName_Length()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(50, DateTime.UtcNow.AddYears(-16)).ToUniversalTime().ToStringOrDefault())
                .Generate();
            model.LastName = "ThisIsFarTooLongToBeALastNamex32";

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == "The length of 'Last Name' must be 30 characters or fewer. You entered 32 characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_LastName_Empty()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(50, DateTime.UtcNow.AddYears(-16)).ToUniversalTime().ToStringOrDefault())
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == "'Last Name' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_BirthDate_Invalid()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.BirthDate, _ => "NotADateTime")
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == $"'Birth Date' is not valid: {model.BirthDate}");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_BirthDate_InFuture()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.BirthDate, f => f.Date.Future().AsUtc().ToStringOrDefault())
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == $"'Birth Date' can not be in future: {model.BirthDate}");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_BirthDate_Empty()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.BirthDate, string.Empty)
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == "'Birth Date' must not be empty.");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_BirthDate_Beyond_Range()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(1, DateTime.UtcNow.AddYears(-90)).ToUniversalTime().ToStringOrDefault())
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == $"Age must be within 16 and 90: {model.BirthDate}");
        }

        [Fact]
        [UnitTest("Validator")]
        public async Task ApplicantUpdateInfoValidator_ShouldFail_With_BirthDate_Below_Range()
        {
            var model = new Faker<ApplicantUpdateInfo>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.BirthDate, f => f.Date.Past(1, DateTime.UtcNow.AddYears(-15)).ToUniversalTime().ToStringOrDefault())
                .Generate();

            var result = await _validator.ValidateAsync(model);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle().And.ContainSingle(x => x.ErrorMessage == $"Age must be within 16 and 90: {model.BirthDate}");
        }
    }
}
