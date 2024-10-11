using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapColleges_ShouldPass_When_Open()
        {
            // Arrange
            var schoolStatus = new Dto.SchoolStatus
            {
                Id = Guid.NewGuid(),
                Code = Constants.SchoolStatuses.Open
            };
            var dtoCollege = new Dto.College
            {
                Id = Guid.NewGuid(),
                SchoolStatusId = schoolStatus.Id
            };

            // Act
            var colleges = _apiMapper.MapColleges(
                new List<Dto.College> { dtoCollege },
                new List<Dto.SchoolStatus> { schoolStatus });

            // Assert
            colleges.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var college = colleges.First();
            college.Id.Should().Be(dtoCollege.Id);
            college.IsOpen.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapColleges_ShouldPass_When_NotOpen()
        {
            // Arrange
            var schoolStatus = new Dto.SchoolStatus
            {
                Id = Guid.NewGuid(),
                Code = "X"
            };
            var dtoCollege = new Dto.College
            {
                Id = Guid.NewGuid(),
                SchoolStatusId = schoolStatus.Id
            };

            // Act
            var colleges = _apiMapper.MapColleges(
                new List<Dto.College> { dtoCollege },
                new List<Dto.SchoolStatus> { schoolStatus });

            // Assert
            colleges.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var college = colleges.First();
            college.Id.Should().Be(dtoCollege.Id);
            college.IsOpen.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapColleges_ShouldPass_When_NoSchoolStatus()
        {
            // Arrange
            var dtoCollege = new Dto.College
            {
                Id = Guid.NewGuid(),
                SchoolStatusId = Guid.NewGuid()
            };

            // Act
            var colleges = _apiMapper.MapColleges(
                new List<Dto.College> { dtoCollege },
                new List<Dto.SchoolStatus>());

            // Assert
            colleges.Should().NotBeNullOrEmpty()
                .And.ContainSingle();
            var college = colleges.First();
            college.Id.Should().Be(dtoCollege.Id);
            college.IsOpen.Should().BeFalse();
        }
    }
}
