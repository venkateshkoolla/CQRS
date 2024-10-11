using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Coltrane.Bds.Provider;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetCollegeTransmissionHistoriesHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILogger<GetCollegeTransmissionHistoriesHandler> _logger;
        private readonly TestFramework.ModelFakerFixture _models;
        private readonly IApiMapper _apiMapper;
        private readonly RequestCache _requestCache;
        private readonly IPrincipal _user;
        private readonly Faker _faker;
        private readonly ILookupsCache _lookupsCache;

        public GetCollegeTransmissionHistoriesHandlerTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _requestCache = XunitInjectionCollection.RequestCacheMock;
            _faker = _dataFakerFixture.Faker;
            _user = Mock.Of<IPrincipal>();
            _logger = Mock.Of<ILogger<GetCollegeTransmissionHistoriesHandler>>();
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionHistoriesHandler_ShouldPass()
        {
            // Arrange
            var application = _models.GetApplication().Generate();

            var request = new GetCollegeTransmissionHistories
            {
                ApplicationId = application.Id,
                User = _user,
                Options = new GetCollegeTransmissionHistoryOptions()
            };

            var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
            var collegeTransmission = new Dto.CollegeTransmission
            {
                TransactionType = Constants.CollegeTransmissionTransactionTypes.Insert,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                CollegeCode = _faker.PickRandom(colleges).Code,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = _faker.Random.String2(50),
                BusinessKey = Guid.NewGuid()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = request.ApplicationId });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { collegeTransmission });

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var translationCacheMock = new Mock<ITranslationsCache>();
            translationCacheMock.Setup(x => x.GetTranslations(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new TranslationsDictionary(new Dictionary<string, string>()));

            var handler = new GetCollegeTransmissionHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorization.Object, _requestCache, coltraneBdsProviderMock.Object, _lookupsCache, translationCacheMock.Object);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Items.Should().NotBeNullOrEmpty();
            results.Items.Should().OnlyContain(x => colleges.Any(y => y.Id == x.CollegeId));
            results.Items.Should().OnlyContain(x => x.TransactionCode == Constants.CollegeTransmissionCodes.ProgramChoice);
            results.Items.Should().OnlyContain(x => x.Data == collegeTransmission.Data);
            results.Items.Should().OnlyContain(x => x.ContextId == collegeTransmission.BusinessKey);
            results.Items.Should().OnlyContain(x => x.Sent == collegeTransmission.LastLoadDateTime);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionHistoriesHandler_ShouldPass_With_Filters()
        {
            // Arrange
            var application = _models.GetApplication().Generate();

            var request = new GetCollegeTransmissionHistories
            {
                ApplicationId = application.Id,
                User = _user,
                Options = new GetCollegeTransmissionHistoryOptions
                {
                    FromDate = _faker.Date.Between(DateTime.Now.AddMonths(-2).AsUtc(), DateTime.Now.AddMonths(-3).AsUtc()).AsUtc().ToStringOrDefault(),
                    ToDate = _faker.Date.Between(DateTime.Now.AddMonths(-1).AsUtc(), DateTime.Now.AddMonths(-2).AsUtc()).AsUtc().ToStringOrDefault(),
                    Activity = Enums.CollegeTransmissionActivity.GCI
                }
            };

            var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
            var collegeTransmission = new Dto.CollegeTransmission
            {
                TransactionType = Constants.CollegeTransmissionTransactionTypes.Insert,
                TransactionCode = Constants.CollegeTransmissionCodes.Grade,
                CollegeCode = _faker.PickRandom(colleges).Code,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = _faker.Random.String2(50),
                BusinessKey = Guid.NewGuid()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = request.ApplicationId });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { collegeTransmission });

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var translationCacheMock = new Mock<ITranslationsCache>();
            translationCacheMock.Setup(x => x.GetTranslations(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new TranslationsDictionary(new Dictionary<string, string>()));

            var handler = new GetCollegeTransmissionHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorization.Object, _requestCache, coltraneBdsProviderMock.Object, _lookupsCache, translationCacheMock.Object);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Items.Should().NotBeNullOrEmpty();
            results.Items.Should().OnlyContain(x => x.TransactionCode == Constants.CollegeTransmissionCodes.Grade);
            results.Items.Should().OnlyContain(x => x.TransactionType == Constants.CollegeTransmissionTransactionTypes.Insert);
            results.Items.Should().OnlyContain(x => x.Sent == collegeTransmission.LastLoadDateTime);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetCollegeTransmissionHistoriesHandler_ShouldThrow_When_NotOcasUser()
        {
            // Arrange
            var request = new GetCollegeTransmissionHistories
            {
                ApplicationId = Guid.NewGuid(),
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new GetCollegeTransmissionHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorization.Object, _requestCache, coltraneBdsProviderMock.Object, _lookupsCache, Mock.Of<ITranslationsCache>());

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }
    }
}
