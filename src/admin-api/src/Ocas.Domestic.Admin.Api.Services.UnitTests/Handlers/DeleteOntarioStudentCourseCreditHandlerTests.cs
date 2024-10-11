using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class DeleteOntarioStudentCourseCreditHandlerTests
    {
        private readonly IUserAuthorization _userAuthorization;

        public DeleteOntarioStudentCourseCreditHandlerTests()
        {
            _userAuthorization = Mock.Of<IUserAuthorization>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeleteOntarioStudentCourseCredit_ShouldPass()
        {
            // Arrange
            var request = new DeleteOntarioStudentCourseCredit
            {
                OntarioStudentCourseCreditId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>()
            };

            var applicantId = Guid.NewGuid();
            var applications = new List<Dto.Application> { new Dto.Application { ApplicantId = applicantId } } as IList<Dto.Application>;
            var domesticContextMock = new DomesticContextMock();

            domesticContextMock.Setup(m => m.GetApplications(It.IsAny<Guid>())).ReturnsAsync(applications);
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredit(It.IsAny<Guid>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit { Id = request.OntarioStudentCourseCreditId });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasTier2User(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new DeleteOntarioStudentCourseCreditHandler(Mock.Of<ILogger<DeleteOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, userAuthorizationMock.Object);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.DeleteOntarioStudentCourseCredit(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeleteOntarioStudentCourseCredit_ShouldThrow_When_RequestNotFound()
        {
            // Arrange
            var request = new DeleteOntarioStudentCourseCredit
            {
                OntarioStudentCourseCreditId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>()
            };
            var domesticContextMock = new DomesticContextMock();

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasTier2User(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new DeleteOntarioStudentCourseCreditHandler(Mock.Of<ILogger<DeleteOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, userAuthorizationMock.Object);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().WithMessage("Ontario Student Course Credit Id not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeleteOntarioStudentCourseCredit_ShouldThrow_When_NoAccess()
        {
            // Arrange
            var request = new DeleteOntarioStudentCourseCredit
            {
                OntarioStudentCourseCreditId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>()
            };
            var domesticContextMock = new DomesticContextMock();

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasTier2User(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new DeleteOntarioStudentCourseCreditHandler(Mock.Of<ILogger<DeleteOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, userAuthorizationMock.Object);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }
    }
}
