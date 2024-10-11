using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Enums;
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
        public void MapSupportingDocument_ShouldPass_When_StandardizedTest()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var dateTestTaken = DateTime.UtcNow;
            var testCreatedOn = DateTime.UtcNow;
            var testTypes = _models.AllApplyLookups.StandardizedTestTypes;

            var dtoTests = new List<Dto.Test> {
                 new Dto.Test
                {
                    Id = Guid.NewGuid(),
                    ApplicantId = applicantId,
                    DateTestTaken = dateTestTaken,
                    TestTypeId = _dataFakerFixture.Faker.PickRandom(testTypes).Id,
                    CreatedOn = testCreatedOn
                },
                 new Dto.Test
                 {
                     Id = Guid.NewGuid(),
                     ApplicantId = applicantId,
                     DateTestTaken = dateTestTaken.AddDays(-1),
                     TestTypeId = _dataFakerFixture.Faker.PickRandom(testTypes).Id,
                     CreatedOn = testCreatedOn
                 }
            };

            // Act
            var supportingDocumentsActual = _apiMapper.MapSupportingDocuments(dtoTests, testTypes);

            // Assert
            supportingDocumentsActual.Should().NotBeNullOrEmpty()
                                     .And.HaveSameCount(dtoTests);
            supportingDocumentsActual.Should().OnlyContain(x => !x.Processing);
            supportingDocumentsActual.Should().OnlyContain(x => x.Type == SupportingDocumentType.StandardizedTest);
            supportingDocumentsActual.Should().OnlyContain(x => x.ReceivedDate == testCreatedOn);
            supportingDocumentsActual.Should().NotContain(x => x.Name.Length < 1);

            supportingDocumentsActual.Should().SatisfyRespectively(
                first => first.Name.Should().EndWith("(" + dateTestTaken.ToStringOrDefault() + ")"),
                second => second.Name.Should().EndWith("(" + dateTestTaken.AddDays(-1).ToStringOrDefault() + ")"));
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapSupportingDocument_ShouldPass_When_Transcript()
        {
            // Arrange
            var colleges = _models.AllApplyLookups.Colleges;
            var universities = _models.AllApplyLookups.Universities;

            var transcriptCreatedOn = DateTime.UtcNow;
            var dtoTranscripts = new List<Dto.Transcript>
            {
                new Dto.Transcript
                {
                    ContactId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    TranscriptType = TranscriptType.OntarioCollegeUniversityTranscript,
                    PartnerId = _dataFakerFixture.Faker.PickRandom(colleges).Id,
                    CreatedOn = transcriptCreatedOn
                }
            };

            // Act
            var supportingDocumentsActual = _apiMapper.MapSupportingDocuments(dtoTranscripts, colleges, universities);

            // Assert
            supportingDocumentsActual.Should().NotBeNullOrEmpty()
                                     .And.HaveSameCount(dtoTranscripts);
            supportingDocumentsActual.Should().NotContain(x => x.Name.Length < 1);
            supportingDocumentsActual.Should().OnlyContain(x => x.Type == SupportingDocumentType.Transcript);
            supportingDocumentsActual.Should().OnlyContain(x => !x.Processing);
            supportingDocumentsActual.Should().OnlyContain(x => x.ReceivedDate == transcriptCreatedOn);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MapSupportingDocument_ShouldPass_When_AcademicRecord()
        {
            // Arrange
            var createdOn = DateTime.UtcNow;
            var dtoAcademicRecords = new List<Dto.AcademicRecord>
            {
                new Dto.AcademicRecord
                {
                    ApplicantId = Guid.NewGuid(),
                    CreatedOn = createdOn
                }
            };

            // Act
            var supportingDocumentsActual = _apiMapper.MapSupportingDocuments(dtoAcademicRecords);

            // Assert
            supportingDocumentsActual.Should().NotBeNullOrEmpty()
                                     .And.HaveSameCount(dtoAcademicRecords);
            supportingDocumentsActual.Should().OnlyContain(x => x.Type == SupportingDocumentType.Grades);
            supportingDocumentsActual.Should().OnlyContain(x => !x.Processing);
            supportingDocumentsActual.Should().OnlyContain(x => x.ReceivedDate == createdOn);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MappingSupportingDocument_ShouldPass_When_Other()
        {
            // Arrange
            var createdOn = DateTime.UtcNow;

            var supportingDocumentTypes = _models.AllApplyLookups.SupportingDocumentTypes;
            var officials = _models.AllApplyLookups.Officials;
            var institutes = _models.AllApplyLookups.Institutes;

            var doc = _dataFakerFixture.Faker.PickRandom(supportingDocumentTypes.Where(x => x.Code == Constants.SupportingDocumentTypes.AcademicUpgradeDocuments));
            var official = _dataFakerFixture.Faker.PickRandom(officials);
            var institute = _dataFakerFixture.Faker.PickRandom(institutes);

            var dtoSupportingDocuments = new List<Dto.SupportingDocument>
            {
                new Dto.SupportingDocument
                {
                    ApplicantId = Guid.NewGuid(),
                    DocumentTypeId = supportingDocumentTypes.Where(x => x.Label == doc.Label).FirstOrDefault().Id,
                    OfficialId = official.Id,
                    InstituteId = institute.Id,
                    DateReceived = createdOn
                }
            };

            var docName = $"{official.Label} {doc.Label} - {institute.Label}".Trim();

            // Act
            var supportingDocumentsActual = _apiMapper.MapSupportingDocuments(dtoSupportingDocuments, supportingDocumentTypes, officials, institutes);

            // Assert
            supportingDocumentsActual.Should().NotBeNullOrEmpty()
                                     .And.HaveSameCount(dtoSupportingDocuments);
            supportingDocumentsActual.Should().OnlyContain(x => x.Type == SupportingDocumentType.Other);
            supportingDocumentsActual.Should().OnlyContain(x => x.Processing);
            supportingDocumentsActual.Should().OnlyContain(x => x.ReceivedDate == createdOn);
            supportingDocumentsActual.Should().OnlyContain(x => x.Name == docName);
        }

        [Fact]
        [UnitTest("Mappers")]
        public void MappingSupportingDocument_ShouldPass_When_IntlCredential()
        {
            // Arrange
            var createdOn = DateTime.UtcNow;

            var supportingDocumentTypes = _models.AllApplyLookups.SupportingDocumentTypes;
            var officials = _models.AllApplyLookups.Officials;
            var institutes = _models.AllApplyLookups.Institutes;

            var official = _dataFakerFixture.Faker.PickRandom(officials);
            var institute = _dataFakerFixture.Faker.PickRandom(institutes);
            var doc = supportingDocumentTypes.Where(x => x.Code == Constants.SupportingDocumentTypes.EvaluationReport).FirstOrDefault();

            var dtoSupportingDocuments = new List<Dto.SupportingDocument>
            {
                new Dto.SupportingDocument
                {
                    ApplicantId = Guid.NewGuid(),
                    DocumentTypeId = doc.Id,
                    OfficialId = official.Id,
                    InstituteId = institute.Id,
                    DateReceived = createdOn,
                    Name = doc.Label
                }
            };

            // Act
            var supportingDocumentsActual = _apiMapper.MapSupportingDocuments(dtoSupportingDocuments, supportingDocumentTypes, officials, institutes);

            // Assert
            supportingDocumentsActual.Should().NotBeNullOrEmpty()
                                     .And.HaveSameCount(dtoSupportingDocuments);
            supportingDocumentsActual.Should().OnlyContain(x => x.Type == SupportingDocumentType.IntlCredentialAssessment);
            supportingDocumentsActual.Should().OnlyContain(x => x.Processing);
            supportingDocumentsActual.Should().OnlyContain(x => x.ReceivedDate == createdOn);
            supportingDocumentsActual.Should().OnlyContain(x => x.Name == doc.Label);
        }
    }
}
