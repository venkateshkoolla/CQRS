using System;
using FluentAssertions;
using FluentAssertions.Extensions;
using Ocas.Domestic.Apply.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapCollegeTransmission_ShouldPass()
        {
            // Arrange
            var dtoEducation = new Dto.Education
            {
                Id = Guid.NewGuid(),
                ApplicantId = Guid.NewGuid(),
                InstituteName = "TestInstitute",
                AttendedFrom = "2017-05",
                AttendedTo = "2019-07",
                CurrentlyAttending = false
            };

            var collegeId = Guid.NewGuid();
            var lastLoadTime = _dataFakerFixture.Faker.Date.Past().AsUtc();

            // Act
            var collegeTransmission = _apiMapper.MapCollegeTransmission(Guid.NewGuid(), lastLoadTime, dtoEducation, collegeId);

            // Assert
            collegeTransmission.Should().NotBeNull();
            collegeTransmission.CollegeId.Should().Be(collegeId);
            collegeTransmission.Sent.Should().Be(lastLoadTime);
            collegeTransmission.Type.Should().Be(CollegeTransmissionType.Education);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapCollegeTransmission_ShouldPass_When_CurrentlyAttending()
        {
            // Arrange
            var dtoEducation = new Dto.Education
            {
                Id = Guid.NewGuid(),
                ApplicantId = Guid.NewGuid(),
                InstituteName = "TestInstitute",
                AttendedFrom = "2017-05",
                CurrentlyAttending = true
            };

            var collegeId = Guid.NewGuid();
            var lastLoadTime = _dataFakerFixture.Faker.Date.Past().AsUtc();

            // Act
            var collegeTransmission = _apiMapper.MapCollegeTransmission(Guid.NewGuid(), lastLoadTime, dtoEducation, collegeId);

            // Assert
            collegeTransmission.Should().NotBeNull();
            collegeTransmission.CollegeId.Should().Be(collegeId);
            collegeTransmission.Sent.Should().Be(lastLoadTime);
            collegeTransmission.Type.Should().Be(CollegeTransmissionType.Education);
        }
    }
}
