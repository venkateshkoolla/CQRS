using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class GetSupportingDocumentsHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly IPrincipal _user;
        private readonly RequestCache _requestCache;
        private readonly IApiMapper _apiMapper;

        public GetSupportingDocumentsHandlerTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _user = Mock.Of<IPrincipal>();
            _requestCache = new RequestCacheMock();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSupportingDocumentsHandler_ShouldPass()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var request = new GetSupportingDocuments
            {
                ApplicantId = applicantId,
                User = _user
            };

            var documentTypes = await XunitInjectionCollection.LookupsCache.GetSupportingDocumentTypes(Constants.Localization.EnglishCanada);
            var documentTypeId = documentTypes.FirstOrDefault().Id;

            var officials = await XunitInjectionCollection.LookupsCache.GetOfficials(Constants.Localization.EnglishCanada);
            var officialId = officials.FirstOrDefault(x => x.Code == "O").Id;

            var supportingDocuments = new List<Dto.SupportingDocument> {
                new Dto.SupportingDocument
                {
                    ApplicantId = applicantId,
                    Id = Guid.NewGuid(),
                    DocumentTypeId = documentTypeId,
                    OfficialId = officialId,
                    DateReceived = DateTime.UtcNow.AddDays(-1)
                },
                new Dto.SupportingDocument
                {
                    ApplicantId = applicantId,
                    Id = Guid.NewGuid(),
                    DocumentTypeId = documentTypeId,
                    OfficialId = officialId,
                    DateReceived = DateTime.UtcNow.AddDays(-2)
                }
            };

            var collegeIds = _dataFakerFixture.SeedData.Colleges.Select(x => x.Id).ToList();
            var transcripts = new List<Dto.Transcript>
            {
                new Dto.Transcript
                {
                    Id = Guid.NewGuid(),
                    ContactId = applicantId,
                    TranscriptType = TranscriptType.OntarioCollegeUniversityTranscript,
                    CreatedOn = DateTime.UtcNow.AddDays(-3),
                    PartnerId = _dataFakerFixture.Faker.PickRandom(collegeIds)
                }
            };

            var dateTestTaken = DateTime.UtcNow;
            var tests = new List<Dto.Test> { new Dto.Test { ApplicantId = applicantId, IsOfficial = true, DateTestTaken = dateTestTaken, CreatedOn = DateTime.UtcNow.AddDays(-4) } };

            var academicRecords = new List<Dto.AcademicRecord> { new Dto.AcademicRecord { ApplicantId = applicantId, Id = Guid.NewGuid(), CreatedOn = DateTime.UtcNow.AddDays(-5) } };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetSupportingDocuments(It.IsAny<Guid>())).ReturnsAsync(supportingDocuments);
            domesticContextMock.Setup(x => x.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(transcripts);
            domesticContextMock.Setup(x => x.GetTests(It.IsAny<Dto.GetTestOptions>(), It.IsAny<Locale>())).ReturnsAsync(tests);
            domesticContextMock.Setup(x => x.GetAcademicRecords(It.IsAny<Guid>())).ReturnsAsync(academicRecords);

            var handler = new GetSupportingDocumentsHandler(Mock.Of<ILogger<GetSupportingDocumentsHandler>>(), domesticContextMock.Object, _apiMapper, XunitInjectionCollection.LookupsCache, _requestCache);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCount(supportingDocuments.Count + transcripts.Count + tests.Count + academicRecords.Count);
            response.Where(x => x.Type == SupportingDocumentType.Other).Should().HaveCount(supportingDocuments.Count);
            response.Where(x => x.Type == SupportingDocumentType.Transcript).Should().HaveCount(transcripts.Count);
            response.Where(x => x.Type == SupportingDocumentType.StandardizedTest).Should().HaveCount(tests.Count);
            response.Where(x => x.Type == SupportingDocumentType.Grades).Should().HaveCount(academicRecords.Count);
            response.Should().BeInAscendingOrder(x => x.ReceivedDate);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSupportingDocumentsHandler_ShouldPass_When_ReturnEmpty()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var request = new GetSupportingDocuments
            {
                ApplicantId = applicantId,
                User = _user
            };

            var supportingDocuments = new List<Dto.SupportingDocument>();
            var transcripts = new List<Dto.Transcript>();
            var tests = new List<Dto.Test>();
            var academicRecords = new List<Dto.AcademicRecord>();

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetSupportingDocuments(It.IsAny<Guid>())).ReturnsAsync(supportingDocuments);
            domesticContextMock.Setup(x => x.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(transcripts);
            domesticContextMock.Setup(x => x.GetTests(It.IsAny<Dto.GetTestOptions>(), It.IsAny<Locale>())).ReturnsAsync(tests);
            domesticContextMock.Setup(x => x.GetAcademicRecords(It.IsAny<Guid>())).ReturnsAsync(academicRecords);

            var handler = new GetSupportingDocumentsHandler(Mock.Of<ILogger<GetSupportingDocumentsHandler>>(), domesticContextMock.Object, _apiMapper, XunitInjectionCollection.LookupsCache, _requestCache);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Should().BeEmpty();
        }
    }
}
