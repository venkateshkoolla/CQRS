using System;
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
    public class VerifyProfileHandlerTests
    {
        [Fact]
        [UnitTest("Handlers")]
        public async Task VerifyProfileHandler_ShouldPass()
        {
            // Arrange
            var request = new VerifyProfile
            {
                ApplicantId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>()
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetContact(It.Is<Guid>(g => g == request.ApplicantId))).ReturnsAsync(new Dto.Contact { Id = request.ApplicantId, ContactType = Domestic.Enums.ContactType.Applicant });

            var handler = new VerifyProfileHandler(Mock.Of<ILogger<VerifyProfileHandler>>(), domesticContext.Object);

            // Act
            await handler.Handle(request, CancellationToken.None);

            // Assert
            domesticContext.Verify(x => x.UpdateContact(It.Is<Dto.Contact>(c => !c.LastLoginExceed && c.Id == request.ApplicantId)), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void VerifyProfileHandler_ShouldThrow_When_NoApplicant()
        {
            // Arrange
            var request = new VerifyProfile
            {
                ApplicantId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>()
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetContact(It.Is<Guid>(g => g == request.ApplicantId))).ReturnsAsync((Dto.Contact)null);

            var handler = new VerifyProfileHandler(Mock.Of<ILogger<VerifyProfileHandler>>(), domesticContext.Object);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .WithMessage($"Applicant {request.ApplicantId} not found");
            domesticContext.Verify(x => x.UpdateContact(It.IsAny<Dto.Contact>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void VerifyProfileHandler_ShouldThrow_When_NotApplicant()
        {
            // Arrange
            var request = new VerifyProfile
            {
                ApplicantId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>()
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetContact(It.Is<Guid>(g => g == request.ApplicantId))).ReturnsAsync(new Dto.Contact { Id = request.ApplicantId, ContactType = Domestic.Enums.ContactType.FacilityEmployee });

            var handler = new VerifyProfileHandler(Mock.Of<ILogger<VerifyProfileHandler>>(), domesticContext.Object);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
            domesticContext.Verify(x => x.UpdateContact(It.IsAny<Dto.Contact>()), Times.Never);
        }
    }
}
