using System;
using System.Linq;
using FluentAssertions;
using Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Validators.Models
{
    public class ProgramBaseValidatorTests
    {
        private readonly ProgramBaseValidator _validator;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly Program _program;

        public ProgramBaseValidatorTests()
        {
            var domesticContextMock = new DomesticContextMock();
            _validator = new ProgramBaseValidator(XunitInjectionCollection.LookupsCache, domesticContextMock.Object);
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _program = _models.GetProgram().Generate();
            _program.Intakes = _models.GetProgramIntake(_program).Generate(3);
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldPass()
        {
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_CollegeId_NotExists()
        {
            _program.CollegeId = Guid.NewGuid();

            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'College Id' does not exist: {_program.CollegeId}");
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Campus Id' {_program.CampusId} does not exist for CollegeId: {_program.CollegeId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_CampusId_NotExists()
        {
            _program.CampusId = Guid.NewGuid();

            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Campus Id' {_program.CampusId} does not exist for CollegeId: {_program.CollegeId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_DeliveryId_NotExists()
        {
            _program.DeliveryId = Guid.NewGuid();

            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Delivery Id' does not exist: {_program.DeliveryId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_ProgramTypeId_NotExists()
        {
            _program.ProgramTypeId = Guid.NewGuid();

            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Program Type Id' does not exist: {_program.ProgramTypeId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_LengthTypeId_NotExists()
        {
            _program.LengthTypeId = Guid.NewGuid();

            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Length Type Id' does not exist: {_program.LengthTypeId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_CredentialId_NotExists()
        {
            _program.CredentialId = Guid.NewGuid();

            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Credential Id' does not exist: {_program.CredentialId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_EntryLevel_LowerThan_Default()
        {
            var entryLevels = _models.AllAdminLookups.EntryLevels;

            _program.DefaultEntryLevelId = entryLevels.ElementAt(2).Id;
            _program.EntryLevelIds = entryLevels.Select(x => x.Id).ToList();

            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Please don't select entry level lower then default");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_DefaultEntry_NotIn_EntryLevels()
        {
            var entryLevels = _models.AllAdminLookups.EntryLevels;

            _program.DefaultEntryLevelId = Guid.NewGuid();
            _program.EntryLevelIds = entryLevels.Select(x => x.Id).ToList();

            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Default Entry Level Id' should be included in list of entry levels");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_StudyArea_NotExists()
        {
            _program.StudyAreaId = Guid.NewGuid();
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Study Area Id' does not exist: {_program.StudyAreaId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_HighlyCompetetiveId_NotExists()
        {
            _program.HighlyCompetitiveId = Guid.NewGuid();
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Highly Competitive Id' does not exist: {_program.HighlyCompetitiveId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_LanguageId_NotExists()
        {
            _program.LanguageId = Guid.NewGuid();
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Language Id' does not exist: {_program.LanguageId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_LevelId_NotExists()
        {
            _program.LevelId = Guid.NewGuid();
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Level Id' does not exist: {_program.LevelId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_McuCode_NotExists()
        {
            _program.McuCode = _dataFakerFixture.Faker.Random.String2(4);
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Mcu Code' does not exist: {_program.McuCode}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_SpecialCode_NotExists()
        {
            _program.SpecialCode = _dataFakerFixture.Faker.Random.String2(4);
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Special Code' does not exist: {_program.SpecialCode}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_MinistryApprovalId_NotExists()
        {
            _program.MinistryApprovalId = Guid.NewGuid();
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Ministry Approval Id' does not exist: {_program.MinistryApprovalId}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_Url_Invalid()
        {
            _program.Url = _dataFakerFixture.Faker.Internet.Random.AlphaNumeric(100);
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Please enter a valid website");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_Url_Invalid_Length()
        {
            _program.Url = _dataFakerFixture.Faker.Internet.Random.AlphaNumeric(1001);
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"The length of 'Url' must be 1000 characters or fewer. You entered {_program.Url.Length} characters.");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_ProgramCategory1Id_NotExists()
        {
            _program.ProgramCategory1Id = Guid.NewGuid();
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Program Category1 Id' does not exist: {_program.ProgramCategory1Id}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_ProgramSubCategory1Id_NotExists()
        {
            _program.ProgramSubCategory1Id = Guid.NewGuid();
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Program Sub Category1 Id' {_program.ProgramSubCategory1Id} does not exist for ProgramCategory1Id: {_program.ProgramCategory1Id}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_ProgramCategory2Id_NotExists()
        {
            _program.ProgramCategory2Id = Guid.NewGuid();
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Program Category2 Id' does not exist: {_program.ProgramCategory2Id}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_ProgramSubCategory2Id_NotExists()
        {
            _program.ProgramSubCategory2Id = Guid.NewGuid();
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == $"'Program Sub Category2 Id' {_program.ProgramSubCategory2Id} does not exist for ProgramCategory2Id: {_program.ProgramCategory2Id}");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_Intakes_HaveSameStartDate()
        {
            _program.Intakes[0].StartDate = _program.Intakes[1].StartDate;
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Start dates must be unique for intakes (one per month)");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_Intakes_MoreThanTwelve()
        {
            _program.Intakes = _models.GetProgramIntake(_program).Generate(12);
            _program.Intakes.Add(new ProgramIntake { StartDate = "2512", IntakeAvailabilityId = Guid.NewGuid(), IntakeStatusId = Guid.NewGuid() });

            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "'Intakes' cannot be more than 12 (one per month)");
        }

        [Fact]
        [UnitTest("Validators")]
        public void ProgramBaseValidator_ShouldThrow_When_Intakes_StartDate_Must_WithIn_ApplicationCycles()
        {
            _dataFakerFixture.Faker.PickRandom(_program.Intakes).StartDate = _dataFakerFixture.Faker.Random.String2(4);
            var result = _validator.Validate(_program);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Start dates must be within the application cycle");
        }
    }
}
