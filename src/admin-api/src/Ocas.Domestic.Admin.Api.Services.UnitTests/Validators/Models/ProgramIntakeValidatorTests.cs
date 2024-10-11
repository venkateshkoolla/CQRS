using System;
using System.Linq;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class ProgramIntakeValidatorTests
    {
        private readonly ProgramIntakeValidator _validator;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly ProgramIntake _programIntake;
        private readonly Program _program;

        public ProgramIntakeValidatorTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _program = _models.GetProgram().Generate();
            _programIntake = _models.GetProgramIntake(_program).Generate();
            var entryLevels = _models.AllAdminLookups.EntryLevels;
            _validator = new ProgramIntakeValidator(XunitInjectionCollection.LookupsCache, entryLevels.First().Id);
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramIntakeValidator_ShouldPass()
        {
            var result = _validator.Validate(_programIntake);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramIntakeValidator_ShouldThrow_When_StartDate_Empty()
        {
            _programIntake.StartDate = string.Empty;
            var result = _validator.Validate(_programIntake);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Start Date' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramIntakeValidator_ShouldThrow_When_EnrolmentEstimate_Not_WithInRage()
        {
            _programIntake.EnrolmentEstimate = 100000;
            var result = _validator.Validate(_programIntake);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Enrolment Estimate' must be between 0 and 99999. You entered {_programIntake.EnrolmentEstimate}.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramIntakeValidator_ShouldThrow_When_EnrolmentMax_Not_WithInRage()
        {
            _programIntake.EnrolmentMax = 100000;
            var result = _validator.Validate(_programIntake);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Enrolment Max' must be between 0 and 99999. You entered {_programIntake.EnrolmentMax}.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramIntakeValidator_ShouldThrow_When_IntakeAvailabilityId_Empty()
        {
            _programIntake.IntakeAvailabilityId = Guid.Empty;
            var result = _validator.Validate(_programIntake);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Intake Availability Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramIntakeValidator_ShouldThrow_When_IntakeStatusId_Empty()
        {
            _programIntake.IntakeStatusId = Guid.Empty;
            var result = _validator.Validate(_programIntake);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "'Intake Status Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramIntakeValidator_ShouldThrow_When_ExpiryDate_Not_Valid()
        {
            _programIntake.ExpiryDate = _dataFakerFixture.Faker.Random.String2(4);
            var result = _validator.Validate(_programIntake);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == "The specified condition was not met for 'Expiry Date'.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramIntakeValidator_ShouldThrow_When_ExpiryDate_Valid_But_ExpiryActionId_Empty()
        {
            _programIntake.ExpiryDate = _dataFakerFixture.Faker.Date.Future(1, DateTime.UtcNow).ToStringOrDefault();
            _programIntake.IntakeExpiryActionId = Guid.Empty;
            var result = _validator.Validate(_programIntake);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(x => x.ErrorMessage == $"'Intake Expiry Action Id' must not be equal to '{Guid.Empty}'.");
        }
    }
}