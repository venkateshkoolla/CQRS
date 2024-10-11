using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.TestFramework;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Authorization
{
    public class UserAuthorizationTests
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _models;
        private readonly AppSettingsMock _appSettingsMock = new AppSettingsMock();

        public UserAuthorizationTests()
        {
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldPass_When_OcasUser()
        {
            // Arrange
            var user = TestConstants.TestUser.Ocas.TestPrincipal;

            var domesticContext = new Mock<IDomesticContext>();
            domesticContext.Setup(m => m.GetContactSubjectId(It.IsAny<Guid>())).ReturnsAsync("applicantId");

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContext.Object, _lookupsCache, _appSettingsMock);
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, Guid.NewGuid());

            // Assert
            func.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldPass_When_HSUser()
        {
            // Arrange
            var user = TestConstants.TestUser.HsUser.TestPrincipal;

            var domesticContext = new Mock<IDomesticContext>();
            domesticContext.Setup(m => m.CanAccessApplicant(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserType>())).ReturnsAsync(true);

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContext.Object, _lookupsCache, _appSettingsMock);
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, Guid.NewGuid());

            // Assert
            func.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldPass_When_CollegeUser()
        {
            // Arrange
            var user = TestConstants.TestUser.College.TestPrincipal;

            var domesticContext = new Mock<IDomesticContext>();
            domesticContext.Setup(m => m.CanAccessApplicant(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<UserType>())).ReturnsAsync(true);

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContext.Object, _lookupsCache, _appSettingsMock);
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, Guid.NewGuid());

            // Assert
            func.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldThrow_When_BoardUser()
        {
            // Arrange
            var user = TestConstants.TestUser.HsBoard.TestPrincipal;

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, Guid.NewGuid());

            // Assert
            func.Should().Throw<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldThrow_When_OcasUser_NoApplicant()
        {
            // Arrange
            var user = TestConstants.TestUser.Ocas.TestPrincipal;

            var domesticContext = new Mock<IDomesticContext>();
            domesticContext.Setup(m => m.GetContactSubjectId(It.IsAny<Guid>())).ReturnsAsync(string.Empty);

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContext.Object, _lookupsCache, _appSettingsMock);
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, Guid.NewGuid());

            // Assert
            func.Should().Throw<NotFoundException>();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessApplicantAsync_ShouldThrow_When_NoRole()
        {
            // Arrange
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("customer_code", Constants.IdentityServer.Partner.Customer)
                    }));

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            Func<Task> func = () => authorization.CanAccessApplicantAsync(user, Guid.NewGuid());

            // Assert
            func.Should().Throw<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessCollegeAsync_ShouldPass_When_OcasUser()
        {
            // Arrange
            var user = TestConstants.TestUser.Ocas.TestPrincipal;

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            Func<Task> func = () => authorization.CanAccessCollegeAsync(user, Guid.NewGuid());

            // Assert
            func.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessCollegeAsync_ShouldPass_When_CollegeUser_AccessingOwn()
        {
            // Arrange
            var user = TestConstants.TestUser.College.TestPrincipal;
            var collegeId = _models.AllAdminLookups.Colleges.First(c => c.Code == TestConstants.TestUser.College.PartnerId).Id;

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            Func<Task> func = () => authorization.CanAccessCollegeAsync(user, collegeId);

            // Assert
            func.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessCollegeAsync_ShouldThrow_When_CollegeEmpty()
        {
            // Arrange
            var user = TestConstants.TestUser.College.TestPrincipal;

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            Func<Task> func = () => authorization.CanAccessCollegeAsync(user, Guid.Empty);

            // Assert
            func.Should().Throw<ValidationException>().WithMessage($"College does not exist: {Guid.Empty}");
        }

        [Fact]
        [UnitTest("Authorization")]
        public void CanAccessCollegeAsync_ShouldThrow_When_CollegeUser_AccessingDifferentCollege()
        {
            // Arrange
            var user = TestConstants.TestUser.College.TestPrincipal;
            var collegeId = _models.AllAdminLookups.Colleges.First(c => c.Code != TestConstants.TestUser.College.PartnerId).Id;

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            Func<Task> func = () => authorization.CanAccessCollegeAsync(user, collegeId);

            // Assert
            func.Should().Throw<NotAuthorizedException>().WithMessage("User does not have access to college");
        }

        [Fact]
        [UnitTest("Authorization")]
        public void IsOcasTier2User_ShouldPass_When_OcasUser_BO_Role()
        {
            // Arrange
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", "vishwatest_20200130@mailinator.com"),
                        new Claim(ClaimTypes.Role, "BO")
                    }));
            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            var result = authorization.IsOcasTier2User(user);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void IsOcasTier2User_ShouldPass_When_OcasUser_PortalAdmin_Role()
        {
            // Arrange
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", "vishwatest_20200130@mailinator.com"),
                        new Claim(ClaimTypes.Role, "PortalAdmin")
                    }));
            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            var result = authorization.IsOcasTier2User(user);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void IsOcasTier2User_ShouldThrow_When_OcasUser_CCC_Role()
        {
            // Arrange
            var user = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", "vishwatest_20200130@mailinator.com"),
                        new Claim(ClaimTypes.Role, "CCC")
                    }));
            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            var result = authorization.IsOcasTier2User(user);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void IsHighSchoolUser_ShouldPass_When_HSBoardUser()
        {
            // Arrange
            var user = TestConstants.TestUser.HsBoard.TestPrincipal;

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            var result = authorization.IsHighSchoolUser(user);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void IsHighSchoolUser_ShouldPass_When_HSUser()
        {
            // Arrange
            var user = TestConstants.TestUser.HsUser.TestPrincipal;

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            var result = authorization.IsHighSchoolUser(user);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Authorization")]
        public void IsHighSchoolUser_ShouldPass_When_CollegeUser()
        {
            // Arrange
            var user = TestConstants.TestUser.College.TestPrincipal;

            // Act
            var authorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettingsMock);
            var result = authorization.IsHighSchoolUser(user);

            // Assert
            result.Should().BeFalse();
        }
    }
}
