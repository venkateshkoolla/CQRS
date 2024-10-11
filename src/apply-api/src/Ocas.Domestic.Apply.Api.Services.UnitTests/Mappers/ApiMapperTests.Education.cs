using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;
using DtoEnums = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_CollegeEmptyFee()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.College);
            var colleges = _models.AllApplyLookups.Colleges.Where(c => c.HasEtms && c.TranscriptFee == null).ToList();

            var collegeId = Guid.NewGuid();

            if (!colleges.Any())
            {
                colleges.Add(
                    new College
                    {
                        Id = collegeId,
                        HasEtms = true,
                        TranscriptFee = null,
                        Address = _models.GetMailingAddress().Generate()
                    });
            }
            else
            {
                collegeId = _dataFakerFixture.Faker.PickRandom(colleges).Id;
            }

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = collegeId
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().Be(0);
            education.Address.Should().NotBeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_CollegeWithEtms()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.College);
            var colleges = _models.AllApplyLookups.Colleges.Where(c => c.HasEtms && c.TranscriptFee > 0);
            var college = _dataFakerFixture.Faker.PickRandom(colleges);

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = college.Id
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().Be(college.TranscriptFee);
            education.Address.Should().NotBeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_CollegeWithoutEtms()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.College);
            var colleges = _models.AllApplyLookups.Colleges.Where(c => !c.HasEtms);
            var college = _dataFakerFixture.Faker.PickRandom(colleges);

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = college.Id
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().BeNull();
            education.Address.Should().NotBeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_CollegeInactive()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.College);
            var colleges = new List<College>() as IList<College>;
            var college = new Dto.College
            {
                Id = Guid.NewGuid(),
                HasEtms = true,
                TranscriptFee = _dataFakerFixture.Faker.Finance.Amount(),
                MailingAddress = new Dto.Address
                {
                    Street = _dataFakerFixture.Faker.Address.StreetAddress()
                }
            };

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = college.Id
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetCollege(It.IsAny<Guid>())).ReturnsAsync(college);

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, domesticContext.Object);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().Be(college.TranscriptFee);
            education.Address.Should().NotBeNull();
            domesticContext.Verify(a => a.GetCollege(It.Is<Guid>(p => p == college.Id)), Times.Once);
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_HighSchoolEmptyFee()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.HighSchool);
            var highSchools = _models.AllApplyLookups.HighSchools.Where(c => c.HasEtms && c.TranscriptFee == null).ToList();

            var highSchoolId = Guid.NewGuid();

            if (!highSchools.Any())
            {
                highSchools.Add(
                new HighSchool
                {
                    Id = highSchoolId,
                    HasEtms = true,
                    TranscriptFee = null,
                    Address = _models.GetMailingAddress().Generate()
                });
            }
            else
            {
                highSchoolId = _dataFakerFixture.Faker.PickRandom(highSchools).Id;
            }

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = highSchoolId
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, highSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().Be(0);
            education.Address.Should().NotBeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_HighSchoolWithEtms()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.HighSchool);
            var highSchools = _models.AllApplyLookups.HighSchools.Where(c => c.HasEtms && c.TranscriptFee > 0);
            var highSchool = _dataFakerFixture.Faker.PickRandom(highSchools);

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = highSchool.Id
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().Be(highSchool.TranscriptFee);
            education.Address.Should().NotBeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_HighSchoolWithoutEtms()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.HighSchool);
            var highSchools = _models.AllApplyLookups.HighSchools.Where(c => !c.HasEtms);
            var highSchool = _dataFakerFixture.Faker.PickRandom(highSchools);

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = highSchool.Id
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().BeNull();
            education.Address.Should().NotBeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_HighSchoolInactive()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.HighSchool);
            var highSchools = new List<HighSchool>() as IList<HighSchool>;
            var highSchool = new Dto.HighSchool
            {
                Id = Guid.NewGuid(),
                HasEtms = true,
                TranscriptFee = _dataFakerFixture.Faker.Finance.Amount(),
                MailingAddress = new Dto.Address
                {
                    Street = _dataFakerFixture.Faker.Address.StreetAddress()
                }
            };

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = highSchool.Id
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetHighSchool(It.IsAny<Guid>(), It.IsAny<DtoEnums.Locale>())).ReturnsAsync(highSchool);

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, highSchools, _models.AllApplyLookups.Universities, domesticContext.Object);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().Be(highSchool.TranscriptFee);
            education.Address.Should().NotBeNull();
            domesticContext.Verify(a => a.GetHighSchool(It.Is<Guid>(p => p == highSchool.Id), It.IsAny<DtoEnums.Locale>()), Times.Once);
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_UniversityEmptyFee()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.University);
            var universities = _models.AllApplyLookups.Universities.Where(c => c.HasEtms && c.TranscriptFee == null);
            var universityId = _dataFakerFixture.Faker.PickRandom(universities).Id;

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = universityId
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().Be(0);
            education.Address.Should().NotBeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_UniversityWithEtms()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.University);
            var universities = _models.AllApplyLookups.Universities.Where(c => c.HasEtms && c.TranscriptFee > 0);
            var university = _dataFakerFixture.Faker.PickRandom(universities);

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = university.Id
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().Be(university.TranscriptFee);
            education.Address.Should().BeEquivalentTo(university.Address, opt => opt
                .Excluding(z => z.ProvinceState)
                .Excluding(z => z.Country));
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_UniversityWithoutEtms()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.University);
            var universities = _models.AllApplyLookups.Universities.Where(c => !c.HasEtms).ToList();

            var universityId = Guid.NewGuid();
            if (!universities.Any())
            {
                universities.Add(
                    new University
                    {
                        Id = universityId,
                        HasEtms = false,
                        Address = _models.GetMailingAddress().Generate()
                    });
            }
            else
            {
                universityId = _dataFakerFixture.Faker.PickRandom(universities).Id;
            }

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = universityId
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().BeNull();
            education.Address.Should().NotBeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_UniversityInactive()
        {
            // Arrange
            var instituteType = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.University);
            var universities = new List<University>() as IList<University>;
            var university = new Dto.University
            {
                Id = Guid.NewGuid(),
                HasEtms = true,
                TranscriptFee = _dataFakerFixture.Faker.Finance.Amount(),
                MailingAddress = new Dto.Address
                {
                    Street = _dataFakerFixture.Faker.Address.StreetAddress()
                }
            };

            var dtoEducation = new Dto.Education
            {
                InstituteTypeId = instituteType.Id,
                InstituteId = university.Id
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetUniversity(It.IsAny<Guid>())).ReturnsAsync(university);

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, universities, domesticContext.Object);

            // Assert
            education.Should().NotBeNull();
            education.TranscriptFee.Should().Be(university.TranscriptFee);
            education.Address.Should().NotBeNull();
            domesticContext.Verify(a => a.GetUniversity(It.Is<Guid>(p => p == university.Id)), Times.Once);
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_HasBoth_CanDelete()
        {
            // Arrange
            var dtoEducation = new Dto.Education
            {
                HasPaidApplication = true,
                HasTranscripts = true
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.CanDelete.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_HasPaidApplication_CanDelete()
        {
            // Arrange
            var dtoEducation = new Dto.Education
            {
                HasPaidApplication = true,
                HasTranscripts = false
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.CanDelete.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_HasTranscripts_CanDelete()
        {
            // Arrange
            var dtoEducation = new Dto.Education
            {
                HasPaidApplication = false,
                HasTranscripts = true
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.CanDelete.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public async Task MapEducation_ShouldPass_When_CanDelete()
        {
            // Arrange
            var dtoEducation = new Dto.Education
            {
                HasPaidApplication = false,
                HasTranscripts = false
            };

            // Act
            var education = await _apiMapper.MapEducation(dtoEducation, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.Colleges, _models.AllApplyLookups.HighSchools, _models.AllApplyLookups.Universities, _domesticContext);

            // Assert
            education.Should().NotBeNull();
            education.CanDelete.Should().BeTrue();
        }
    }
}
