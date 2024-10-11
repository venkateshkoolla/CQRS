using System;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Core.Settings;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.AppSettings.Extras;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class AcceptOfferHandlerTests
    {
        private readonly AllLookups _lookups;
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _models;
        private readonly IPrincipal _user;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IAppSettingsExtras _appSettingsExtras;
        private readonly ILogger<AcceptOfferHandler> _logger;

        public AcceptOfferHandlerTests()
        {
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _lookups = _models.AllApplyLookups;
            _user = Mock.Of<IPrincipal>();
            var appSettings = Mock.Of<IAppSettings>();
            Mock.Get(appSettings).Setup(a => a.GetAppSetting(It.IsAny<string>())).Returns("00:00:00");
            _appSettingsExtras = new AppSettingsExtras(appSettings);
            _logger = Mock.Of<ILogger<AcceptOfferHandler>>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void AcceptOffer_ShouldThrow_WhenAccepted()
        {
            // Arrange
            var offer = _models.GetOffer().Generate("default, accepted");
            var request = new AcceptOffer
            {
                User = _user,
                OfferId = offer.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);
            var handler = new AcceptOfferHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be("Offer is already accepted");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void AcceptOffer_ShouldThrow_WhenNotPaid()
        {
            // Arrange
            var offer = _models.GetOffer().Generate("default");
            offer.ApplicationStatusId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationStatuses.Where(x => x.Code != Constants.ApplicationStatuses.Active)).Id;
            var request = new AcceptOffer
            {
                User = _user,
                OfferId = offer.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);
            var handler = new AcceptOfferHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be($"ApplicationStatusId is not paid: {offer.ApplicationStatusId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void AcceptOffer_ShouldThrow_WhenNotActive()
        {
            // Arrange
            var offer = _models.GetOffer().Generate("default");
            offer.OfferStateId = _dataFakerFixture.Faker.PickRandom(_lookups.OfferStates.Where(x => x.Code != Constants.Offers.State.Active)).Id;
            var request = new AcceptOffer
            {
                User = _user,
                OfferId = offer.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);
            var handler = new AcceptOfferHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be($"Offer must be in Active state: {offer.OfferStateId}");
        }
    }
}
