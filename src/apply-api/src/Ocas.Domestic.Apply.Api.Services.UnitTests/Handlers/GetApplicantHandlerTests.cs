using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Core.Settings;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Data.Extras;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class GetApplicantHandlerTests
    {
        private readonly ILookupsCache _lookupCache;
        private readonly IApiMapper _apiMapper;
        private readonly IAppSettings _appSettings;

        public GetApplicantHandlerTests()
        {
            _lookupCache = XunitInjectionCollection.LookupsCache;
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();

            var appSettings = new Mock<IAppSettings>();
            appSettings.Setup(x => x.GetAppSetting<int>(It.Is<string>(s => s == Constants.AppSettings.LoginExpiryMonths))).Returns(1);
            _appSettings = appSettings.Object;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetApplicant_ShouldPass_When_Login_NotExceeded()
        {
            // Arrange

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {
                new Claim("upn", "vishwatest_08012019@mailinator.com"),
                new Claim("sub", Guid.NewGuid().ToString())
                }));

            var request = new GetApplicant
            {
                User = user
            };

            var lastLogin = DateTime.UtcNow;
            var contact = new Dto.Contact
            {
                Id = Guid.Parse(request.User.GetUserId()),
                Username = request.User.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                BirthDate = DateTime.UtcNow,
                LastLogin = lastLogin
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetContact(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(contact);
            domesticContextMock.Setup(x => x.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(CompletedSteps.MyApplication);

            var domesticContextExtrasMock = new Mock<IDomesticContextExtras>();
            domesticContextExtrasMock.Setup(m => m.PatchEducationStatus(It.IsAny<Dto.Contact>(), It.IsAny<string>(), It.IsAny<List<Dto.BasisForAdmission>>(), It.IsAny<List<Dto.Current>>(), It.IsAny<List<Dto.ApplicationCycle>>())).ReturnsAsync(false);

            var handler = new GetApplicantHandler(Mock.Of<ILogger<GetApplicantHandler>>(), domesticContextMock.Object, domesticContextExtrasMock.Object, _apiMapper, _lookupCache, _appSettings, Mock.Of<IUserAuthorization>());

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(contact.Id);
            response.LastLogin.Should().BeCloseTo(lastLogin, 10.Seconds());
            response.LastLoginExceed.Should().BeFalse();

            domesticContextMock.Verify(x => x.UpdateContact(It.Is<Dto.Contact>(c => c.LastLogin > lastLogin)), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetApplicant_ShouldPass_When_Login_Exceeded()
        {
            // Arrange

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {
                new Claim("upn", "vishwatest_08012019@mailinator.com"),
                new Claim("sub", Guid.NewGuid().ToString())
                }));

            var request = new GetApplicant
            {
                User = user
            };

            var lastLogin = DateTime.UtcNow;
            var contact = new Dto.Contact
            {
                Id = Guid.Parse(request.User.GetUserId()),
                Username = request.User.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                BirthDate = DateTime.UtcNow,
                LastLogin = lastLogin.AddMonths(-10)
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetContact(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(contact);
            domesticContextMock.Setup(x => x.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(CompletedSteps.MyApplication);

            var domesticContextExtrasMock = new Mock<IDomesticContextExtras>();
            domesticContextExtrasMock.Setup(m => m.PatchEducationStatus(It.IsAny<Dto.Contact>(), It.IsAny<string>(), It.IsAny<List<Dto.BasisForAdmission>>(), It.IsAny<List<Dto.Current>>(), It.IsAny<List<Dto.ApplicationCycle>>())).ReturnsAsync(false);

            var handler = new GetApplicantHandler(Mock.Of<ILogger<GetApplicantHandler>>(), domesticContextMock.Object, domesticContextExtrasMock.Object, _apiMapper, _lookupCache, _appSettings, Mock.Of<IUserAuthorization>());

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(contact.Id);
            response.LastLogin.Should().Be(contact.LastLogin);
            response.LastLoginExceed.Should().BeTrue();

            domesticContextMock.Verify(x => x.UpdateContact(It.Is<Dto.Contact>(c => c.LastLoginExceed && c.LastLogin == contact.LastLogin)), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetApplicant_ShouldPass_When_Profile_Incomplete_Then_Not_Exceeded()
        {
            // Arrange
            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {
                new Claim("upn", "vishwatest_08012019@mailinator.com"),
                new Claim("sub", Guid.NewGuid().ToString())
                }));

            var request = new GetApplicant
            {
                User = user
            };

            var lastLogin = DateTime.UtcNow;
            var contact = new Dto.Contact
            {
                Id = Guid.Parse(request.User.GetUserId()),
                Username = request.User.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                BirthDate = DateTime.UtcNow,
                LastLogin = lastLogin.AddMonths(-2)
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetContact(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(contact);
            domesticContextMock.Setup(x => x.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(CompletedSteps.Education);

            var domesticContextExtrasMock = new Mock<IDomesticContextExtras>();
            domesticContextExtrasMock.Setup(m => m.PatchEducationStatus(It.IsAny<Dto.Contact>(), It.IsAny<string>(), It.IsAny<List<Dto.BasisForAdmission>>(), It.IsAny<List<Dto.Current>>(), It.IsAny<List<Dto.ApplicationCycle>>())).ReturnsAsync(false);

            var handler = new GetApplicantHandler(Mock.Of<ILogger<GetApplicantHandler>>(), domesticContextMock.Object, domesticContextExtrasMock.Object, _apiMapper, _lookupCache, _appSettings, Mock.Of<IUserAuthorization>());

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(contact.Id);
            response.LastLogin.Should().Be(contact.LastLogin);
            response.LastLoginExceed.Should().BeFalse();

            domesticContextMock.Verify(x => x.UpdateContact(It.Is<Dto.Contact>(c => !c.LastLoginExceed && c.LastLogin >= contact.LastLogin)), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicant_ShouldThrow_NotApplicant()
        {
            // Arrange
            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {
                new Claim("upn", "vishwatest_08012019@mailinator.com"),
                new Claim("sub", Guid.NewGuid().ToString())
                }));

            var request = new GetApplicant
            {
                User = user
            };

            var contact = new Dto.Contact
            {
                Id = Guid.Parse(request.User.GetUserId()),
                Username = request.User.GetUpnOrEmail(),
                ContactType = ContactType.NonOCASApplicant,
                BirthDate = DateTime.UtcNow
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetContact(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(contact);

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            var handler = new GetApplicantHandler(Mock.Of<ILogger<GetApplicantHandler>>(), domesticContextMock.Object, domesticContextExtras, _apiMapper, _lookupCache, _appSettings, Mock.Of<IUserAuthorization>());

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicant_ShouldThrow_NoApplicantFound()
        {
            // Arrange

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {
                new Claim("upn", "vishwatest_08012019@mailinator.com"),
                new Claim("sub", Guid.NewGuid().ToString())
                }));

            var request = new GetApplicant
            {
                User = user
            };

            var domesticContextMock = new DomesticContextMock();

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            var handler = new GetApplicantHandler(Mock.Of<ILogger<GetApplicantHandler>>(), domesticContextMock.Object, domesticContextExtras, _apiMapper, _lookupCache, _appSettings, Mock.Of<IUserAuthorization>());

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().And.Message.Should().Be($"Applicant {request.User.GetSubject()} not found");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetApplicant_ShouldPass_When_Login_NotExceeded_AsOcas()
        {
            // Arrange
            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {
                new Claim("upn", "vishwatest_08012019@mailinator.com"),
                new Claim("sub", Guid.NewGuid().ToString())
                }));

            var request = new GetApplicant
            {
                ApplicantId = Guid.Parse(user.GetUserId()),
                User = user
            };

            var lastLogin = DateTime.UtcNow;
            var contact = new Dto.Contact
            {
                Id = request.ApplicantId.Value,
                Username = user.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                BirthDate = DateTime.UtcNow,
                LastLogin = lastLogin
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);
            domesticContextMock.Setup(x => x.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(CompletedSteps.MyApplication);

            var domesticContextExtrasMock = new Mock<IDomesticContextExtras>();
            domesticContextExtrasMock.Setup(m => m.PatchEducationStatus(It.IsAny<Dto.Contact>(), It.IsAny<string>(), It.IsAny<List<Dto.BasisForAdmission>>(), It.IsAny<List<Dto.Current>>(), It.IsAny<List<Dto.ApplicationCycle>>())).ReturnsAsync(false);

            var handler = new GetApplicantHandler(Mock.Of<ILogger<GetApplicantHandler>>(), domesticContextMock.Object, domesticContextExtrasMock.Object, _apiMapper, _lookupCache, _appSettings, Mock.Of<IUserAuthorization>());

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(contact.Id);
            response.LastLogin.Should().BeAfter(lastLogin);
            response.LastLoginExceed.Should().BeFalse();

            domesticContextMock.Verify(x => x.UpdateContact(It.IsAny<Dto.Contact>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetApplicant_ShouldPass_When_Login_Exceeded_AsOcas()
        {
            // Arrange

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {
                new Claim("upn", "vishwatest_08012019@mailinator.com"),
                new Claim("sub", Guid.NewGuid().ToString())
                }));

            var request = new GetApplicant
            {
                ApplicantId = Guid.Parse(user.GetUserId()),
                User = user
            };

            var lastLogin = DateTime.UtcNow;
            var contact = new Dto.Contact
            {
                Id = request.ApplicantId.Value,
                Username = user.GetUpnOrEmail(),
                ContactType = ContactType.Applicant,
                BirthDate = DateTime.UtcNow,
                LastLogin = lastLogin.AddMonths(-10)
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);
            domesticContextMock.Setup(x => x.GetCompletedStep(It.IsAny<Guid>())).ReturnsAsync(CompletedSteps.MyApplication);

            var domesticContextExtrasMock = new Mock<IDomesticContextExtras>();
            domesticContextExtrasMock.Setup(m => m.PatchEducationStatus(It.IsAny<Dto.Contact>(), It.IsAny<string>(), It.IsAny<List<Dto.BasisForAdmission>>(), It.IsAny<List<Dto.Current>>(), It.IsAny<List<Dto.ApplicationCycle>>())).ReturnsAsync(false);

            var handler = new GetApplicantHandler(Mock.Of<ILogger<GetApplicantHandler>>(), domesticContextMock.Object, domesticContextExtrasMock.Object, _apiMapper, _lookupCache, _appSettings, Mock.Of<IUserAuthorization>());

            // Act
            var response = await handler.Handle(request, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(contact.Id);
            response.LastLogin.Should().Be(contact.LastLogin);
            response.LastLoginExceed.Should().BeTrue();

            domesticContextMock.Verify(x => x.UpdateContact(It.IsAny<Dto.Contact>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicant_ShouldThrow_NotApplicant_AsOcas()
        {
            // Arrange
            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {
                new Claim("upn", "vishwatest_08012019@mailinator.com"),
                new Claim("sub", Guid.NewGuid().ToString())
                }));

            var request = new GetApplicant
            {
                ApplicantId = Guid.Parse(user.GetUserId()),
                User = user
            };

            var contact = new Dto.Contact
            {
                Id = request.ApplicantId.Value,
                Username = user.GetUpnOrEmail(),
                ContactType = ContactType.NonOCASApplicant,
                BirthDate = DateTime.UtcNow
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            var handler = new GetApplicantHandler(Mock.Of<ILogger<GetApplicantHandler>>(), domesticContextMock.Object, domesticContextExtras, _apiMapper, _lookupCache, _appSettings, Mock.Of<IUserAuthorization>());

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicant_ShouldThrow_NoApplicantFound_AsOcas()
        {
            // Arrange

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(new[] {
                new Claim("upn", "vishwatest_08012019@mailinator.com"),
                new Claim("sub", Guid.NewGuid().ToString())
                }));

            var request = new GetApplicant
            {
                ApplicantId = Guid.Parse(user.GetUserId()),
                User = user
            };

            var domesticContextMock = new DomesticContextMock();

            var domesticContextExtras = new DomesticContextExtras(domesticContextMock.Object);

            var handler = new GetApplicantHandler(Mock.Of<ILogger<GetApplicantHandler>>(), domesticContextMock.Object, domesticContextExtras, _apiMapper, _lookupCache, _appSettings, Mock.Of<IUserAuthorization>());

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().And.Message.Should().Be($"Applicant {request.ApplicantId} not found");
        }
    }
}
