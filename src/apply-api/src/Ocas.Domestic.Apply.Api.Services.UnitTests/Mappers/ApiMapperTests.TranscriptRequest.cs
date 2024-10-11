using System;
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
        public void MapTranscriptRequest_ShouldPass()
        {
            // Arrange
            var dtoTranscriptReq = new Dto.TranscriptRequest
            {
                TranscriptFee = _dataFakerFixture.Faker.Finance.Amount(),
                FromSchoolId = Guid.NewGuid(),
                FromSchoolName = _dataFakerFixture.Faker.Name.JobTitle(),
                TranscriptRequestStatusId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.TranscriptRequestStatuses).Id,
                ToSchoolId = Guid.NewGuid(),
                ToSchoolName = _dataFakerFixture.Faker.Name.JobTitle(),
                TranscriptTransmissionId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.TranscriptTransmissions.Where(x => x.Code != Constants.TranscriptTransmissions.SendTranscriptNow)).Id
            };

            // Act
            var transcriptReq = _apiMapper.MapTranscriptRequest(dtoTranscriptReq, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.TranscriptTransmissions);

            // Assert
            transcriptReq.Should().NotBeNull();
            transcriptReq.Amount.Should().Be(dtoTranscriptReq.TranscriptFee);
            transcriptReq.FromInstituteId.Should().Be(dtoTranscriptReq.FromSchoolId);
            transcriptReq.FromInstituteName.Should().Be(dtoTranscriptReq.FromSchoolName);
            transcriptReq.RequestStatusId.Should().Be(dtoTranscriptReq.TranscriptRequestStatusId);
            transcriptReq.ToInstituteId.Should().Be(dtoTranscriptReq.ToSchoolId);
            transcriptReq.ToInstituteName.Should().Be(dtoTranscriptReq.ToSchoolName);
            transcriptReq.TransmissionId.Should().Be(dtoTranscriptReq.TranscriptTransmissionId.Value);
            transcriptReq.FromInstituteTypeId.Should().BeNull();
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapTranscriptRequest_ShouldPass_When_College()
        {
            // Arrange
            var dtoTranscriptReq = new Dto.TranscriptRequest
            {
                FromSchoolType = TranscriptSchoolType.College
            };
            var instituteTypeId = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.College).Id;

            // Act
            var transcriptReq = _apiMapper.MapTranscriptRequest(dtoTranscriptReq, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.TranscriptTransmissions);

            // Assert
            transcriptReq.Should().NotBeNull();
            transcriptReq.FromInstituteTypeId.Should().Be(instituteTypeId);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapTranscriptRequest_ShouldPass_When_HighSchool()
        {
            // Arrange
            var dtoTranscriptReq = new Dto.TranscriptRequest
            {
                FromSchoolType = TranscriptSchoolType.HighSchool
            };
            var instituteTypeId = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.HighSchool).Id;

            // Act
            var transcriptReq = _apiMapper.MapTranscriptRequest(dtoTranscriptReq, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.TranscriptTransmissions);

            // Assert
            transcriptReq.Should().NotBeNull();
            transcriptReq.FromInstituteTypeId.Should().Be(instituteTypeId);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapTranscriptRequest_ShouldPass_When_University()
        {
            // Arrange
            var dtoTranscriptReq = new Dto.TranscriptRequest
            {
                FromSchoolType = TranscriptSchoolType.University
            };
            var instituteTypeId = _models.AllApplyLookups.InstituteTypes.First(i => i.Code == Constants.InstituteTypes.University).Id;

            // Act
            var transcriptReq = _apiMapper.MapTranscriptRequest(dtoTranscriptReq, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.TranscriptTransmissions);

            // Assert
            transcriptReq.Should().NotBeNull();
            transcriptReq.FromInstituteTypeId.Should().Be(instituteTypeId);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapTranscriptRequest_ShouldPass_When_TransmissionIdNull()
        {
            // Arrange
            var transmissionId = _dataFakerFixture.SeedData.TranscriptTransmissions.First(x => x.Code == Constants.TranscriptTransmissions.SendTranscriptNow).Id;
            var dtoTranscriptReq = new Dto.TranscriptRequest
            {
                TranscriptTransmissionId = null
            };

            // Act
            var transcriptReq = _apiMapper.MapTranscriptRequest(dtoTranscriptReq, _models.AllApplyLookups.InstituteTypes, _models.AllApplyLookups.TranscriptTransmissions);

            // Assert
            transcriptReq.Should().NotBeNull();
            transcriptReq.TransmissionId.Should().Be(transmissionId);
        }
    }
}
