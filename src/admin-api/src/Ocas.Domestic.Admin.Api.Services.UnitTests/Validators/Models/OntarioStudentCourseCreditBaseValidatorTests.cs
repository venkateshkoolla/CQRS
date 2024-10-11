using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class OntarioStudentCourseCreditBaseValidatorTests
    {
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;
        private readonly Faker _faker;
        private readonly OntarioStudentCourseCreditBaseValidator _validator;

        public OntarioStudentCourseCreditBaseValidatorTests()
        {
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _validator = new OntarioStudentCourseCreditBaseValidator(XunitInjectionCollection.LookupsCache, XunitInjectionCollection.DomesticContext);
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldPass()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldPass_When_GradeFinal()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate("default, GradeFinal");

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldPass_When_Notes_Empty()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.Notes = new List<string>();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldPass_When_Notes_Null()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.Notes = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_ApplicantId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.ApplicantId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Applicant Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CourseCode_Empty()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CourseCode = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Course Code' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CourseCode_NotExist()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CourseCode = _faker.Random.AlphaNumeric(5);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Course Code' must exist.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CourseCode_Not_Valid_Length()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CourseCode = _faker.Random.AlphaNumeric(4);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Course Code' must be between 5 and 6 characters. You entered 4 characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_Mark_TooLong()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.Grade = _faker.Random.AlphaNumeric(4);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.ContainSingle(x => x.ErrorMessage == "'Grade' is invalid.");
        }

        [Theory]
        [UnitTest("Validators")]
        [InlineData("100")]
        [InlineData("99")]
        [InlineData("0")]
        [InlineData(Constants.OntarioHighSchool.CourseGrade.NotApplicable)]
        [InlineData(Constants.OntarioHighSchool.CourseGrade.AlternativeCourse)]
        [InlineData(Constants.OntarioHighSchool.CourseGrade.InsufficientEvidence)]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldPass_When_Mark_Is_Valid(string mark)
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.Grade = mark;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_Credit_TooLarge()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.Credit = 100;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == $"'Credit' must be between 0 and 99.99. You entered {model.Credit}.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CourseMident_TooLong()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CourseMident = _faker.Random.AlphaNumeric(8);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == $"'Course Mident' must be 6 characters in length. You entered {model.CourseMident.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CompletedDate_Empty()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CompletedDate = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Completed Date' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CompletedDate_NotADate()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CompletedDate = _faker.Random.AlphaNumeric(6);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Completed Date' must be a valid date.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_GradeFinal_Then_CompletedDate_InFuture()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate("default, GradeFinal");
            model.CompletedDate = DateTime.UtcNow.AddMonths(6).ToString(Constants.DateFormat.YearMonthDashed);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Completed Date' is invalid.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CompletedDate_InPast()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CompletedDate = DateTime.UtcNow.AddMonths(-6).ToString(Constants.DateFormat.YearMonthDashed);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Completed Date' is invalid.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_GradeTypeId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.GradeTypeId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.ContainSingle(x => x.ErrorMessage == "'Grade Type Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_GradeTypeId_NotExists()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.GradeTypeId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.ContainSingle(x => x.ErrorMessage == "'Grade Type Id' must exist.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CourseStatusId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CourseStatusId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Course Status Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CourseStatusId_NotExists()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CourseStatusId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Course Status Id' must exist.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CourseDeliveryId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CourseDeliveryId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Course Delivery Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CourseDeliveryId_NotExists()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CourseDeliveryId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Course Delivery Id' must exist.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CourseTypeId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CourseTypeId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Course Type Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task OntarioStudentCourseCreditBaseValidator_ShouldFail_When_CourseTypeId_NotExists()
        {
            // Arrange
            var model = _modelFaker.GetOntarioStudentCourseCreditBase().Generate();
            model.CourseTypeId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeNullOrEmpty()
                .And.OnlyContain(x => x.ErrorMessage == "'Course Type Id' must exist.");
        }
    }
}
