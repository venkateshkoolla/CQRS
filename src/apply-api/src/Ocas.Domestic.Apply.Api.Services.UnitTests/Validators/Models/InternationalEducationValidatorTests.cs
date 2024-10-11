using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Enums;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Validators.Models
{
    public class InternationalEducationValidatorTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly EducationBaseValidator _validator;
        private readonly ModelFakerFixture _modelFakerFixture;

        public InternationalEducationValidatorTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _validator = new EducationBaseValidator(_lookupsCache, new DomesticContextMock().Object);
            _modelFakerFixture = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task InternationalEducationValidator_ShouldPass_WhenInternational()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default, Intl");

            // Act
            var result = await _validator.ValidateAsync(model);
            var educationType = model.GetEducationType(_modelFakerFixture.AllApplyLookups.Countries, _modelFakerFixture.AllApplyLookups.InstituteTypes);

            // Assert
            educationType.Should().Be(EducationType.International);
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task InternationalEducationValidator_ShouldFail_WhenMissingRequiredFields_InternationalHighschool()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Intl,Highschool");
            model.AttendedFrom = null;
            model.CurrentlyAttending = null;
            model.InstituteName = null;
            model.LevelAchievedId = null;
            model.Major = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(4);
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Attended From' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Currently Attending' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Institute Name' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Level Achieved Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task InternationalEducationValidator_ShouldFail_WhenMissingRequiredFields_InternationalPostSecondary()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Intl,PostSecondary");
            model.AttendedFrom = null;
            model.CredentialId = null;
            model.CurrentlyAttending = null;
            model.InstituteName = null;
            model.LevelAchievedId = null;
            model.Major = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(6);
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Attended From' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Credential Id' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Currently Attending' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Institute Name' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Level Achieved Id' must not be empty.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Major' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task InternationalEducationValidator_ShouldFail_WhenInvalidRequiredFields_InternationalPostSecondary()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default,Intl,PostSecondary");
            model.AttendedFrom = "ASDFghj";
            model.CredentialId = Guid.NewGuid();
            model.LevelAchievedId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(3);
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Attended From' is not a date: {model.AttendedFrom}");
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Credential Id' does not exist: {model.CredentialId}");
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Level Achieved Id' does not exist: {model.LevelAchievedId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task InternationalEducationValidator_ShouldFail_WhenMissingRequiredFields_AttendedTo_International()
        {
            // Arrange
            var faker = _modelFakerFixture.GetEducationBase();
            var model = faker.Generate("default, Intl");
            model.AttendedTo = null;
            model.CurrentlyAttending = false;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Attended To' must not be empty.");
        }
    }
}
