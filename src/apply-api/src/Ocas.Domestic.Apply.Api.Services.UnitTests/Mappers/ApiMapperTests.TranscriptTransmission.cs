using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Mappers
{
    public partial class ApiMapperTests
    {
        [Fact]
        [UnitTest("Mappers")]
        public void MapTranscriptTransmissions_ShouldPass_Without_Institution()
        {
            // Arrange
            var dtoTransmission = new Dto.TranscriptTransmission
            {
                TermDueDate = _dataFakerFixture.Faker.Date.Soon(),
                LocalizedName = _dataFakerFixture.Faker.Lorem.Sentence(5),
                InstitutionType = null
            };
            var dtoTransmissions = new List<Dto.TranscriptTransmission> { dtoTransmission };

            // Act
            var transmissions = _apiMapper.MapTranscriptTransmissions(dtoTransmissions, _dataFakerFixture.SeedData.InstituteTypes);

            // Assert
            transmissions.Should().HaveSameCount(dtoTransmissions);
            var transmission = transmissions.Single();
            transmission.Should().NotBeNull();
            transmission.EligibleUntil.Should().Be(dtoTransmission.TermDueDate);
            transmission.Label.Should().Be(dtoTransmission.LocalizedName);
            transmission.InstituteTypeId.Should().BeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapTranscriptTransmissions_ShouldPass_When_College()
        {
            // Arrange
            var instituteTypeId = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.College).Id;

            var dtoTransmission = new Dto.TranscriptTransmission
            {
                InstitutionType = InstitutionType.College,
                LocalizedName = _dataFakerFixture.Faker.Lorem.Sentence(5),
                TermDueDate = _dataFakerFixture.Faker.Date.Soon()
            };
            var dtoTransmissions = new List<Dto.TranscriptTransmission> { dtoTransmission };

            // Act
            var transmissions = _apiMapper.MapTranscriptTransmissions(dtoTransmissions, _dataFakerFixture.SeedData.InstituteTypes);

            // Assert
            transmissions.Should().HaveSameCount(dtoTransmissions);
            var transmission = transmissions.Single();
            transmission.Should().NotBeNull();
            transmission.Label.Should().Be(dtoTransmission.LocalizedName);
            transmission.InstituteTypeId.Should().Be(instituteTypeId);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapTranscriptTransmissions_ShouldPass_When_University()
        {
            // Arrange
            var instituteTypeId = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.University).Id;

            var dtoTransmission = new Dto.TranscriptTransmission
            {
                InstitutionType = InstitutionType.University,
                LocalizedName = _dataFakerFixture.Faker.Lorem.Sentence(5),
                TermDueDate = _dataFakerFixture.Faker.Date.Soon()
            };
            var dtoTransmissions = new List<Dto.TranscriptTransmission> { dtoTransmission };

            // Act
            var transmissions = _apiMapper.MapTranscriptTransmissions(dtoTransmissions, _dataFakerFixture.SeedData.InstituteTypes);

            // Assert
            transmissions.Should().HaveSameCount(dtoTransmissions);
            var transmission = transmissions.Single();
            transmission.Should().NotBeNull();
            transmission.Label.Should().Be(dtoTransmission.LocalizedName);
            transmission.InstituteTypeId.Should().Be(instituteTypeId);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapTranscriptTransmissions_ShouldPass_When_SendNow()
        {
            // Arrange
            var dtoTransmission = new Dto.TranscriptTransmission
            {
                Code = Constants.TranscriptTransmissions.SendTranscriptNow
            };
            var dtoTransmissions = new List<Dto.TranscriptTransmission> { dtoTransmission };

            // Act
            var transmissions = _apiMapper.MapTranscriptTransmissions(dtoTransmissions, _dataFakerFixture.SeedData.InstituteTypes);

            // Assert
            transmissions.Should().HaveSameCount(dtoTransmissions);
            var transmission = transmissions.Single();
            transmission.Should().NotBeNull();
            transmission.Code.Should().Be(Constants.TranscriptTransmissions.SendTranscriptNow);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapTranscriptTransmissions_Should_Be_Missing_Without_TermDueDate_Or_SendNow()
        {
            // Arrange
            var dtoTransmission = new Dto.TranscriptTransmission
            {
                Code = Constants.TranscriptTransmissions.AfterDegreeConferred
            };
            var dtoTransmissions = new List<Dto.TranscriptTransmission> { dtoTransmission };

            // Act
            var transmissions = _apiMapper.MapTranscriptTransmissions(dtoTransmissions, _dataFakerFixture.SeedData.InstituteTypes);

            // Assert
            transmissions.Should().BeEmpty();
        }
    }
}
