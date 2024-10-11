using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class CompleteTranscriptsHandlerTests
    {
        private readonly IPrincipal _user;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ILogger<CompleteTranscriptsHandler> _logger;

        public CompleteTranscriptsHandlerTests()
        {
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _user = Mock.Of<IPrincipal>();
            _logger = Mock.Of<ILogger<CompleteTranscriptsHandler>>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CompleteTranscripts_ShouldPass()
        {
            // Arrange
            var programChoices = new List<Dto.ProgramChoice>
            {
                new Dto.ProgramChoice()
            };
            var application = new Dto.Application
            {
                Id = Guid.NewGuid(),
                CompletedSteps = (int)ApplicationCompletedSteps.ProgramChoice
            };
            var request = new CompleteTranscripts
            {
                User = _user,
                ApplicationId = application.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(programChoices as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);
            var handler = new CompleteTranscriptsHandler(_logger, domesticContextMock.Object, _userAuthorization);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.UpdateApplication(It.IsAny<Dto.Application>()), Times.Once);
            application.CompletedSteps.Should().Be((int)ApplicationCompletedSteps.TranscriptRequests);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CompleteTranscripts_ShouldPass_WhenAlreadyCompleted()
        {
            // Arrange
            var programChoices = new List<Dto.ProgramChoice>
            {
                new Dto.ProgramChoice()
            };
            var application = new Dto.Application
            {
                Id = Guid.NewGuid(),
                CompletedSteps = (int)ApplicationCompletedSteps.TranscriptRequests
            };
            var request = new CompleteTranscripts
            {
                User = _user,
                ApplicationId = application.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(programChoices as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);
            var handler = new CompleteTranscriptsHandler(_logger, domesticContextMock.Object, _userAuthorization);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.UpdateApplication(It.IsAny<Dto.Application>()), Times.Never);
            application.CompletedSteps.Should().Be((int)ApplicationCompletedSteps.TranscriptRequests);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CompleteTranscripts_ShouldThrow_WhenNoApplications()
        {
            // Arrange
            var programChoices = new List<Dto.ProgramChoice>();
            var request = new CompleteTranscripts
            {
                User = _user,
                ApplicationId = Guid.NewGuid()
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(programChoices as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync((Dto.Application)null);
            var handler = new CompleteTranscriptsHandler(_logger, domesticContextMock.Object, _userAuthorization);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().WithMessage($"Application {request.ApplicationId} not found");
            domesticContextMock.Verify(e => e.UpdateApplication(It.IsAny<Dto.Application>()), Times.Never);
        }
    }
}
