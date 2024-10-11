using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class CanadianHighSchoolValidatorTests
    {
        private readonly EducationBaseValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;
        private readonly DataFakerFixture _dataFakerFixture;

        public CanadianHighSchoolValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _validator = new EducationBaseValidator(XunitInjectionCollection.LookupsCache, new DomesticContextMock().Object);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_InCanadian()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,Highschool");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_InOntario()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,Ontario,Highschool");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldPass_InOntario_When_Update()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,Ontario,Highschool");
            model.InstituteId = Guid.NewGuid();

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetHighSchool(It.IsAny<Guid>(), It.IsAny<Domestic.Enums.Locale>())).ReturnsAsync(new Dto.HighSchool { Id = model.InstituteId.Value, ShowInEducation = true });

            var validator = new CanadianHighSchoolValidator(XunitInjectionCollection.LookupsCache, domesticContextMock.Object, Core.Enums.OperationType.Update);

            // Act
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
            domesticContextMock.Verify(e => e.GetHighSchool(It.Is<Guid>(p => p == model.InstituteId.Value), It.IsAny<Domestic.Enums.Locale>()), Times.Once);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_InOntario_NotShowInEducation()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,Ontario,Highschool");
            model.InstituteId = _dataFakerFixture.Faker.PickRandom(_modelFakerFixture.AllApplyLookups.HighSchools.Where(h => !h.ShowInEducation)).Id;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Institute Id' is not an Ontario high school: {model.InstituteId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenMissingFields_InCanadian()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,Highschool");
            model.ProvinceId = null;
            model.StudentNumber = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Province Id' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Student Number' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenInvalidProvince_InCanadian()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,Highschool");
            model.ProvinceId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Province Id' is not in Canada: {model.ProvinceId}");
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Province Id' province does not match City.ProvinceId: {model.ProvinceId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenNotCurrentlyAttending_FieldsMissing_InCanadian()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,Highschool");
            model.CurrentlyAttending = false;
            model.AttendedTo = _dataFakerFixture.Faker.Date.Between(model.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed), DateTime.UtcNow).ToString(Constants.DateFormat.YearMonthDashed);
            model.Graduated = null;
            model.LastGradeCompletedId = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Graduated' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Last Grade Completed Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenNotCurrentlyAttending_FieldsInvalid_InCanadian()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,Highschool");
            model.CurrentlyAttending = false;
            model.Graduated = _dataFakerFixture.Faker.Random.Bool();
            model.AttendedTo = _dataFakerFixture.Faker.Date.Between(model.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed), DateTime.UtcNow).ToString(Constants.DateFormat.YearMonthDashed);
            model.LastGradeCompletedId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Last Grade Completed Id' does not exist: {model.LastGradeCompletedId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_MissingInstitute_InOntario()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,Ontario,Highschool");
            model.InstituteId = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Institute Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_InvalidInstitute_InOntario()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Canadian,Ontario,Highschool");
            model.InstituteId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Institute Id' is not an Ontario high school: {model.InstituteId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_When_InvalidInstitute_InOntario_Update()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,Ontario,Highschool");
            model.InstituteId = Guid.NewGuid();

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetHighSchool(It.IsAny<Guid>(), It.IsAny<Domestic.Enums.Locale>())).ReturnsAsync((Dto.HighSchool)null);

            var validator = new CanadianHighSchoolValidator(XunitInjectionCollection.LookupsCache, domesticContextMock.Object, Core.Enums.OperationType.Update);

            // Act
            var result = await validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == $"'Institute Id' is not an Ontario high school: {model.InstituteId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenInstituteName_Empty()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,Highschool");
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
        public async Task Validator_ShouldFail_WhenInstituteName_TooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,Highschool");
            model.InstituteId = null;
            model.InstituteName = _dataFakerFixture.Faker.Random.String2(105);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"The length of 'Institute Name' must be 100 characters or fewer. You entered {model.InstituteName.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenInstituteName_NotIso8859()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate("default,Canadian,Highschool");
            model.InstituteId = null;
            model.InstituteName = ((char)0xCF80).ToString();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Institute Name' is not in the correct format.");
        }
    }
}
