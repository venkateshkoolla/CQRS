using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetMcuCodesHandlerTests
    {
        private readonly Faker _faker;
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;
        private readonly RequestCacheMock _requestCache;

        public GetMcuCodesHandlerTests()
        {
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _requestCache = XunitInjectionCollection.RequestCacheMock;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetMcuCodesHandler_ShouldPass_When_Paging()
        {
            // Arrange
            var request = new GetMcuCodes
            {
                Params = new GetMcuCodeOptions
                {
                    Search = "The",
                    Page = 2,
                    PageSize = 10
                }
            };

            var mcuCodes = new Faker<McuCode>()
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(2))
                .RuleFor(o => o.Title, f => $"The {f.Random.Words(3)}")
                .Generate(25);

            var lookupsCacheMock = new AdminTestFramework.LookupsCacheMock();
            lookupsCacheMock.Setup(x => x.GetMcuCodes(It.IsAny<string>())).ReturnsAsync(mcuCodes);
            var handler = new GetMcuCodesHandler(Mock.Of<ILogger<GetMcuCodesHandler>>(), lookupsCacheMock.Object, _requestCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNull()
                .And.BeOfType<PagedResult<McuCode>>();
            results.TotalCount.Should().Be(mcuCodes.Count);
            results.Items.Should().NotBeNullOrEmpty()
                .And.HaveCount(10);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetMcuCodesHandler_ShouldPass_When_Sorting()
        {
            // Arrange
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles);
            var request = new GetMcuCodes
            {
                Params = new GetMcuCodeOptions
                {
                    Search = "The",
                    SortBy = McuCodeSortField.Title
                }
            };

            var mcuCodes = new Faker<McuCode>()
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(2))
                .RuleFor(o => o.Title, f => $"The {f.Random.Words(3)}")
                .Generate(5);

            var lookupsCacheMock = new AdminTestFramework.LookupsCacheMock();
            lookupsCacheMock.Setup(x => x.GetMcuCodes(It.IsAny<string>())).ReturnsAsync(mcuCodes);
            var handler = new GetMcuCodesHandler(Mock.Of<ILogger<GetMcuCodesHandler>>(), lookupsCacheMock.Object, _requestCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNull()
                .And.BeOfType<PagedResult<McuCode>>();
            results.TotalCount.Should().Be(mcuCodes.Count);
            results.Items.Should().NotBeNullOrEmpty()
                .And.HaveSameCount(mcuCodes)
            .And.BeInAscendingOrder(i => i.Title);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetMcuCodesHandler_ShouldPass_When_Order_Descending()
        {
            // Arrange
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles);
            var request = new GetMcuCodes
            {
                Params = new GetMcuCodeOptions
                {
                    Search = "The",
                    SortBy = McuCodeSortField.Code,
                    SortDirection = SortDirection.Descending
                }
            };

            var mcuCodes = new Faker<McuCode>()
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(2))
                .RuleFor(o => o.Title, f => $"The {f.Random.Words(3)}")
                .Generate(5);

            var lookupsCacheMock = new AdminTestFramework.LookupsCacheMock();
            lookupsCacheMock.Setup(x => x.GetMcuCodes(It.IsAny<string>())).ReturnsAsync(mcuCodes);

            var handler = new GetMcuCodesHandler(Mock.Of<ILogger<GetMcuCodesHandler>>(), lookupsCacheMock.Object, _requestCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNull()
                .And.BeOfType<PagedResult<McuCode>>();
            results.TotalCount.Should().Be(mcuCodes.Count);
            results.Items.Should().NotBeNullOrEmpty()
                .And.HaveSameCount(mcuCodes)
                .And.BeInDescendingOrder(i => i.Code);
        }
    }
}
