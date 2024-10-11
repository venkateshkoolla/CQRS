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
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Core.Settings;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Data;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetSpecialCodeHandlerTests
    {
        private readonly Faker _faker;
        private readonly ILookupsCache _lookupsCache;
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;
        private readonly IApiMapper _apiMapper;
        private readonly RequestCacheMock _requestCache;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly IAppSettings _appSettings = new AppSettingsMock();

        public GetSpecialCodeHandlerTests()
        {
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _requestCache = XunitInjectionCollection.RequestCacheMock;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSpecialCodeHandler_ShouldPass_When_OcasUser()
        {
            // Arrange
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles);
            var request = new GetSpecialCode
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                SpecialCode = "SC",
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var dtoSpecialCode = new Dto.ProgramSpecialCode
            {
                Code = request.SpecialCode,
                CollegeApplicationId = request.CollegeApplicationCycleId,
                Id = Guid.NewGuid(),
                LocalizedName = _faker.Random.Words(5)
            };

            var userAuthorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettings);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramSpecialCodes(It.Is<Guid>(p => p == request.CollegeApplicationCycleId))).ReturnsAsync(new List<Dto.ProgramSpecialCode> { dtoSpecialCode });

            var handler = new GetSpecialCodeHandler(Mock.Of<ILogger<GetSpecialCodeHandler>>(), _lookupsCache, userAuthorization, domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(request.SpecialCode);
            result.ApplicationCycleId.Should().Be(request.CollegeApplicationCycleId);
            result.Id.Should().Be(dtoSpecialCode.Id);
            result.Description.Should().Be(dtoSpecialCode.LocalizedName);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSpecialCodeHandler_ShouldPass_When_CollegeUser_CanAccessCollege()
        {
            // Arrange
            var testCollege = _modelFaker.AllAdminLookups.Colleges.First(c => c.Code == TestConstants.TestUser.College.PartnerId);
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles.Where(c => c.CollegeId == testCollege.Id));
            var request = new GetSpecialCode
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                SpecialCode = "SC",
                User = TestConstants.TestUser.College.TestPrincipal
            };

            var dtoSpecialCode = new Dto.ProgramSpecialCode
            {
                Code = request.SpecialCode,
                CollegeApplicationId = request.CollegeApplicationCycleId,
                Id = Guid.NewGuid(),
                LocalizedName = _faker.Random.Words(5)
            };

            var userAuthorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettings);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramSpecialCodes(It.Is<Guid>(p => p == request.CollegeApplicationCycleId))).ReturnsAsync(new List<Dto.ProgramSpecialCode> { dtoSpecialCode });

            var handler = new GetSpecialCodeHandler(Mock.Of<ILogger<GetSpecialCodeHandler>>(), _lookupsCache, userAuthorization, domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Code.Should().Be(request.SpecialCode);
            result.ApplicationCycleId.Should().Be(request.CollegeApplicationCycleId);
            result.Id.Should().Be(dtoSpecialCode.Id);
            result.Description.Should().Be(dtoSpecialCode.LocalizedName);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetSpecialCodeHandler_ShouldFail_When_CollegeApplicationCycleId_NotFound()
        {
            // Arrange
            var request = new GetSpecialCode
            {
                CollegeApplicationCycleId = Guid.NewGuid(),
                SpecialCode = "SC",
                User = _user
            };

            var handler = new GetSpecialCodeHandler(Mock.Of<ILogger<GetSpecialCodeHandler>>(), _lookupsCache, Mock.Of<IUserAuthorization>(), Mock.Of<IDomesticContext>(), _requestCache, _apiMapper);

            // Act
            Func<Task> action = () => handler.Handle(request, CancellationToken.None);

            // Assert
            action.Should().Throw<NotFoundException>()
                .WithMessage($"'College Application Cycle Id' not found: {request.CollegeApplicationCycleId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetSpecialCodeHandler_ShouldFail_When_SpecialCode_NotFound()
        {
            // Arrange
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles);
            var request = new GetSpecialCode
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                SpecialCode = "SC",
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramSpecialCodes(It.Is<Guid>(p => p == request.CollegeApplicationCycleId))).ReturnsAsync(new List<Dto.ProgramSpecialCode>());

            var handler = new GetSpecialCodeHandler(Mock.Of<ILogger<GetSpecialCodeHandler>>(), _lookupsCache, Mock.Of<IUserAuthorization>(), domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            Func<Task> action = () => handler.Handle(request, CancellationToken.None);

            // Assert
            action.Should().Throw<NotFoundException>()
                .WithMessage($"'Special Code' not found: {request.SpecialCode}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetSpecialCodeHandler_ShouldFail_When_User_Cannot_AccessCollege()
        {
            // Arrange
            var testCollege = _modelFaker.AllAdminLookups.Colleges.First(c => c.Code == TestConstants.TestUser.College.PartnerId);
            var openColleges = _modelFaker.AllAdminLookups.Colleges;
            var collegeAppCycles = _modelFaker.AllAdminLookups.CollegeApplicationCycles.Where(c => openColleges.Any(x => x.Id == c.CollegeId)).ToList();
            var collegeAppCycle = _faker.PickRandom(collegeAppCycles.Where(c => c.CollegeId != testCollege.Id).ToList());
            var request = new GetSpecialCode
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                SpecialCode = "SC",
                User = TestConstants.TestUser.College.TestPrincipal
            };

            var userAuthorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettings);

            var handler = new GetSpecialCodeHandler(Mock.Of<ILogger<GetSpecialCodeHandler>>(), _lookupsCache, userAuthorization, Mock.Of<IDomesticContext>(), _requestCache, _apiMapper);

            // Act
            Func<Task> action = () => handler.Handle(request, CancellationToken.None);

            // Assert
            action.Should().Throw<NotAuthorizedException>()
                .WithMessage("User does not have access to college");
        }
    }
}
