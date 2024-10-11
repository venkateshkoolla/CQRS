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
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class RemoveEducationHandlerTests
    {
        private readonly ModelFakerFixture _models;

        public RemoveEducationHandlerTests()
        {
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveEducationHandler_ShouldPass()
        {
            // Arrange
            var request = new RemoveEducation
            {
                EducationId = _models.GetEducation().Generate().Id,
                User = Mock.Of<IPrincipal>()
            };

            var applicantId = Guid.NewGuid();
            var applications = new List<Dto.Application> { new Dto.Application { ApplicantId = applicantId } } as IList<Dto.Application>;

            var domesticContextMock = new DomesticContextMock();

            domesticContextMock.Setup(m => m.GetApplications(It.IsAny<Guid>())).ReturnsAsync(applications);
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(new Dto.Education { ApplicantId = applicantId, HasMoreThanOneEducation = true, HasPaidApplication = false, HasTranscripts = false });

            var userAuthorization = Mock.Of<IUserAuthorization>();
            var handler = new RemoveEducationHandler(Mock.Of<ILogger<RemoveEducationHandler>>(), domesticContextMock.Object, userAuthorization);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.DeleteEducation(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveEducationHandler_ShouldThrow_When_RequestHasNoApplicant()
        {
            // Arrange
            var request = new RemoveEducation
            {
                EducationId = _models.GetEducation().Generate().Id,
                User = Mock.Of<IPrincipal>()
            };
            var domesticContextMock = new DomesticContextMock();

            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(new Dto.Education());

            var userAuthorization = Mock.Of<IUserAuthorization>();
            var handler = new RemoveEducationHandler(Mock.Of<ILogger<RemoveEducationHandler>>(), domesticContextMock.Object, userAuthorization);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be("Education request does not have an applicant.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveEducationHandler_ShouldThrow_When_RequestNotFound()
        {
            // Arrange
            var request = new RemoveEducation
            {
                EducationId = _models.GetEducation().Generate().Id,
                User = Mock.Of<IPrincipal>()
            };
            var domesticContextMock = new DomesticContextMock();
            var userAuthorization = Mock.Of<IUserAuthorization>();
            var handler = new RemoveEducationHandler(Mock.Of<ILogger<RemoveEducationHandler>>(), domesticContextMock.Object, userAuthorization);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().And.Message.Should().Be("Education request not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveEducationHandler_ShouldThrow_When_HasMoreThanOneEducation()
        {
            // Arrange
            var request = new RemoveEducation
            {
                EducationId = _models.GetEducation().Generate().Id,
                User = Mock.Of<IPrincipal>()
            };

            var applicantId = Guid.NewGuid();
            var applications = new List<Dto.Application> { new Dto.Application { ApplicantId = applicantId } } as IList<Dto.Application>;

            var domesticContextMock = new DomesticContextMock();

            domesticContextMock.Setup(m => m.GetApplications(It.IsAny<Guid>())).ReturnsAsync(applications);
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(new Dto.Education { ApplicantId = applicantId, HasMoreThanOneEducation = false });

            var handler = new RemoveEducationHandler(Mock.Of<ILogger<RemoveEducationHandler>>(), domesticContextMock.Object, Mock.Of<IUserAuthorization>());

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Cannot remove the only education record.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveEducationHandler_ShouldThrow_When_HasPaidApplication()
        {
            // Arrange
            var request = new RemoveEducation
            {
                EducationId = _models.GetEducation().Generate().Id,
                User = Mock.Of<IPrincipal>()
            };

            var applicantId = Guid.NewGuid();
            var applications = new List<Dto.Application> { new Dto.Application { ApplicantId = applicantId } } as IList<Dto.Application>;

            var domesticContextMock = new DomesticContextMock();

            domesticContextMock.Setup(m => m.GetApplications(It.IsAny<Guid>())).ReturnsAsync(applications);
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(new Dto.Education { ApplicantId = applicantId, HasMoreThanOneEducation = true, HasPaidApplication = true });

            var handler = new RemoveEducationHandler(Mock.Of<ILogger<RemoveEducationHandler>>(), domesticContextMock.Object, Mock.Of<IUserAuthorization>());

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Cannot remove education with paid application.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void RemoveEducationHandler_ShouldThrow_When_HasTranscripts()
        {
            // Arrange
            var request = new RemoveEducation
            {
                EducationId = _models.GetEducation().Generate().Id,
                User = Mock.Of<IPrincipal>()
            };

            var applicantId = Guid.NewGuid();
            var applications = new List<Dto.Application> { new Dto.Application { ApplicantId = applicantId } } as IList<Dto.Application>;

            var domesticContextMock = new DomesticContextMock();

            domesticContextMock.Setup(m => m.GetApplications(It.IsAny<Guid>())).ReturnsAsync(applications);
            domesticContextMock.Setup(m => m.GetEducation(It.IsAny<Guid>())).ReturnsAsync(new Dto.Education { ApplicantId = applicantId, HasMoreThanOneEducation = true, HasTranscripts = true });

            var handler = new RemoveEducationHandler(Mock.Of<ILogger<RemoveEducationHandler>>(), domesticContextMock.Object, Mock.Of<IUserAuthorization>());

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Cannot remove education with transcript requests.");
        }
    }
}
