using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Models;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models.ProgramIntake;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetIntakesHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly Faker _faker;
        private readonly ILogger<GetIntakesHandler> _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly RequestCache _requestCache;

        public GetIntakesHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _logger = Mock.Of<ILogger<GetIntakesHandler>>();
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _requestCache = new RequestCacheMock();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetIntakesHandler_ShouldPass()
        {
            var request = new GetIntakes
            {
                ApplicationCycleId = Guid.NewGuid(),
                CollegeId = Guid.NewGuid(),
                Options = new GetIntakesOptions { SortBy = IntakeSortField.Title, SortDirection = SortDirection.Ascending },
                User = Mock.Of<IPrincipal>()
            };

            var intakes = new List<Dto>
                            {
                                new Dto {
                                    CollegeApplicationCycleId = request.ApplicationCycleId,
                                    ProgramTitle = _faker.Random.String2(10)
                                },
                                new Dto {
                                    CollegeApplicationCycleId = request.ApplicationCycleId,
                                    ProgramTitle = _faker.Random.String2(10)
                                }
                            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetProgramIntakes(It.IsAny<GetProgramIntakeOptions>())).ReturnsAsync(intakes);

            var handler = new GetIntakesHandler(_logger, Mock.Of<IUserAuthorization>(), _lookupsCache, domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty()
                .And.BeOfType<List<IntakeBrief>>()
                .And.HaveSameCount(intakes)
                .And.BeInAscendingOrder(x => x.ProgramTitle);
        }
    }
}
