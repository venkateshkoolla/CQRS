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
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Core.Settings;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.AppSettings.Extras;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class DeclineOfferHandlerTests
    {
        private readonly AllLookups _lookups;
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _models;
        private readonly IPrincipal _user;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IAppSettingsExtras _appSettingsExtras;
        private readonly ILogger<DeclineOfferHandler> _logger;

        public DeclineOfferHandlerTests()
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
            _logger = Mock.Of<ILogger<DeclineOfferHandler>>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineOffer_ShouldPass()
        {
            // Arrange
            var offer = _models.GetOffer().Generate("default, accepted");
            offer.OfferLockReleaseDate = DateTime.UtcNow.AddDays(-7).ToDateInEstAsUtc();

            var request = new DeclineOffer
            {
                User = _user,
                OfferId = offer.Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);
            domesticContextMock.Setup(m => m.GetOffers(It.IsAny<Dto.GetOfferOptions>())).ReturnsAsync(new List<Dto.Offer> { offer } as IList<Dto.Offer>);
            domesticContextMock.Setup(m => m.DeclineOffer(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var handler = new DeclineOfferHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.DeclineOffer(It.Is<Guid>(x => x == offer.Id), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineOffer_ShouldPass_When_OcasUser_OfferAccepted()
        {
            // Arrange
            var offer = _models.GetOffer().Generate("default, accepted");
            offer.OfferLockReleaseDate = DateTime.UtcNow.AddDays(-7).ToDateInEstAsUtc();
            var request = new DeclineOffer
            {
                User = _user,
                OfferId = offer.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);
            domesticContextMock.Setup(m => m.GetOffers(It.IsAny<Dto.GetOfferOptions>())).ReturnsAsync(new List<Dto.Offer> { offer } as IList<Dto.Offer>);
            domesticContextMock.Setup(m => m.DeclineOffer(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new DeclineOfferHandler(_logger, domesticContextMock.Object, userAuthorization.Object, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineOffer_ShouldPass_When_OcasUser_OfferNoDecision()
        {
            // Arrange
            var offer = _models.GetOffer().Generate();
            offer.OfferLockReleaseDate = DateTime.UtcNow.AddDays(-7).ToDateInEstAsUtc();
            var request = new DeclineOffer
            {
                User = _user,
                OfferId = offer.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);
            domesticContextMock.Setup(m => m.GetOffers(It.IsAny<Dto.GetOfferOptions>())).ReturnsAsync(new List<Dto.Offer> { offer } as IList<Dto.Offer>);
            domesticContextMock.Setup(m => m.DeclineOffer(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new DeclineOfferHandler(_logger, domesticContextMock.Object, userAuthorization.Object, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineOffer_ShouldThrow_When_Expired()
        {
            // Arrange
            var offer = _models.GetOffer().Generate("default, accepted");
            offer.OfferLockReleaseDate = DateTime.UtcNow.ToDateInEstAsUtc();
            var request = new DeclineOffer
            {
                User = _user,
                OfferId = offer.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);
            domesticContextMock.Setup(m => m.GetOffers(It.IsAny<Dto.GetOfferOptions>())).ReturnsAsync(new List<Dto.Offer> { offer } as IList<Dto.Offer>);
            domesticContextMock.Setup(m => m.DeclineOffer(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var handler = new DeclineOfferHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Lock-out time has not ended");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineOffer_ShouldThrow_When_NotAccepted()
        {
            // Arrange
            var offer = _models.GetOffer().Generate("default, declined");
            var request = new DeclineOffer
            {
                User = _user,
                OfferId = offer.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);
            var handler = new DeclineOfferHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be($"Offer must be in Accepted status: {offer.OfferStatusId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineOffer_ShouldThrow_When_NotPaid()
        {
            // Arrange
            var offer = _models.GetOffer().Generate("default, accepted");
            offer.ApplicationStatusId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationStatuses.Where(x => x.Code != Constants.ApplicationStatuses.Active)).Id;
            var request = new DeclineOffer
            {
                User = _user,
                OfferId = offer.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);
            var handler = new DeclineOfferHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be($"ApplicationStatusId is not paid: {offer.ApplicationStatusId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineOffer_ShouldThrow_When_NotActive()
        {
            // Arrange
            var offer = _models.GetOffer().Generate("default, accepted");
            offer.OfferStateId = _dataFakerFixture.Faker.PickRandom(_lookups.OfferStates.Where(x => x.Code != Constants.Offers.State.Active)).Id;
            var request = new DeclineOffer
            {
                User = _user,
                OfferId = offer.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);
            var handler = new DeclineOfferHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be($"Offer must be in Active state: {offer.OfferStateId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineOffer_ShouldThrow_When_OcasUser_AlreadyDeclined()
        {
            // Arrange
            var offer = _models.GetOffer().Generate("default, declined");
            var request = new DeclineOffer
            {
                User = _user,
                OfferId = offer.Id
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffer(It.IsAny<Guid>())).ReturnsAsync(offer);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new DeclineOfferHandler(_logger, domesticContextMock.Object, userAuthorization.Object, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Offer must be not already be declined.");
        }
    }
}
