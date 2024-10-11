using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetOfferHistoriesHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly TestFramework.ModelFakerFixture _models;
        private readonly ILogger<GetOfferHistoriesHandler> _logger;
        private readonly IApiMapper _apiMapper;
        private readonly RequestCache _requestCache;
        private readonly IPrincipal _user;

        public GetOfferHistoriesHandlerTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _logger = Mock.Of<ILogger<GetOfferHistoriesHandler>>();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _requestCache = XunitInjectionCollection.RequestCacheMock;
            _user = Mock.Of<IPrincipal>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetOfferHistoriesHandler_ShouldPass()
        {
            // Arrange
            var offersAcceptancesMock = new List<OfferAcceptance>
            {
                new OfferAcceptance { ApplicationId = Guid.NewGuid() }
            };

            var request = new GetOfferHistories
            {
                ApplicationId = (Guid)offersAcceptancesMock.FirstOrDefault().ApplicationId,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Application { Id = request.ApplicationId });
            domesticContextMock.Setup(m => m.GetOfferAcceptances(It.IsAny<GetOfferAcceptancesOptions>(), It.IsAny<Locale>())).ReturnsAsync(offersAcceptancesMock);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new GetOfferHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorization.Object, _requestCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().OnlyContain(r => r.ApplicationId == request.ApplicationId);
            domesticContextMock.Verify(e => e.GetOfferAcceptances(It.IsAny<GetOfferAcceptancesOptions>(), It.IsAny<Locale>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetOfferHistoriesHandler_ShouldPass_When_NoOffers_NewApplication()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var request = new GetOfferHistories
            {
                ApplicationId = application.Id,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Application { Id = request.ApplicationId });
            domesticContextMock.Setup(m => m.GetOfferAcceptances(It.IsAny<GetOfferAcceptancesOptions>(), It.IsAny<Locale>())).ReturnsAsync(new List<OfferAcceptance>());

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new GetOfferHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorization.Object, _requestCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetOfferHistoriesHandler_ShouldThrow_When_NotOcasUser()
        {
            // Arrange
            var request = new GetOfferHistories
            {
                ApplicationId = Guid.NewGuid(),
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new GetOfferHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorization.Object, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetOfferHistoriesHandler_ShouldThrow_When_NoApplicationFound()
        {
            // Arrange
            var request = new GetOfferHistories
            {
                ApplicationId = Guid.NewGuid(),
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new GetOfferHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorization.Object, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .WithMessage($"No application found with id: {request.ApplicationId}");
        }
    }
}
