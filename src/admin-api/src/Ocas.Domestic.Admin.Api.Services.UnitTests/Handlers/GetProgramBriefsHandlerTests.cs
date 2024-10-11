using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Apply.TestFramework.RuleCollections;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetProgramBriefsHandlerTests
    {
        private readonly Faker _faker;
        private readonly ILookupsCache _lookupsCache;
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;
        private readonly IApiMapper _apiMapper;
        private readonly RequestCacheMock _requestCache;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();

        public GetProgramBriefsHandlerTests()
        {
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _requestCache = new RequestCacheMock();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetProgramBriefsHandler_ShouldPass()
        {
            // Arrange
            var appCycle = _modelFaker.AllAdminLookups.ApplicationCycles.FirstOrDefault(x => x.Status == Constants.ApplicationCycleStatuses.Active);
            var campuses = _modelFaker.AllAdminLookups.Campuses;
            var college = _faker.PickRandom(_modelFaker.AllAdminLookups.Colleges.WithCampuses(campuses));
            var request = new GetProgramBriefs
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = college.Id,
                Params = new GetProgramBriefOptions(),
                User = _user
            };

            var dtoPrograms = new Faker<Dto.Program>()
                .RuleFor(o => o.CollegeApplicationCycleId, request.ApplicationCycleId)
                .RuleFor(o => o.CollegeId, request.CollegeId)
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(6).ToUpperInvariant())
                .RuleFor(o => o.CampusId, f => f.PickRandom(campuses.Where(c => c.CollegeId == request.CollegeId)).Id)
                .RuleFor(o => o.DeliveryId, f => f.PickRandom(_modelFaker.AllAdminLookups.ProgramDeliveries).Id)
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(8).ToUpperInvariant())
                .RuleFor(o => o.Title, f => f.Random.Words(5))
                .Generate(15);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetPrograms(It.IsAny<Dto.GetProgramsOptions>())).ReturnsAsync(dtoPrograms);

            var handler = new GetProgramBriefsHandler(Mock.Of<ILogger<GetProgramBriefsHandler>>(), domesticContextMock.Object, _lookupsCache, _requestCache, _apiMapper);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty()
                .And.HaveSameCount(dtoPrograms)
                .And.BeInAscendingOrder(i => i.Title);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetProgramBriefsHandler_ShouldPass_When_Sorting()
        {
            // Arrange
            var appCycle = _modelFaker.AllAdminLookups.ApplicationCycles.FirstOrDefault(x => x.Status == Constants.ApplicationCycleStatuses.Active);
            var campuses = _modelFaker.AllAdminLookups.Campuses;
            var college = _faker.PickRandom(_modelFaker.AllAdminLookups.Colleges.WithCampuses(campuses));
            var request = new GetProgramBriefs
            {
                ApplicationCycleId = appCycle.Id,
                CollegeId = college.Id,
                Params = new GetProgramBriefOptions
                {
                    SortDirection = Enums.SortDirection.Descending,
                    SortBy = Enums.ProgramSortField.Code
                },
                User = _user
            };

            var dtoPrograms = new Faker<Dto.Program>()
                .RuleFor(o => o.CollegeApplicationCycleId, request.ApplicationCycleId)
                .RuleFor(o => o.CollegeId, request.CollegeId)
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(6).ToUpperInvariant())
                .RuleFor(o => o.CampusId, f => f.PickRandom(_modelFaker.AllAdminLookups.Campuses.Where(c => c.CollegeId == request.CollegeId)).Id)
                .RuleFor(o => o.DeliveryId, f => f.PickRandom(_modelFaker.AllAdminLookups.ProgramDeliveries).Id)
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(8).ToUpperInvariant())
                .RuleFor(o => o.Title, f => f.Random.Words(5))
                .Generate(15);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetPrograms(It.IsAny<Dto.GetProgramsOptions>())).ReturnsAsync(dtoPrograms);

            var handler = new GetProgramBriefsHandler(Mock.Of<ILogger<GetProgramBriefsHandler>>(), domesticContextMock.Object, _lookupsCache, _requestCache, _apiMapper);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty()
                .And.HaveSameCount(dtoPrograms)
                .And.BeInDescendingOrder(i => i.Code);
        }
    }
}
