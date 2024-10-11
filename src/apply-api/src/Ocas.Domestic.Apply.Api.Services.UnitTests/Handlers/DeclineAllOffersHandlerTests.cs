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
    public class DeclineAllOffersHandlerTests
    {
        private readonly AllLookups _lookups;
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _models;
        private readonly IPrincipal _user;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IAppSettingsExtras _appSettingsExtras;
        private readonly ILogger<DeclineAllOffersHandler> _logger;

        public DeclineAllOffersHandlerTests()
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
            _logger = Mock.Of<ILogger<DeclineAllOffersHandler>>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineAllOffers_ShouldThrow_WhenLockedOut()
        {
            // Arrange
            var offers = _models.GetOffer().Generate(2);
            var estToday = DateTime.UtcNow.ToDateInEstAsUtc();
            var applicationId = offers[0].ApplicationId;
            offers.ForEach(x =>
            {
                x.OfferLockReleaseDate = estToday;
                x.ApplicationId = applicationId;
            });
            var request = new DeclineAllOffers
            {
                User = _user,
                ApplicationId = applicationId
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffers(It.IsAny<Dto.GetOfferOptions>())).ReturnsAsync(offers as IList<Dto.Offer>);
            var handler = new DeclineAllOffersHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be("Lock-out time has not ended");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineAllOffers_ShouldThrow_WhenExpired()
        {
            // Arrange
            var offers = _models.GetOffer().Generate(2);
            var estYesterday = DateTime.UtcNow.AddDays(-1).ToDateInEstAsUtc();
            var applicationId = offers[0].ApplicationId;
            offers.ForEach(x =>
            {
                x.HardExpiryDate = estYesterday;
                x.ApplicationId = applicationId;
            });
            var request = new DeclineAllOffers
            {
                User = _user,
                ApplicationId = applicationId
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffers(It.IsAny<Dto.GetOfferOptions>())).ReturnsAsync(offers as IList<Dto.Offer>);
            var handler = new DeclineAllOffersHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>().And.Message.Should().Be("No offers to decline");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineAllOffers_ShouldPass_OnlyWhenOffersAreNotDeclined()
        {
            // Arrange
            var offers = _models.GetOffer().Generate(4);
            var estYesterday = DateTime.UtcNow.AddDays(-7).ToDateInEstAsUtc();
            var applicationId = offers[0].ApplicationId;
            offers.ForEach(x =>
            {
                x.OfferLockReleaseDate = estYesterday;
                x.ApplicationId = applicationId;
            });
            offers[0].OfferStatusId = _lookups.OfferStatuses.Single(x => x.Code == Constants.Offers.Status.Declined).Id;
            offers[1].OfferStatusId = _lookups.OfferStatuses.Single(x => x.Code == Constants.Offers.Status.Accepted).Id;
            offers[2].OfferStatusId = _lookups.OfferStatuses.Single(x => x.Code == Constants.Offers.Status.NoDecision).Id;
            offers[3].OfferStatusId = _lookups.OfferStatuses.Single(x => x.Code == Constants.Offers.Status.NoDecision).Id;
            offers[3].HardExpiryDate = estYesterday;
            var declinedOffer = offers[0];
            var acceptedOffer = offers[1];
            var noDecisionOffer = offers[2];
            var expiredOffer = offers[3];
            var request = new DeclineAllOffers
            {
                User = _user,
                ApplicationId = applicationId
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffers(It.IsAny<Dto.GetOfferOptions>())).ReturnsAsync(offers as IList<Dto.Offer>);
            domesticContextMock.Setup(m => m.DeclineOffer(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            var handler = new DeclineAllOffersHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.DeclineOffer(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
            domesticContextMock.Verify(e => e.DeclineOffer(It.Is<Guid>(arg => arg == acceptedOffer.Id), It.IsAny<string>()), Times.Never);
            domesticContextMock.Verify(e => e.DeclineOffer(It.Is<Guid>(arg => arg == noDecisionOffer.Id), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineAllOffers_ShouldPass_When_IncludesAcceptedOffer()
        {
            // Arrange
            var offers = _models.GetOffer().Generate(1);
            var estYesterday = DateTime.UtcNow.AddDays(-7).ToDateInEstAsUtc();
            var applicationId = offers[0].ApplicationId;
            offers.ForEach(x =>
            {
                x.OfferLockReleaseDate = estYesterday;
                x.ApplicationId = applicationId;
            });
            offers[0].OfferStatusId = _lookups.OfferStatuses.Single(x => x.Code == Constants.Offers.Status.Accepted).Id;
            var acceptedOffer = offers[0];
            var request = new DeclineAllOffers
            {
                User = _user,
                ApplicationId = applicationId,
                IncludeAccepted = true
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffers(It.IsAny<Dto.GetOfferOptions>())).ReturnsAsync(offers as IList<Dto.Offer>);
            domesticContextMock.Setup(m => m.DeclineOffer(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var handler = new DeclineAllOffersHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.DeclineOffer(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
            domesticContextMock.Verify(e => e.DeclineOffer(It.Is<Guid>(arg => arg == acceptedOffer.Id), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void DeclineAllOffers_ShouldPass_When_ExcludesAcceptedOffer()
        {
            // Arrange
            var offers = _models.GetOffer().Generate(2);
            var estYesterday = DateTime.UtcNow.AddDays(-7).ToDateInEstAsUtc();
            var applicationId = offers[0].ApplicationId;
            offers.ForEach(x =>
            {
                x.OfferLockReleaseDate = estYesterday;
                x.ApplicationId = applicationId;
            });
            offers[0].OfferStatusId = _lookups.OfferStatuses.Single(x => x.Code == Constants.Offers.Status.NoDecision).Id;
            offers[1].OfferStatusId = _lookups.OfferStatuses.Single(x => x.Code == Constants.Offers.Status.Accepted).Id;
            var noDecisionOffer = offers[0];
            var acceptedOffer = offers[1];
            var request = new DeclineAllOffers
            {
                User = _user,
                ApplicationId = applicationId,
                IncludeAccepted = false
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOffers(It.IsAny<Dto.GetOfferOptions>())).ReturnsAsync(offers as IList<Dto.Offer>);
            domesticContextMock.Setup(m => m.DeclineOffer(It.IsAny<Guid>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var handler = new DeclineAllOffersHandler(_logger, domesticContextMock.Object, _userAuthorization, _lookupsCache, _apiMapper, _appSettingsExtras);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.DeclineOffer(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
            domesticContextMock.Verify(e => e.DeclineOffer(It.Is<Guid>(arg => arg == acceptedOffer.Id), It.IsAny<string>()), Times.Never);
            domesticContextMock.Verify(e => e.DeclineOffer(It.Is<Guid>(arg => arg == noDecisionOffer.Id), It.IsAny<string>()), Times.Once);
        }
    }
}
