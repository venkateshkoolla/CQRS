using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class ApplicantAddressValidatorTests
    {
        private readonly ApplicantAddressValidator _validator;
        private readonly ModelFakerFixture _modelFaker;
        private readonly Faker _faker;

        public ApplicantAddressValidatorTests()
        {
            _validator = new ApplicantAddressValidator(XunitInjectionCollection.LookupsCache);
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldPass()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldPass_Canada()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Can");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldPass_Usa()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Usa");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldPass_Intl()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Intl");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Street_Empty()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate();
            model.Street = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Street' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Street_TooLong()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate();
            model.Street = _faker.Random.String2(60);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == $"The length of 'Street' must be 50 characters or fewer. You entered {model.Street.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Street_BeIso8859()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate();
            model.Street = ((char)0xCF80).ToString();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Street' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_City_Empty()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate();
            model.City = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'City' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_City_TooLong()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate();
            model.City = _faker.Random.String2(60);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == $"The length of 'City' must be 30 characters or fewer. You entered {model.City.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_City_BeIso8859()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate();
            model.City = ((char)0xCF80).ToString();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'City' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_CountryId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate();
            model.CountryId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.ContainSingle(x => x.ErrorMessage == "'Country Id' must not be empty.")
                .And.ContainSingle(x => x.ErrorMessage == $"'Country Id' does not exist: {model.CountryId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_PostalCode_Empty()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Intl");
            model.PostalCode = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Postal Code' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_PostalCode_TooLong()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Intl");
            model.PostalCode = _faker.Random.String2(60);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == $"The length of 'Postal Code' must be 10 characters or fewer. You entered {model.PostalCode.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_PostalCode_BeIso8859()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Intl");
            model.PostalCode = ((char)0xCF80).ToString();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Postal Code' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_CountryId_Invalid()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate();
            model.CountryId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.ContainSingle(x => x.ErrorMessage == $"'Country Id' does not exist: {model.CountryId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Canada_Then_ProvinceStateId_Required()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Can");
            model.ProvinceStateId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.ContainSingle(x => x.ErrorMessage == $"'Province State Id' is missing: {model.ProvinceStateId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Canada_Then_ProvinceStateId_NotExist()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Can");
            model.ProvinceStateId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Province State Id' does not exist: {model.ProvinceStateId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Canada_Then_ProvinceStateId_NotInCanada()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Can");
            model.ProvinceStateId = _faker.PickRandom(_modelFaker.AllApplyLookups.ProvinceStates.Where(x => x.CountryId != model.CountryId)).Id;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"ProvinceState.CountryId does not match Applicant.MailingAddress.CountryId: {model.CountryId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Usa_Then_ProvinceStateId_Required()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Usa");
            model.ProvinceStateId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.ContainSingle(x => x.ErrorMessage == $"'Province State Id' is missing: {model.ProvinceStateId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Usa_Then_ProvinceStateId_NotExist()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Usa");
            model.ProvinceStateId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Province State Id' does not exist: {model.ProvinceStateId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Usa_Then_ProvinceStateId_NotInUsa()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Usa");
            model.ProvinceStateId = _faker.PickRandom(_modelFaker.AllApplyLookups.ProvinceStates.Where(x => x.CountryId != model.CountryId)).Id;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"ProvinceState.CountryId does not match Applicant.MailingAddress.CountryId: {model.CountryId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Canada_PostalCode_Invalid()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Can");
            model.PostalCode = _faker.Address.ZipCode();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Postal Code' is invalid: {model.PostalCode}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task ApplicantAddressValidator_ShouldFail_When_Usa_PostalCode_Invalid()
        {
            // Arrange
            var model = _modelFaker.GetApplicantAddress().Generate("default, Usa");
            model.PostalCode = "A1A1A1";

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Postal Code' is invalid: {model.PostalCode}");
        }
    }
}
