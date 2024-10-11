using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Data;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Authorization
{
    public class UserAuthorizationTests
    {
        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldPass_When_SubjectIdMatchUpn()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", "alextest_20181217@mailinator.com"),
                        new Claim("sub", Guid.NewGuid().ToString())
                    }));

            var domesticContext = new Mock<IDomesticContext>();
            domesticContext.Setup(m => m.GetContactSubjectId(It.IsAny<Guid>())).ReturnsAsync("ALEXTEST_20181217@mailinator.com");
            domesticContext.Setup(m => m.IsActive(It.IsAny<Guid>())).ReturnsAsync(true);
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContext.Object, new AppSettingsMock());

            // Act
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, applicantId);

            // Assert
            func.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldPass_When_OcasUser()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new[]
                        {
                            new Claim("role", TestConstants.Identity.OcasRole)
                        },
                        "Bearer",
                        "name",
                        "role"));

            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), new AppSettingsMock());

            // Act
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, applicantId);

            // Assert
            func.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldPass_When_SubjectIdMatchSub()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var subjectId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", "alextest_20181217@mailinator.com"),
                        new Claim("sub", subjectId)
                    }));

            var domesticContext = new Mock<IDomesticContext>();
            domesticContext.Setup(m => m.GetContactSubjectId(It.IsAny<Guid>())).ReturnsAsync(subjectId);
            domesticContext.Setup(m => m.IsActive(It.IsAny<Guid>())).ReturnsAsync(true);
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContext.Object, new AppSettingsMock());

            // Act
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, applicantId);

            // Assert
            func.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldFail_When_ApplicantIdEmpty()
        {
            // Arrange
            var applicantId = Guid.Empty;
            var user = Mock.Of<IPrincipal>();
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), new AppSettingsMock());

            // Act
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, applicantId);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("'Applicant Id' must not be empty.");
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldFail_When_ApplicantNotFound()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", "alextest_20181217@mailinator.com"),
                        new Claim("sub", Guid.NewGuid().ToString())
                    }));

            var domesticContext = new Mock<IDomesticContext>();
            domesticContext.Setup(m => m.GetContactSubjectId(It.IsAny<Guid>())).ReturnsAsync(string.Empty);
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContext.Object, new AppSettingsMock());

            // Act
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, applicantId);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be($"Applicant {applicantId} not found");
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldFail_When_UpnMismatch()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", "alextest_20181217@mailinator.com"),
                        new Claim("sub", Guid.NewGuid().ToString())
                    }));

            var domesticContext = new Mock<IDomesticContext>();
            domesticContext.Setup(m => m.GetContactSubjectId(It.IsAny<Guid>())).ReturnsAsync("alextest_20181217@test.ocas.ca");
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContext.Object, new AppSettingsMock());

            // Act
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, applicantId);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldFail_When_SubjectMismatch()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", "alextest_20181217@mailinator.com"),
                        new Claim("sub", Guid.NewGuid().ToString())
                    }));

            var domesticContext = new Mock<IDomesticContext>();
            domesticContext.Setup(m => m.GetContactSubjectId(It.IsAny<Guid>())).ReturnsAsync(Guid.NewGuid().ToString());
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContext.Object, new AppSettingsMock());

            // Act
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, applicantId);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldFail_When_ApplicantNotActive()
        {
            // Arrange
            var applicantId = Guid.NewGuid();
            var subjectId = Guid.NewGuid().ToString();
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", "alextest_20181217@mailinator.com"),
                        new Claim("sub", subjectId)
                    }));

            var domesticContext = new Mock<IDomesticContext>();
            domesticContext.Setup(m => m.GetContactSubjectId(It.IsAny<Guid>())).ReturnsAsync(subjectId);
            domesticContext.Setup(m => m.IsActive(It.IsAny<Guid>())).ReturnsAsync(false);
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContext.Object, new AppSettingsMock());

            // Act
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, applicantId);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Applicant must be active");
        }
    }
}
