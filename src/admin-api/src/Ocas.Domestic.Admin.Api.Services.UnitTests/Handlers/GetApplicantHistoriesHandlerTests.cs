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
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetApplicantHistoriesHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILogger<GetApplicantHistoriesHandler> _logger;
        private readonly IApiMapper _apiMapper;
        private readonly IUserAuthorization _userAuthorization;
        private readonly TestFramework.ModelFakerFixture _models;
        private readonly RequestCache _requestCache;
        private readonly ILookupsCache _lookupsCache;

        public GetApplicantHistoriesHandlerTests()
        {
            _dataFakerFixture = new DataFakerFixture();
            _logger = Mock.Of<ILogger<GetApplicantHistoriesHandler>>();
            _apiMapper = new AutoMapperFixture().CreateApiMapper();
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _models = XunitInjectionCollection.ModelFakerFixture;
            _requestCache = new RequestCacheMock();
            _lookupsCache = XunitInjectionCollection.LookupsCache;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetApplicantHistoriesHandler_ShouldPass()
        {
            // Arrange
            var applicant = _models.GetApplicant().Generate();
            var application = _models.GetApplication().Generate();
            application.ApplicantId = applicant.Id;

            var request = new GetApplicantHistories
            {
                ApplicantId = applicant.Id,
                User = Mock.Of<IPrincipal>(),
                Options = new Models.GetApplicantHistoryOptions()
            };
            var customAuditId = Guid.NewGuid();

            var dtoCustomAuditsByApplication = new List<Dto.CustomAudit> {
                new Dto.CustomAudit
                {
                    Id = customAuditId,
                    ApplicantId = applicant.Id,
                    ApplicationId = application.Id,
                    Details = new List<Dto.CustomAuditDetail>
                    {
                        new Dto.CustomAuditDetail { CustomAuditId = customAuditId, Id = Guid.NewGuid() }
                    }
                },
                new Dto.CustomAudit { Id = Guid.NewGuid(), ApplicantId = null, ApplicationId = application.Id }
            };

            var dtoCustomAuditsByApplicant = new List<Dto.CustomAudit> {
                new Dto.CustomAudit { Id = Guid.NewGuid(), ApplicantId = application.ApplicantId, ApplicationId = null }
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplications(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Application> { new Dto.Application { Id = application.Id, ApplicantId = applicant.Id, ApplicationCycleId = application.ApplicationCycleId } });
            domesticContextMock.Setup(x => x.GetCustomAudits(It.Is<Dto.GetCustomAuditOptions>(o => o.ApplicantId == applicant.Id && o.ApplicationId == null), It.IsAny<Locale>())).ReturnsAsync(dtoCustomAuditsByApplicant);
            domesticContextMock.Setup(x => x.GetCustomAudits(It.Is<Dto.GetCustomAuditOptions>(o => o.ApplicantId == null && o.ApplicationId == application.Id), It.IsAny<Locale>())).ReturnsAsync(dtoCustomAuditsByApplication);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new GetApplicantHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorizationMock.Object, _requestCache, _lookupsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            results.Should().NotBeNull();
            results.Items.Should().HaveCount(dtoCustomAuditsByApplicant.Count + dtoCustomAuditsByApplication.Count);
            results.Items.Should().SatisfyRespectively(
                first => first.Details.Should().BeNullOrEmpty(),
                second => second.Details.Should().NotBeNullOrEmpty(),
                third => third.Details.Should().BeNullOrEmpty());
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetApplicantHistoriesHandler_When_ApplicationId_ShouldPass()
        {
            // Arrange
            var applicant = _models.GetApplicant().Generate();
            var application = _models.GetApplication().Generate();
            application.ApplicantId = applicant.Id;

            var request = new GetApplicantHistories
            {
                ApplicantId = applicant.Id,
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>(),
                Options = new Models.GetApplicantHistoryOptions()
            };
            var customAuditId = Guid.NewGuid();

            var dtoCustomAuditsByApplication = new List<Dto.CustomAudit> {
                new Dto.CustomAudit
                {
                    Id = customAuditId,
                    ApplicantId = applicant.Id,
                    ApplicationId = application.Id,
                    Details = new List<Dto.CustomAuditDetail>
                    {
                        new Dto.CustomAuditDetail { CustomAuditId = customAuditId, Id = Guid.NewGuid() }
                    }
                },
                new Dto.CustomAudit { Id = Guid.NewGuid(), ApplicantId = null, ApplicationId = application.Id }
            };

            var dtoCustomAuditsByApplicant = new List<Dto.CustomAudit> {
                new Dto.CustomAudit { Id = Guid.NewGuid(), ApplicantId = application.ApplicantId, ApplicationId = null }
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplications(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Application> { new Dto.Application { Id = application.Id, ApplicantId = applicant.Id, ApplicationCycleId = application.ApplicationCycleId } });
            domesticContextMock.Setup(x => x.GetCustomAudits(It.Is<Dto.GetCustomAuditOptions>(o => o.ApplicantId == applicant.Id && o.ApplicationId == null), It.IsAny<Locale>())).ReturnsAsync(dtoCustomAuditsByApplicant);
            domesticContextMock.Setup(x => x.GetCustomAudits(It.Is<Dto.GetCustomAuditOptions>(o => o.ApplicantId == null && o.ApplicationId == application.Id), It.IsAny<Locale>())).ReturnsAsync(dtoCustomAuditsByApplication);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new GetApplicantHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorizationMock.Object, _requestCache, _lookupsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            results.Should().NotBeNull();
            results.Items.Should().HaveCount(dtoCustomAuditsByApplicant.Count + dtoCustomAuditsByApplication.Count);
            results.Items.Should().SatisfyRespectively(
                first => first.Details.Should().BeNullOrEmpty(),
                second => second.Details.Should().NotBeNullOrEmpty(),
                third => third.Details.Should().BeNullOrEmpty());
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetApplicantHistoriesHandler_ShouldPass_When_Order_NotNull_And_CustomEntity_Equals_Order()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var applicant = _models.GetApplicant().Generate();
            application.ApplicantId = applicant.Id;

            var request = new GetApplicantHistories
            {
                ApplicantId = applicant.Id,
                User = Mock.Of<IPrincipal>()
            };

            var dtoCustomAuditsByApplication = new List<Dto.CustomAudit> {
                new Dto.CustomAudit
                {
                    Id = Guid.NewGuid(),
                    ApplicantId = application.ApplicantId,
                    ApplicationId = application.Id,
                    CustomEntityLabelEn = "order",
                    OrderId = null,
                    Details = new List<Dto.CustomAuditDetail>()
                },
                new Dto.CustomAudit
                {
                    Id = Guid.NewGuid(),
                    ApplicantId = null,
                    ApplicationId = application.Id,
                    CustomEntityLabelEn = "order",
                    OrderId = null,
                    Details = new List<Dto.CustomAuditDetail>()
                }
            };

            var expectedCustomAuditId = Guid.NewGuid();
            var dtoCustomAuditsByApplicant = new List<Dto.CustomAudit> {
                new Dto.CustomAudit
                {
                    Id = Guid.NewGuid(),
                    ApplicantId = application.ApplicantId, ApplicationId = null,
                    CustomEntityLabelEn = "NotOrder",
                    OrderId = Guid.NewGuid(),
                    Details = new List<Dto.CustomAuditDetail>()
                },
                new Dto.CustomAudit
                {
                    Id = expectedCustomAuditId,
                    ApplicantId = application.ApplicantId,
                    ApplicationId = null,
                    CustomEntityLabelEn = "NotOrder",
                    OrderId = null,
                    Details = new List<Dto.CustomAuditDetail>()
                }
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplications(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Application> { new Dto.Application { Id = application.Id } });
            domesticContextMock.SetupSequence(x => x.GetCustomAudits(It.IsAny<Dto.GetCustomAuditOptions>(), It.IsAny<Locale>()))
                .ReturnsAsync(dtoCustomAuditsByApplicant)
                .ReturnsAsync(dtoCustomAuditsByApplication);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new GetApplicantHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorizationMock.Object, _requestCache, _lookupsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            results.Items.Should().NotBeNullOrEmpty();
            results.TotalCount.Should().BePositive();
            results.Items.Should().BeEquivalentTo(
                dtoCustomAuditsByApplicant.Where(x => x.Id == expectedCustomAuditId),
                opt => opt.ExcludingMissingMembers()
                          .Excluding(p => p.ModifiedOn));
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetApplicantHistoriesHandler_ShouldThrow_When_Not_OcasUser()
        {
            // Arrange
            var request = new GetApplicantHistories
            {
                ApplicantId = Guid.NewGuid(),
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new GetApplicantHistoriesHandler(_logger, domesticContextMock.Object, _apiMapper, userAuthorizationMock.Object, _requestCache, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }
    }
}
