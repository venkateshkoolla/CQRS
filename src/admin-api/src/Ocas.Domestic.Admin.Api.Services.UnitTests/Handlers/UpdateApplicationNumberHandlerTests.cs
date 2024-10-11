using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Apply.TestFramework.RuleCollections;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class UpdateApplicationNumberHandlerTests
    {
        private const string EightNumbers = "########";

        private readonly ILogger<UpdateApplicationNumberHandler> _logger;
        private readonly IApiMapper _apiMapper;
        private readonly Faker _faker;
        private readonly IPrincipal _user;
        private readonly ILookupsCache _lookupsCache;
        private readonly AllLookups _lookups;

        public UpdateApplicationNumberHandlerTests()
        {
            _logger = Mock.Of<ILogger<UpdateApplicationNumberHandler>>();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _user = Mock.Of<IPrincipal>();
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _lookups = XunitInjectionCollection.ModelFakerFixture.AllAdminLookups;
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicationHandler_ShouldPass_When_NineNumbers()
        {
            // Arrange
            var applicationCycle = _lookups.ApplicationCycles.OrderByDescending(x => x.Year).First();
            var request = new UpdateApplicationNumber
            {
                ApplicationId = Guid.NewGuid(),
                Number = _faker.GenerateApplicationNumber(applicationCycle.Year),
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.IsDuplicateApplication(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
            domesticContext.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = request.ApplicationId, ApplicationCycleId = applicationCycle.Id });
            domesticContext.Setup(m => m.UpdateApplication(It.IsAny<Dto.Application>())).ReturnsAsync(new Dto.Application { Id = request.ApplicationId, ApplicationNumber = request.Number });

            var handler = new UpdateApplicationNumberHandler(_logger, userAuthorization.Object, domesticContext.Object, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContext.Verify(m => m.UpdateApplication(It.Is<Dto.Application>(a => a.ApplicationNumber == request.Number)), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicationHandler_ShouldPass_When_EightNumbers()
        {
            // Arrange
            var applicationCycle = _lookups.ApplicationCycles.OrderByDescending(x => x.Year).First();
            var request = new UpdateApplicationNumber
            {
                ApplicationId = Guid.NewGuid(),
                Number = _faker.GenerateApplicationNumber(applicationCycle.Year, EightNumbers),
                User = _user
            };
            var formattedNumber = request.Number.Insert(0, "0");

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.IsDuplicateApplication(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
            domesticContext.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = request.ApplicationId, ApplicationCycleId = applicationCycle.Id });
            domesticContext.Setup(m => m.UpdateApplication(It.IsAny<Dto.Application>())).ReturnsAsync(new Dto.Application { Id = request.ApplicationId, ApplicationNumber = formattedNumber });

            var handler = new UpdateApplicationNumberHandler(_logger, userAuthorization.Object, domesticContext.Object, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContext.Verify(m => m.UpdateApplication(It.Is<Dto.Application>(a => a.ApplicationNumber == formattedNumber)), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicationHandler_ShouldFail_When_NotOcasUser()
        {
            // Arrange
            var applicationCycle = _lookups.ApplicationCycles.OrderByDescending(x => x.Year).First();
            var request = new UpdateApplicationNumber
            {
                ApplicationId = Guid.NewGuid(),
                Number = _faker.GenerateApplicationNumber(applicationCycle.Year),
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(_faker.PickRandomWithout(UserType.OcasUser));

            var domesticContext = new DomesticContextMock();

            var handler = new UpdateApplicationNumberHandler(_logger, userAuthorization.Object, domesticContext.Object, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
            domesticContext.Verify(m => m.UpdateApplication(It.IsAny<Dto.Application>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicationHandler_ShouldFail_When_DuplicateApplication()
        {
            // Arrange
            var applicationCycle = _lookups.ApplicationCycles.OrderByDescending(x => x.Year).First();
            var request = new UpdateApplicationNumber
            {
                ApplicationId = Guid.NewGuid(),
                Number = _faker.GenerateApplicationNumber(applicationCycle.Year),
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.IsDuplicateApplication(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(true);

            var handler = new UpdateApplicationNumberHandler(_logger, userAuthorization.Object, domesticContext.Object, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ConflictException>()
                .WithMessage($"Application with {request.Number} already exists.");
            domesticContext.Verify(m => m.UpdateApplication(It.IsAny<Dto.Application>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateApplicationHandler_ShouldFail_When_ApplicationNotFound()
        {
            // Arrange
            var applicationCycle = _lookups.ApplicationCycles.OrderByDescending(x => x.Year).First();
            var request = new UpdateApplicationNumber
            {
                ApplicationId = Guid.NewGuid(),
                Number = _faker.GenerateApplicationNumber(applicationCycle.Year),
                User = _user
            };

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.IsDuplicateApplication(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync(false);
            domesticContext.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync((Dto.Application)null);

            var handler = new UpdateApplicationNumberHandler(_logger, userAuthorization.Object, domesticContext.Object, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .WithMessage($"Application not found: {request.ApplicationId}");
            domesticContext.Verify(m => m.UpdateApplication(It.IsAny<Dto.Application>()), Times.Never);
        }
    }
}
