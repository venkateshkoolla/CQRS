using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public class DtoMapperTests
    {
        private readonly IDtoMapper _dtoMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ModelFakerFixture _models;

        public DtoMapperTests()
        {
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task PatchEducation_Should_Patch_InternationalEdu()
        {
            // Arrange
            var eduModel = _models.GetEducationBase().Generate("default, Intl");
            var eduDto = new Dto.EducationBase();

            //Act
            await _dtoMapper.PatchEducation(eduDto, eduModel);

            //Assert
            eduDto.Should().BeEquivalentTo(eduModel, opt => opt
                .Excluding(z => z.TranscriptFee)
                .Excluding(z => z.Address));
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task PatchEducation_Should_NotPatchFields_InternationalEdu()
        {
            // Arrange
            var eduModel = _models.GetEducationBase().Generate("default, Intl");
            var eduDto = new Dto.EducationBase
            {
                //Cannot be overwritten
                AcademicUpgrade = null,
                CityId = Guid.NewGuid(),
                FirstNameOnRecord = _dataFakerFixture.Faker.Person.FirstName,
                InstituteId = Guid.NewGuid(),
                LastGradeCompletedId = Guid.NewGuid(),
                LastNameOnRecord = _dataFakerFixture.Faker.Person.LastName,
                LevelOfStudiesId = Guid.NewGuid(),
                OntarioEducationNumber = _dataFakerFixture.Faker.Random.String(9, '0', '9'),
                OtherCredential = _dataFakerFixture.Faker.Name.JobTitle(),
                ProvinceId = Guid.NewGuid(),
                StudentNumber = _dataFakerFixture.Faker.Random.AlphaNumeric(9)
            };

            //Act
            await _dtoMapper.PatchEducation(eduDto, eduModel);

            //Assert
            eduDto.Should().BeEquivalentTo(eduModel, opts => opts
                .Excluding(z => z.AcademicUpgrade)
                .Excluding(z => z.CityId)
                .Excluding(z => z.FirstNameOnRecord)
                .Excluding(z => z.InstituteId)
                .Excluding(z => z.LastGradeCompletedId)
                .Excluding(z => z.LastNameOnRecord)
                .Excluding(z => z.LevelOfStudiesId)
                .Excluding(z => z.OtherCredential)
                .Excluding(z => z.ProvinceId)
                .Excluding(z => z.StudentNumber)
                .Excluding(z => z.TranscriptFee)
                .Excluding(z => z.Address));
            eduDto.AcademicUpgrade.Should().BeFalse();
            eduDto.CityId.Should().BeNull();
            eduDto.FirstNameOnRecord.Should().BeNull();
            eduDto.InstituteId.Should().BeNull();
            eduDto.LastGradeCompletedId.Should().BeNull();
            eduDto.LastNameOnRecord.Should().BeNull();
            eduDto.LevelOfStudiesId.Should().BeNull();
            eduDto.OtherCredential.Should().BeNull();
            eduDto.ProvinceId.Should().BeNull();
            eduDto.StudentNumber.Should().BeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task PatchEducation_Should_NotPatchFields_InternationalHighschoolEdu()
        {
            // Arrange
            var eduModel = _models.GetEducationBase().Generate("default, Intl, Highschool");
            eduModel.Major = _dataFakerFixture.Faker.Name.JobTitle();
            eduModel.CredentialId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Credentials).Id;

            var eduDto = new Dto.EducationBase();

            //Act
            await _dtoMapper.PatchEducation(eduDto, eduModel);

            //Assert
            eduDto.Should().BeEquivalentTo(eduModel, opts => opts
                .Excluding(z => z.Major)
                .Excluding(z => z.CredentialId)
                .Excluding(z => z.TranscriptFee)
                .Excluding(z => z.Address));
            eduDto.Major.Should().BeNull();
            eduDto.CredentialId.Should().BeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task PatchEducation_Should_Patch_CanadianHighSchool()
        {
            // Arrange
            var eduModel = _models.GetEducationBase().Generate("default, Canadian, Highschool");
            var eduDto = new Dto.EducationBase();

            //Act
            await _dtoMapper.PatchEducation(eduDto, eduModel);

            //Assert
            eduDto.Should().BeEquivalentTo(eduModel, opt => opt
                .Excluding(z => z.TranscriptFee)
                .Excluding(z => z.Address));
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task PatchEducation_Should_Patch_OntarioCanadianHighSchool()
        {
            // Arrange
            var eduModel = _models.GetEducationBase().Generate("default, Canadian, Ontario, Highschool");
            var eduDto = new Dto.EducationBase();

            //Act
            await _dtoMapper.PatchEducation(eduDto, eduModel);

            //Assert
            eduDto.Should().BeEquivalentTo(eduModel, opt =>
                opt.Excluding(z => z.CityId)
                .Excluding(z => z.TranscriptFee)
                .Excluding(z => z.Address));
            eduDto.CityId.Should().Be(_models.AllApplyLookups.Cities.FirstOrDefault(c =>
                c.Name == _models.AllApplyLookups.HighSchools.FirstOrDefault(x => x.Id == eduModel.InstituteId).Address.City
                && c.ProvinceId == eduDto.ProvinceId).Id);
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task PatchEducation_Should_NotPatchFields_CanadianHighSchool()
        {
            // Arrange
            var eduModel = _models.GetEducationBase().Generate("default, Canadian, Highschool");
            eduModel.Major = _dataFakerFixture.Faker.Name.JobTitle();
            eduModel.CredentialId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Credentials).Id;
            eduModel.LevelOfStudiesId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.StudyLevels).Id;

            var eduDto = new Dto.EducationBase();

            //Act
            await _dtoMapper.PatchEducation(eduDto, eduModel);

            //Assert
            eduDto.Should().BeEquivalentTo(eduModel, opts => opts
                .Excluding(z => z.Major)
                .Excluding(z => z.CredentialId)
                .Excluding(z => z.LevelOfStudiesId)
                .Excluding(z => z.AcademicUpgrade)
                .Excluding(z => z.TranscriptFee)
                .Excluding(z => z.Address));
            eduDto.Major.Should().BeNull();
            eduDto.CredentialId.Should().BeNull();
            eduDto.LevelOfStudiesId.Should().BeNull();
            eduDto.AcademicUpgrade.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task PatchEducation_Should_Patch_CanadianHighSchool_WhenCurrentlyAttending()
        {
            // Arrange
            var eduModel = _models.GetEducationBase().Generate("default, Canadian, Highschool");
            eduModel.CurrentlyAttending = true;
            var eduDto = new Dto.EducationBase();

            //Act
            await _dtoMapper.PatchEducation(eduDto, eduModel);

            //Assert
            eduDto.Should().BeEquivalentTo(eduModel, opts => opts
                .Excluding(z => z.AttendedTo)
                .Excluding(z => z.LevelAchievedId)
                .Excluding(z => z.Graduated)
                .Excluding(z => z.TranscriptFee)
                .Excluding(z => z.Address));
            eduDto.AttendedTo.Should().BeNull();
            eduDto.LevelAchievedId.Should().BeNull();
            eduDto.Graduated.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task PatchEducation_Should_Patch_CanadianHighSchool_WhenNotCurrentlyAttending()
        {
            // Arrange
            var eduModel = _models.GetEducationBase().Generate("default, Canadian, Highschool");
            eduModel.CurrentlyAttending = false;
            eduModel.AttendedTo = _dataFakerFixture.Faker.Date.Between(eduModel.AttendedFrom.ToDateTime(Constants.DateFormat.YearMonthDashed), DateTime.UtcNow).ToString(Constants.DateFormat.YearMonthDashed);
            var eduDto = new Dto.EducationBase();

            //Act
            await _dtoMapper.PatchEducation(eduDto, eduModel);

            //Assert
            eduDto.Should().BeEquivalentTo(eduModel, opts => opts
                .Excluding(z => z.AttendedTo)
                .Excluding(z => z.TranscriptFee)
                .Excluding(z => z.Address));
            eduDto.AttendedTo.Should().Be(eduModel.AttendedTo);
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task PatchEducation_Should_Patch_CanadianHighSchool_WhenInstituteId()
        {
            // Arrange
            var eduModel = _models.GetEducationBase().Generate("default, Canadian, Highschool");
            var highSchool = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.HighSchools);
            eduModel.InstituteId = highSchool.Id;
            var eduDto = new Dto.EducationBase();

            //Act
            await _dtoMapper.PatchEducation(eduDto, eduModel);

            //Assert
            eduDto.Should().BeEquivalentTo(eduModel, opts => opts
                .Excluding(z => z.InstituteName)
                .Excluding(z => z.InstituteId)
                .Excluding(z => z.TranscriptFee)
                .Excluding(z => z.Address));
            eduDto.InstituteId.Should().Be(highSchool.Id);
            eduDto.InstituteName.Should().Be(highSchool.Name);
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task PatchEducation_Should_Patch_CanadianHighSchool_WhenNoInstituteId()
        {
            // Arrange
            var eduModel = _models.GetEducationBase().Generate("default, Canadian, Highschool");
            eduModel.InstituteId = null;
            eduModel.InstituteName = _dataFakerFixture.Faker.Name.JobArea();

            var eduDto = new Dto.EducationBase();

            //Act
            await _dtoMapper.PatchEducation(eduDto, eduModel);

            //Assert
            eduDto.Should().BeEquivalentTo(eduModel, opts => opts
                .Excluding(z => z.InstituteName)
                .Excluding(z => z.InstituteId)
                .Excluding(z => z.TranscriptFee)
                .Excluding(z => z.Address));
            eduDto.InstituteId.Should().BeNull();
            eduDto.InstituteName.Should().Be(eduModel.InstituteName);
        }
    }
}
