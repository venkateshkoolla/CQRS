using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
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
        public void MapInstituteWarnings_ShouldPass_When_Education()
        {
            // Arrange
            var trException = _dataFakerFixture.Faker.PickRandom(_dataFakerFixture.SeedData.TranscriptRequestExceptions);
            var trExceptions = new List<Dto.TranscriptRequestException>
            {
                trException
            };

            var educationIds = new List<Guid>
            {
                trException.Id
            };

            // Act
            var instituteWarnings = _apiMapper.MapInstituteWarnings(trExceptions, educationIds);

            // Assert
            instituteWarnings.Should().ContainSingle();
            var instituteWarning = instituteWarnings.First();
            instituteWarning.Type.Should().Be(InstituteWarningType.Education);
            instituteWarning.Content.Should().Be(trException.LocalizedName);
            instituteWarning.Id.Should().Be(trException.Id);
            instituteWarning.InstituteId.Should().Be(trException.InstituteId);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapInstituteWarnings_ShouldPass_When_NoEducationIds()
        {
            // Arrange
            var trException = _dataFakerFixture.Faker.PickRandom(_dataFakerFixture.SeedData.TranscriptRequestExceptions);
            var trExceptions = new List<Dto.TranscriptRequestException>
            {
                trException
            };

            // Act
            var instituteWarnings = _apiMapper.MapInstituteWarnings(trExceptions, new List<Guid>());

            // Assert
            instituteWarnings.Should().ContainSingle();
            var instituteWarning = instituteWarnings.First();
            instituteWarning.Type.Should().Be(InstituteWarningType.Transcript);
            instituteWarning.Content.Should().Be(trException.LocalizedName);
            instituteWarning.Id.Should().Be(trException.Id);
            instituteWarning.InstituteId.Should().Be(trException.InstituteId);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapInstituteWarnings_ShouldPass_When_Transcript()
        {
            // Arrange
            var trException = _dataFakerFixture.Faker.PickRandom(_dataFakerFixture.SeedData.TranscriptRequestExceptions);
            var trExceptions = new List<Dto.TranscriptRequestException>
            {
                trException
            };

            var educationIds = new List<Guid>
            {
                Guid.NewGuid()
            };

            // Act
            var instituteWarnings = _apiMapper.MapInstituteWarnings(trExceptions, educationIds);

            // Assert
            instituteWarnings.Should().ContainSingle();
            var instituteWarning = instituteWarnings.First();
            instituteWarning.Type.Should().Be(InstituteWarningType.Transcript);
            instituteWarning.Content.Should().Be(trException.LocalizedName);
            instituteWarning.Id.Should().Be(trException.Id);
            instituteWarning.InstituteId.Should().Be(trException.InstituteId);
        }
    }
}
