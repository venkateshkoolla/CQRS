using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Apply.TestFramework.RuleCollections;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class ProgramCodeAvailableHandlerTests
    {
        private readonly Faker _faker;
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;
        private readonly ILookupsCache _lookupsCache;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly ILogger<ProgramCodeAvailableHandler> _logger = Mock.Of<ILogger<ProgramCodeAvailableHandler>>();
        private readonly IUserAuthorization _userAuthorization = Mock.Of<IUserAuthorization>();

        public ProgramCodeAvailableHandlerTests()
        {
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task ProgramCodeAvailableHandler_ShouldPass()
        {
            // Arrange
            var college = _faker.PickRandom(_modelFaker.AllAdminLookups.Colleges.WithCampuses(_modelFaker.AllAdminLookups.Campuses));
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles.Where(a => a.CollegeId == college.Id));
            var campus = _faker.PickRandom(_modelFaker.AllAdminLookups.Campuses.Where(c => c.CollegeId == college.Id));
            var delivery = _faker.PickRandom(_modelFaker.AllAdminLookups.StudyMethods);
            var code = _faker.Random.AlphaNumeric(8).ToUpperInvariant();

            var request = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = campus.Id,
                Code = code,
                DeliveryId = delivery.Id,
                User = _user
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetPrograms(It.IsAny<Dto.GetProgramsOptions>())).ReturnsAsync(new List<Dto.Program>());

            var handler = new ProgramCodeAvailableHandler(_logger, _lookupsCache, _userAuthorization, domesticContext.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task ProgramCodeAvailableHandler_ShouldPass_When_Unavailable()
        {
            // Arrange
            var college = _faker.PickRandom(_modelFaker.AllAdminLookups.Colleges.WithCampuses(_modelFaker.AllAdminLookups.Campuses));
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles.Where(a => a.CollegeId == college.Id));
            var campus = _faker.PickRandom(_modelFaker.AllAdminLookups.Campuses.Where(c => c.CollegeId == college.Id));
            var delivery = _faker.PickRandom(_modelFaker.AllAdminLookups.StudyMethods);
            var code = _faker.Random.AlphaNumeric(8).ToUpperInvariant();

            var request = new ProgramCodeAvailable
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                CampusId = campus.Id,
                Code = code,
                DeliveryId = delivery.Id,
                User = _user
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetPrograms(It.IsAny<Dto.GetProgramsOptions>())).ReturnsAsync(new List<Dto.Program>
            {
                new Dto.Program
                {
                    Id = Guid.NewGuid(),
                    CollegeApplicationCycleId = collegeAppCycle.Id,
                    CampusId = campus.Id,
                    Code = code,
                    DeliveryId = delivery.Id
                }
            });

            var handler = new ProgramCodeAvailableHandler(_logger, _lookupsCache, _userAuthorization, domesticContext.Object);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeFalse();
        }
    }
}
