using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class DeleteTranscriptRequestHandlerTests
    {
        private readonly ILogger<DeleteTranscriptRequestHandler> _logger;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IPrincipal _user;

        public DeleteTranscriptRequestHandlerTests()
        {
            _logger = Mock.Of<ILogger<DeleteTranscriptRequestHandler>>();
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _user = Mock.Of<IPrincipal>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeleteTranscriptRequest_ShouldPass()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new DeleteTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(new Dto.TranscriptRequest
            {
                Id = transcriptRequestId,
                ApplicantId = Guid.NewGuid()
            });

            var handler = new DeleteTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.DeleteTranscriptRequest(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeleteTranscriptRequest_ShouldThrow_When_RequestNotFound()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new DeleteTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync((Dto.TranscriptRequest)null);

            var handler = new DeleteTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be("Transcript request not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeleteTranscriptRequest_ShouldThrow_When_RequestNoApplicant()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new DeleteTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(new Dto.TranscriptRequest());

            var handler = new DeleteTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            const string expectedMessage = "Transcript request does not have an applicant.";
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be(expectedMessage);
            VerifyLog(LogLevel.Warning, expectedMessage);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeleteTranscriptRequest_ShouldThrow_When_RequestHasRequestLog()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new DeleteTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(new Dto.TranscriptRequest
            {
                Id = transcriptRequestId,
                ApplicantId = Guid.NewGuid(),
                PeteRequestLogId = Guid.NewGuid()
            });

            var handler = new DeleteTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            const string expectedMessage = "Transcript request cannot have a request log.";
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be(expectedMessage);
            VerifyLog(LogLevel.Warning, expectedMessage);
        }

        private void VerifyLog(LogLevel loglevel, string expectedMessage)
        {
            Mock.Get(_logger).Verify(m => m.Log(
                loglevel,
                It.IsAny<EventId>(),
                It.Is<FormattedLogValues>(v => v.ToString().Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));
        }
    }
}
