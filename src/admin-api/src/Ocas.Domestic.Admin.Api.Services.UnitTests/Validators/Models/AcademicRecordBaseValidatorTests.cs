using System;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class AcademicRecordBaseValidatorTests
    {
        private readonly AcademicRecordBaseValidator _validator;
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;

        public AcademicRecordBaseValidatorTests()
        {
            _validator = new AcademicRecordBaseValidator(XunitInjectionCollection.LookupsCache);
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldPass()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldPass_When_DateCredentialAchieved_Empty()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.DateCredentialAchieved = string.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldPass_When_HighSkillsMajorId_Null()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.HighSkillsMajorId = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldPass_When_SchoolId_Null()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.SchoolId = null;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_ApplicantId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.ApplicantId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Applicant Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_DateCredentialAchieved_Invalid()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.DateCredentialAchieved = "NotADateTime";

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Date Credential Achieved' must be a valid date.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_DateCredentialAchieved_InFuture()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.DateCredentialAchieved = DateTime.UtcNow.AddYears(2).ToStringOrDefault();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Date Credential Achieved' must be in the past.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_SchoolId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.SchoolId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'School Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_SchoolId_MustBeAHighShool()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.SchoolId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == $"'School Id' does not exist: {model.SchoolId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_CommunityInvolvementId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.CommunityInvolvementId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Community Involvement Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_CommunityInvolvementId_MustExist()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.CommunityInvolvementId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == $"'Community Involvement Id' does not exist: {model.CommunityInvolvementId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_LiteracyTestId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.LiteracyTestId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Literacy Test Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_LiteracyTestId_MustExist()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.LiteracyTestId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == $"'Literacy Test Id' does not exist: {model.LiteracyTestId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_HighestEducationId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.HighestEducationId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'Highest Education Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_HighestEducationId_MustExist()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.HighestEducationId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == $"'Highest Education Id' does not exist: {model.HighestEducationId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_HighSkillsMajorId_Empty()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.HighSkillsMajorId = Guid.Empty;

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == "'High Skills Major Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public async Task AcademicRecordBaseValidator_ShouldThrow_When_HighSkillsMajorId_MustExist()
        {
            // Arrange
            var model = _modelFaker.GetAcademicRecordBase().Generate();
            model.HighSkillsMajorId = Guid.NewGuid();

            // Act
            var result = await _validator.ValidateAsync(model);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle()
                .And.Contain(x => x.ErrorMessage == $"'High Skills Major Id' does not exist: {model.HighSkillsMajorId}");
        }
    }
}
