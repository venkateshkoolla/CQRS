using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Models.Templates;
using Ocas.Domestic.Apply.Services.Handlers;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.Apply.Services.Messages;
using Ocas.Domestic.Apply.TemplateProcessors;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;
using Encoding = System.Text.Encoding;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class GetSupportingDocumentFileHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ITemplateMapper _templateMapper;
        private readonly RequestCache _requestCache;
        private readonly ITranslationsCache _translationsCache = new TranslationsCacheMock();

        public GetSupportingDocumentFileHandlerTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _templateMapper = XunitInjectionCollection.AutoMapperFixture.CreateTemplateMapper();
            _requestCache = new RequestCacheMock();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSupportingDocumentFileHandler_ShouldPass_WithSupportingDocument()
        {
            // Arrange
            var data = _dataFakerFixture.Faker.Random.Bytes(100);
            var documentId = Guid.NewGuid();
            var request = new GetSupportingDocumentFile
            {
                Id = documentId
            };

            var base64String = Convert.ToBase64String(data);
            var annotation = new Dto.Annotation { Id = documentId, MimeType = "text/plain", DocumentBody = base64String, CreatedOn = DateTime.Now };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetSupportingDocument(It.IsAny<Guid>())).ReturnsAsync(new Dto.SupportingDocument { ApplicantId = Guid.NewGuid(), Id = documentId });
            domesticContextMock.Setup(m => m.GetSupportingDocumentBinaryData(It.IsAny<Guid>())).ReturnsAsync(annotation);

            var handler = new GetSupportingDocumentFileHandler(Mock.Of<ILogger<GetSupportingDocumentFileHandler>>(), domesticContextMock.Object, Mock.Of<IUserAuthorization>(), Mock.Of<IRazorTemplateService>(), XunitInjectionCollection.LookupsCache, _translationsCache, _templateMapper, _requestCache);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            domesticContextMock.Verify(e => e.GetSupportingDocumentBinaryData(It.IsAny<Guid>()), Times.Once);
            response.Data.Should().NotBeNullOrEmpty();
            response.Data.Length.Should().Be(100);
            response.Data.Should().BeEquivalentTo(data);
            response.MimeType.Should().NotBeNullOrEmpty();
            response.CreatedDate.ToString().Should().NotBeNullOrEmpty();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSupportingDocumentFileHandler_ShouldPass_WithTranscript()
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes("<envelope format=\"X12\"></envelope>");
            var documentId = Guid.NewGuid();
            var request = new GetSupportingDocumentFile
            {
                Id = documentId
            };

            var base64String = Convert.ToBase64String(data);
            var annotation = new Dto.Annotation { Id = documentId, MimeType = "text/plain", DocumentBody = base64String, CreatedOn = DateTime.Now };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscript(It.IsAny<Guid>())).ReturnsAsync(new Dto.Transcript { ContactId = Guid.NewGuid(), Id = documentId, TranscriptType = TranscriptType.OntarioCollegeUniversityTranscript });
            domesticContextMock.Setup(m => m.GetSupportingDocumentBinaryData(It.IsAny<Guid>())).ReturnsAsync(annotation);

            var razorTemplateServiceMock = new Mock<IRazorTemplateService>();
            razorTemplateServiceMock.Setup(x => x.GeneratePostSecondaryTranscriptAsync(It.IsAny<PostSecondaryTranscriptViewModel>())).ReturnsAsync(data);

            var handler = new GetSupportingDocumentFileHandler(Mock.Of<ILogger<GetSupportingDocumentFileHandler>>(), domesticContextMock.Object, Mock.Of<IUserAuthorization>(), razorTemplateServiceMock.Object, XunitInjectionCollection.LookupsCache, _translationsCache, _templateMapper, _requestCache);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            domesticContextMock.Verify(e => e.GetSupportingDocumentBinaryData(It.IsAny<Guid>()), Times.Once);
            response.Data.Should().NotBeNullOrEmpty();
            response.Data.Length.Should().Be(data.Length);
            response.Data.Should().BeEquivalentTo(data);
            response.MimeType.Should().NotBeNullOrEmpty();
            response.CreatedDate.ToString().Should().NotBeNullOrEmpty();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetSupportingDocumentFileHandler_ShouldThrow_NoTranscript_NoSupportingDocument()
        {
            // Arrange
            var documentId = Guid.NewGuid();
            var request = new GetSupportingDocumentFile
            {
                Id = documentId
            };

            var domesticContextMock = new DomesticContextMock();
            var handler = new GetSupportingDocumentFileHandler(Mock.Of<ILogger<GetSupportingDocumentFileHandler>>(), domesticContextMock.Object, Mock.Of<IUserAuthorization>(), Mock.Of<IRazorTemplateService>(), XunitInjectionCollection.LookupsCache, _translationsCache, _templateMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().And.Message.Should().Be($"No supporting document found {request.Id}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetSupportingDocumentFileHandler_Should_Throw_When_No_Request_Found()
        {
            // Arrange
            var documentId = Guid.NewGuid();
            var request = new GetSupportingDocumentFile { Id = documentId };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetSupportingDocument(It.IsAny<Guid>())).ReturnsAsync(new Dto.SupportingDocument { ApplicantId = Guid.NewGuid(), Id = documentId });
            var handler = new GetSupportingDocumentFileHandler(Mock.Of<ILogger<GetSupportingDocumentFileHandler>>(), domesticContextMock.Object, Mock.Of<IUserAuthorization>(), Mock.Of<IRazorTemplateService>(), XunitInjectionCollection.LookupsCache, _translationsCache, _templateMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().And.Message.Should().Be($"Binary data {request.Id} not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetSupportingDocumentFileHandler_Should_Throw_When_Invalid_Base64()
        {
            // Arrange
            var documentId = Guid.NewGuid();
            var request = new GetSupportingDocumentFile
            {
                Id = documentId
            };

            var inValidBase64String = System.Convert.ToBase64String(_dataFakerFixture.Faker.Random.Bytes(100)) + "==";

            var annotation = new Dto.Annotation { Id = documentId, MimeType = "text/plain", DocumentBody = inValidBase64String, CreatedOn = DateTime.Now };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetSupportingDocument(It.IsAny<Guid>())).ReturnsAsync(new Dto.SupportingDocument { ApplicantId = Guid.NewGuid(), Id = documentId });
            domesticContextMock.Setup(m => m.GetSupportingDocumentBinaryData(It.IsAny<Guid>())).ReturnsAsync(annotation);

            var handler = new GetSupportingDocumentFileHandler(Mock.Of<ILogger<GetSupportingDocumentFileHandler>>(), domesticContextMock.Object, Mock.Of<IUserAuthorization>(), Mock.Of<IRazorTemplateService>(), XunitInjectionCollection.LookupsCache, _translationsCache, _templateMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be("Invalid Data");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetSupportingDocumentFileHandler_Should_Throw_When_Base64_Empty()
        {
            // Arrange
            var documentId = Guid.NewGuid();
            var request = new GetSupportingDocumentFile
            {
                Id = documentId
            };

            var annotation = new Dto.Annotation { Id = documentId, MimeType = "text/plain", DocumentBody = string.Empty, CreatedOn = DateTime.Now };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetSupportingDocument(It.IsAny<Guid>())).ReturnsAsync(new Dto.SupportingDocument { ApplicantId = Guid.NewGuid(), Id = documentId });
            domesticContextMock.Setup(m => m.GetSupportingDocumentBinaryData(It.IsAny<Guid>())).ReturnsAsync(annotation);

            var handler = new GetSupportingDocumentFileHandler(Mock.Of<ILogger<GetSupportingDocumentFileHandler>>(), domesticContextMock.Object, Mock.Of<IUserAuthorization>(), Mock.Of<IRazorTemplateService>(), XunitInjectionCollection.LookupsCache, _translationsCache, _templateMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be("Invalid Base64");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSupportingDocumentFileHandler4HighSchoolGrades_ShouldPass_WithTranscript()
        {
            // Arrange
            var data = Encoding.UTF8.GetBytes("<envelope format=\"X12\"></envelope>");
            var documentId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();
            var request = new GetSupportingDocumentFile
            {
                Id = documentId
            };
            var retOntStudCrsCr = new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = applicantId, Id = Guid.NewGuid() } };
            var retTranscripts = new List<Dto.Transcript> { new Dto.Transcript { Id = Guid.NewGuid(), ContactId = applicantId, TranscriptType = TranscriptType.OntarioHighSchoolTranscript } };
            var domesticContextMock = new DomesticContextMock();
            var razorTemplateServiceMock = new Mock<IRazorTemplateService>();
            var handler = new GetSupportingDocumentFileHandler(Mock.Of<ILogger<GetSupportingDocumentFileHandler>>(), domesticContextMock.Object, Mock.Of<IUserAuthorization>(), razorTemplateServiceMock.Object, XunitInjectionCollection.LookupsCache, _translationsCache, _templateMapper, _requestCache);

            domesticContextMock.Setup(m => m.GetAcademicRecord(It.IsAny<Guid>())).ReturnsAsync(new Dto.AcademicRecord { Id = Guid.NewGuid(), ApplicantId = applicantId });
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(retOntStudCrsCr);
            domesticContextMock.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(retTranscripts);
            razorTemplateServiceMock.Setup(x => x.GenerateHighSchoolGradesAsync(It.IsAny<HighSchoolGradesViewModel>())).ReturnsAsync(data);

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNullOrEmpty();
            response.MimeType.Should().NotBeNullOrEmpty();
            response.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, 20000);
            response.MimeType.Should().BeEquivalentTo("application/pdf");
            response.Data.Should().BeEquivalentTo(data);
            domesticContextMock.Verify(e => e.GetAcademicRecord(It.IsAny<Guid>()), Times.Once);
            razorTemplateServiceMock.Verify(r => r.GenerateHighSchoolGradesAsync(It.IsAny<HighSchoolGradesViewModel>()), Times.Once);
            domesticContextMock.Verify(e => e.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>()), Times.Once);
            domesticContextMock.Verify(e => e.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>()), Times.Once);
        }
    }
}
