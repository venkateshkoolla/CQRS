using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class GetEducationsHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupCache;
        private readonly ILogger<GetEducationsHandler> _logger;
        private readonly IApiMapper _apiMapper;
        private readonly ModelFakerFixture _models;

        public GetEducationsHandlerTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupCache = XunitInjectionCollection.LookupsCache;
            _logger = Mock.Of<ILogger<GetEducationsHandler>>();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetEducationsHandler_ShouldPass_When_Empty()
        {
            // Arrange
            var applicant = _models.GetApplicant().Generate();
            var request = new GetEducations
            {
                ApplicantId = applicant.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education>());

            var handler = new GetEducationsHandler(_logger, domesticContextMock.Object, _apiMapper, _lookupCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().BeEmpty();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetEducationsHandler_ShouldPass()
        {
            // Arrange
            var applicant = _models.GetApplicant().Generate();
            var request = new GetEducations
            {
                ApplicantId = applicant.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { new Dto.Education { Id = Guid.NewGuid() } });

            var handler = new GetEducationsHandler(_logger, domesticContextMock.Object, _apiMapper, _lookupCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty();
        }
    }
}
