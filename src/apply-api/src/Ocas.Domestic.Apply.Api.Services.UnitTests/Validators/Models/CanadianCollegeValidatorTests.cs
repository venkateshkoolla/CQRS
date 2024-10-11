using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class CanadianCollegeValidatorTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly EducationBaseValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly DataFakerFixture _dataFakerFixture;

        public CanadianCollegeValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _validator = new EducationBaseValidator(_lookupsCache, new DomesticContextMock().Object);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldPass_WhenCanadianCollege()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,College");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldPass_WhenCanadianCollege_Update()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,College");
            model.InstituteId = Guid.NewGuid();

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetCollege(It.IsAny<Guid>())).ReturnsAsync(new Dto.College { Id = model.InstituteId.Value });

            var validator = new CanadianCollegeValidator(_lookupsCache, domesticContextMock.Object, OperationType.Update);

            // Act
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
            domesticContextMock.Verify(e => e.GetCollege(It.Is<Guid>(p => p == model.InstituteId.Value)), Times.Once);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenMissingFields_InCanadianCollege()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,College");
            model.CredentialId = null;
            model.CurrentlyAttending = null;
            model.FirstNameOnRecord = string.Empty;
            model.InstituteId = null;
            model.InstituteName = string.Empty;
            model.LastNameOnRecord = string.Empty;
            model.StudentNumber = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Credential Id' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Currently Attending' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Institute Name' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenMissingFields_InOntarioCollege()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,Ontario,College");
            model.CredentialId = null;
            model.CurrentlyAttending = null;
            model.FirstNameOnRecord = string.Empty;
            model.InstituteId = null;
            model.InstituteName = string.Empty;
            model.LastNameOnRecord = string.Empty;
            model.StudentNumber = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(4);
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Credential Id' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Currently Attending' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Institute Name' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Student Number' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenState_InCanadianCollege()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,College");
            var provinceStates = await _lookupsCache.GetProvinceStates(TestConstants.Locale.English);
            var countries = await _lookupsCache.GetCountries(TestConstants.Locale.English);
            var canada = countries.Single(x => x.Code == Constants.Countries.Canada);
            var states = provinceStates.Where(x => x.CountryId != canada.Id).ToList();
            model.ProvinceId = _dataFakerFixture.Faker.PickRandom(states).Id;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Province Id' is not in Canada: {model.ProvinceId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenOtherCredential_InCanadianCollege()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,College");
            var credentials = await _lookupsCache.GetCredentials(TestConstants.Locale.English);
            model.CredentialId = _dataFakerFixture.Faker.PickRandom(credentials.Where(c => c.Code == Constants.Credentials.Other).ToList()).Id;
            model.OtherCredential = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Other Credential' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenInstituteTypeDoesNotMatch_InCanadianCollege()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,College");
            var universities = await _lookupsCache.GetUniversities();
            model.InstituteId = _dataFakerFixture.Faker.PickRandom(universities).Id;
            model.InstituteName = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Institute Id' is not an Ontario college: {model.InstituteId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenInstituteName_Empty()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,College");
            model.InstituteId = null;
            model.InstituteName = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Institute Name' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenInstituteName_TooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,College");
            model.InstituteId = null;
            model.InstituteName = _dataFakerFixture.Faker.Random.String2(75);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"The length of 'Institute Name' must be 60 characters or fewer. You entered {model.InstituteName.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenInstituteName_NotIso8859()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,College");
            model.InstituteId = null;
            model.InstituteName = ((char)0xCF80).ToString();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Institute Name' is not in the correct format.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenNotCollege_InCanadianCollege_Update()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,College");
            model.InstituteId = Guid.NewGuid();

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetCollege(It.IsAny<Guid>())).ReturnsAsync((Dto.College)null);

            var validator = new CanadianCollegeValidator(_lookupsCache, domesticContextMock.Object, OperationType.Update);

            // Act
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.ContainSingle(x => x.ErrorMessage == $"'Institute Id' is not an Ontario college: {model.InstituteId}");
            domesticContextMock.Verify(e => e.GetCollege(It.Is<Guid>(p => p == model.InstituteId.Value)), Times.Once);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenMajor_Empty()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,College");
            model.Major = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Major' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenMajor_TooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,College");
            model.Major = _dataFakerFixture.Faker.Random.String2(75);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == $"The length of 'Major' must be 60 characters or fewer. You entered {model.Major.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task CanadianCollegeValidator_ShouldFail_WhenMajor_NotIso8859()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,College");
            model.Major = ((char)0xCF80).ToString();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().OnlyContain(x => x.ErrorMessage == "'Major' is not in the correct format.");
        }
    }
}
