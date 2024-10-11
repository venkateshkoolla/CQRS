using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class ApplicantValidatorTests
    {
        private readonly ApplicantValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;

        public ApplicantValidatorTests()
        {
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
            _validator = new ApplicantValidator(XunitInjectionCollection.LookupsCache);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_MailingAddress_Null()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate();
            model.MailingAddress = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors[0].ErrorMessage.Should().Be("'Mailing Address' must not be empty.");
            result.Errors[1].ErrorMessage.Should().Be("'Mailing Address' is null");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_Fields_Missing()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicant().Generate("default,Can");
            model.BirthDate = string.Empty;
            model.Email = string.Empty;
            model.FirstName = string.Empty;
            model.HomePhone = string.Empty;
            model.LastName = string.Empty;
            model.MobilePhone = string.Empty;
            model.UserName = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(7)
                .And.ContainSingle(x => x.ErrorMessage == "'Email' must not be empty.")
                .And.ContainSingle(x => x.ErrorMessage == "'Home Phone' must not be empty.")
                .And.ContainSingle(x => x.ErrorMessage == "'Mobile Phone' must not be empty.")
                .And.ContainSingle(x => x.ErrorMessage == "'First Name' must not be empty.")
                .And.ContainSingle(x => x.ErrorMessage == "'Last Name' must not be empty.")
                .And.ContainSingle(x => x.ErrorMessage == "'Birth Date' must not be empty.")
                .And.ContainSingle(x => x.ErrorMessage == "'User Name' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_When_Mobile_Phone_Only()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicant().Generate("default,Can");
            model.MobilePhone = "5195555558";
            model.HomePhone = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_Canada_Phone_Invalid()
        {
            // Arrange
            const string tooLong = "51955512340";
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Can");

            model.HomePhone = tooLong;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Home Phone' is invalid: {tooLong}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_Usa_Phone_Invalid()
        {
            // Arrange
            const string tooLong = "51955512340";
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Usa");

            model.HomePhone = tooLong;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Home Phone' is invalid: {tooLong}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_Intl_Phone_Invalid()
        {
            // Arrange
            const string invalidChars = "5195551234x1234";
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Intl");

            model.HomePhone = invalidChars;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Home Phone' is invalid: {invalidChars}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_Intl_Phone_TooLong()
        {
            // Arrange
            const string invalidChars = "5195551234123456";
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Intl");

            model.HomePhone = invalidChars;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Home Phone' is invalid: {invalidChars}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_Intl_Phone_TooShort()
        {
            // Arrange
            const string invalidChars = "5551234";
            var model = _modelFakerFixture.GetApplicant().Generate("default,Intl");
            model.HomePhone = invalidChars;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Home Phone' is invalid: {invalidChars}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_ArrivalDate_Invalid_Format()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Intl");
            var birthDate = model.BirthDate.ToDateTime();
            birthDate = birthDate.AddDays(-1);
            model.DateOfArrival = birthDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Date Of Arrival' must be a valid date 'yyyy-MM-dd'");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_ArrivalDate_Canadian_Invalid_Format()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Can");
            var birthDate = model.BirthDate.ToDateTime();
            birthDate = birthDate.AddDays(-1);
            model.DateOfArrival = birthDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Date Of Arrival' must be a valid date 'yyyy-MM-dd'");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_When_ArrivalDate_Empty_And_Canadian()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Can");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            model.DateOfArrival.Should().BeNullOrEmpty();
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_When_Title_Empty()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Intl");
            model.TitleId = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_Title_Invalid()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Intl");
            model.TitleId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Title Id' does not exist: {model.TitleId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_OtherAboriginalStatus_Empty()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicant().Generate("default,Intl");
            model.IsAboriginalPerson = true;
            model.AboriginalStatuses = _modelFakerFixture.AllApplyLookups.AboriginalStatuses.Where(a => a.Code == Constants.AboriginalStatuses.Other).Select(a => a.Id).ToList();
            model.OtherAboriginalStatus = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Other Aboriginal Status' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_OtherAboriginalStatus_TooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicant().Generate("default,Intl");
            model.IsAboriginalPerson = true;
            model.AboriginalStatuses = _modelFakerFixture.AllApplyLookups.AboriginalStatuses.Where(a => a.Code == Constants.AboriginalStatuses.Other).Select(a => a.Id).ToList();
            model.OtherAboriginalStatus = "asdfasdfasdfasdfasdfasdfasdfasdf";

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"The length of 'Other Aboriginal Status' must be 30 characters or fewer. You entered {model.OtherAboriginalStatus.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_OtherAboriginalStatus_NotIso8859()
        {
            // Arrange
            var model = _modelFakerFixture.GetApplicant().Generate("default,Intl");
            model.IsAboriginalPerson = true;
            model.AboriginalStatuses = _modelFakerFixture.AllApplyLookups.AboriginalStatuses.Where(a => a.Code == Constants.AboriginalStatuses.Other).Select(a => a.Id).ToList();
            model.OtherAboriginalStatus = ((char)0xCF80).ToString();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Other Aboriginal Status' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_PreferredLanguage_Invalid()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Intl");
            model.PreferredLanguageId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Preferred Language Id' does not exist: {model.PreferredLanguageId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_When_StatusInCanada_International()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Intl");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_When_StatusInCanada_CanadianCitizen()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Can");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            var canadianCitizenId = _modelFakerFixture.AllApplyLookups.CanadianStatuses.Single(x => x.Code == Constants.CanadianStatuses.CanadianCitizen).Id;
            model.StatusInCanadaId.Should().Be(canadianCitizenId);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_StatusInCanada_Invalid()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Intl");
            model.StatusInCanadaId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Status In Canada Id' does not exist: {model.StatusInCanadaId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_With_SponsorAgencyId()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Intl");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
            model.SponsorAgencyId.Should().NotBeEmpty();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_SponsorAgencyId_Invalid()
        {
            // Arrange
            var faker = _modelFakerFixture.GetApplicant();
            var model = faker.Generate("default,Intl");
            model.SponsorAgencyId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Sponsor Agency Id' does not exist: {model.SponsorAgencyId}");
        }
    }
}
