using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class EducationBaseValidatorTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly EducationBaseValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;

        public EducationBaseValidatorTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _validator = new EducationBaseValidator(_lookupsCache, new DomesticContextMock().Object);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenDatesNotFormatted()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate();
            model.CurrentlyAttending = false;
            model.AttendedFrom = "abcd-ef";
            model.AttendedTo = "abcd-ef";

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Attended From' is not a date: {model.AttendedFrom}");
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Attended To' is not a date: {model.AttendedTo}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenAttendedFromInFuture()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate();
            model.CurrentlyAttending = true;
            model.AttendedFrom = DateTime.UtcNow.AddMonths(1).ToString(Constants.DateFormat.YearMonthDashed);
            model.AttendedTo = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Attended From' must be in the past: {model.AttendedFrom}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenDatesTooLong()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate();
            model.CurrentlyAttending = false;
            model.AttendedFrom = model.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).ToString(Constants.DateFormat.YearMonthDay);
            model.AttendedTo = model.AttendedFrom;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain(x => x.ErrorMessage.StartsWith("'Attended From' must be 7 characters in length"));
            result.Errors.Should().Contain(x => x.ErrorMessage.StartsWith("'Attended To' must be 7 characters in length"));
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenAttendToBeforeAttendedFrom()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate();

            model.CurrentlyAttending = false;
            var attendedFrom = model.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed);
            var attendedTo = model.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed).AddMonths(-1);

            model.AttendedFrom = attendedFrom.ToString(Constants.DateFormat.YearMonthDashed);
            model.AttendedTo = attendedTo.ToString(Constants.DateFormat.YearMonthDashed);

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Attended To' must be greater than 'Attended From': {model.AttendedTo} >= {model.AttendedFrom}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenMissingRequiredFields_InGeneral()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate();
            model.AcademicUpgrade = null;
            model.ApplicantId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Applicant Id' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Academic Upgrade' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task Validator_ShouldFail_WhenLookupsDoNotExist_InGeneral()
        {
            // Arrange
            var model = _modelFakerFixture.GetEducationBase().Generate();
            model.AcademicUpgrade = false;
            model.CountryId = Guid.NewGuid();
            model.CredentialId = Guid.NewGuid();
            model.InstituteTypeId = Guid.NewGuid();
            model.LevelAchievedId = Guid.NewGuid();
            model.ProvinceId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Country Id' does not exist: {model.CountryId}");
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Institute Type Id' does not exist: {model.InstituteTypeId}");
            result.Errors.Should().Contain(x => x.ErrorMessage == "Could not determine education type");
        }
    }
}
