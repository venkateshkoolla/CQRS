using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Core.Settings;
using Ocas.Domestic.Apply.Admin.Messages;
using Xunit;
using Xunit.Categories;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetCollegeApplicationCyclesHandlerTests
    {
        private readonly Faker _faker;
        private readonly ILookupsCache _lookupsCache;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly IAppSettings _appSettings = Mock.Of<IAppSettings>();

        public GetCollegeApplicationCyclesHandlerTests()
        {
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeApplicationCyclesHandler_ShouldPass()
        {
            // Arrange
            var collegeId = _faker.PickRandom(await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada)).Id;
            var applCycleStatuses = await _lookupsCache.GetApplicationCycleStatuses(Constants.Localization.EnglishCanada);
            var draftAppCycleStatusId = applCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Draft).Id;
            var activeAppCycleStatusId = applCycleStatuses.First(a => a.Code == Constants.ApplicationCycleStatuses.Active).Id;

            var request = new GetCollegeApplicationCycles
            {
                CollegeId = collegeId,
                User = _user
            };

            var handler = new GetCollegeApplicationCyclesHandler(Mock.Of<ILogger<GetCollegeApplicationCyclesHandler>>(), _lookupsCache, _appSettings);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty()
                .And.OnlyHaveUniqueItems()
                .And.OnlyContain(c => c.CollegeId == collegeId)
                .And.OnlyContain(c => c.StatusId == draftAppCycleStatusId || c.StatusId == activeAppCycleStatusId);
        }
    }
}
