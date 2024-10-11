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
using Ocas.Domestic.Apply.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Data;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetSpecialCodesHandlerTests
    {
        private readonly Faker _faker;
        private readonly ILookupsCache _lookupsCache;
        private readonly AdminTestFramework.ModelFakerFixture _modelFaker;
        private readonly IApiMapper _apiMapper;
        private readonly RequestCacheMock _requestCache;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly IAppSettings _appSettings = new AppSettingsMock();

        public GetSpecialCodesHandlerTests()
        {
            _faker = XunitInjectionCollection.DataFakerFixture.Faker;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _requestCache = XunitInjectionCollection.RequestCacheMock;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSpecialCodesHandler_ShouldPass_When_OcasUser()
        {
            // Arrange
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles);
            var request = new GetSpecialCodes
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                Params = new GetSpecialCodeOptions { Search = "SC" },
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var dtoSpecialCodes = new List<Dto.ProgramSpecialCode>
            {
                new Dto.ProgramSpecialCode
                {
                Code = "SC",
                CollegeApplicationId = request.CollegeApplicationCycleId,
                Id = Guid.NewGuid(),
                LocalizedName = _faker.Random.Words(5)
                },
                new Dto.ProgramSpecialCode
                {
                Code = "AC",
                CollegeApplicationId = request.CollegeApplicationCycleId,
                Id = Guid.NewGuid(),
                LocalizedName = $"SC {_faker.Random.Words(5)}"
                }
            };

            var userAuthorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettings);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramSpecialCodes(It.Is<Guid>(p => p == request.CollegeApplicationCycleId))).ReturnsAsync(dtoSpecialCodes);

            var handler = new GetSpecialCodesHandler(Mock.Of<ILogger<GetSpecialCodesHandler>>(), _lookupsCache, userAuthorization, domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNull();
            results.TotalCount.Should().Be(dtoSpecialCodes.Count);
            results.Items.Should().NotBeNullOrEmpty()
                .And.HaveSameCount(dtoSpecialCodes)
                .And.BeInAscendingOrder(i => i.Code);

            results.Items.Should().SatisfyRespectively(
                first =>
                {
                    first.Code.Should().Be(dtoSpecialCodes[1].Code);
                    first.ApplicationCycleId.Should().Be(dtoSpecialCodes[1].CollegeApplicationId);
                    first.Id.Should().Be(dtoSpecialCodes[1].Id);
                    first.Description.Should().Be(dtoSpecialCodes[1].LocalizedName);
                },
                second =>
                {
                    second.Code.Should().Be(dtoSpecialCodes[0].Code);
                    second.ApplicationCycleId.Should().Be(dtoSpecialCodes[0].CollegeApplicationId);
                    second.Id.Should().Be(dtoSpecialCodes[0].Id);
                    second.Description.Should().Be(dtoSpecialCodes[0].LocalizedName);
                });
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSpecialCodesHandler_ShouldPass_When_CollegeUser_CanAccessCollege()
        {
            // Arrange
            var testCollege = _modelFaker.AllAdminLookups.Colleges.First(c => c.Code == TestConstants.TestUser.College.PartnerId);
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles.Where(c => c.CollegeId == testCollege.Id));
            var request = new GetSpecialCodes
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                Params = new GetSpecialCodeOptions { Search = "SC" },
                User = TestConstants.TestUser.College.TestPrincipal
            };

            var dtoSpecialCodes = new List<Dto.ProgramSpecialCode>
            {
                new Dto.ProgramSpecialCode
                {
                Code = "SC",
                CollegeApplicationId = request.CollegeApplicationCycleId,
                Id = Guid.NewGuid(),
                LocalizedName = _faker.Random.Words(5)
                },
                new Dto.ProgramSpecialCode
                {
                Code = "AC",
                CollegeApplicationId = request.CollegeApplicationCycleId,
                Id = Guid.NewGuid(),
                LocalizedName = $"SC {_faker.Random.Words(5)}"
                }
            };

            var userAuthorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettings);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramSpecialCodes(It.Is<Guid>(p => p == request.CollegeApplicationCycleId))).ReturnsAsync(dtoSpecialCodes);

            var handler = new GetSpecialCodesHandler(Mock.Of<ILogger<GetSpecialCodesHandler>>(), _lookupsCache, userAuthorization, domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNull();
            results.TotalCount.Should().Be(dtoSpecialCodes.Count);
            results.Items.Should().NotBeNullOrEmpty()
                .And.HaveSameCount(dtoSpecialCodes)
                .And.BeInAscendingOrder(i => i.Code);

            results.Items.Should().SatisfyRespectively(
                first =>
                {
                    first.Code.Should().Be(dtoSpecialCodes[1].Code);
                    first.ApplicationCycleId.Should().Be(dtoSpecialCodes[1].CollegeApplicationId);
                    first.Id.Should().Be(dtoSpecialCodes[1].Id);
                    first.Description.Should().Be(dtoSpecialCodes[1].LocalizedName);
                },
                second =>
                {
                    second.Code.Should().Be(dtoSpecialCodes[0].Code);
                    second.ApplicationCycleId.Should().Be(dtoSpecialCodes[0].CollegeApplicationId);
                    second.Id.Should().Be(dtoSpecialCodes[0].Id);
                    second.Description.Should().Be(dtoSpecialCodes[0].LocalizedName);
                });
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSpecialCodesHandler_ShouldPass_When_Paging()
        {
            // Arrange
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles);
            var request = new GetSpecialCodes
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                Params = new GetSpecialCodeOptions
                {
                    Search = "The",
                    Page = 2,
                    PageSize = 10
                },
                User = _user
            };

            var dtoSpecialCodes = new Faker<Dto.ProgramSpecialCode>()
                .RuleFor(o => o.CollegeApplicationId, collegeAppCycle.Id)
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(2))
                .RuleFor(o => o.LocalizedName, f => $"The {f.Random.Words(3)}")
                .Generate(25);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramSpecialCodes(It.Is<Guid>(p => p == request.CollegeApplicationCycleId))).ReturnsAsync(dtoSpecialCodes);

            var handler = new GetSpecialCodesHandler(Mock.Of<ILogger<GetSpecialCodesHandler>>(), _lookupsCache, Mock.Of<IUserAuthorization>(), domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNull()
                .And.BeOfType<PagedResult<SpecialCode>>();
            results.TotalCount.Should().Be(dtoSpecialCodes.Count);
            results.Items.Should().NotBeNullOrEmpty()
                .And.HaveCount(10)
                .And.BeInAscendingOrder(i => i.Code);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSpecialCodesHandler_ShouldPass_When_OrderByDescription()
        {
            // Arrange
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles);
            var request = new GetSpecialCodes
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                Params = new GetSpecialCodeOptions
                {
                    Search = "The",
                    SortBy = SpecialCodeSortField.Description
                },
                User = _user
            };

            var dtoSpecialCodes = new Faker<Dto.ProgramSpecialCode>()
                .RuleFor(o => o.CollegeApplicationId, collegeAppCycle.Id)
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(2))
                .RuleFor(o => o.LocalizedName, f => $"The {f.Random.Words(3)}")
                .Generate(5);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramSpecialCodes(It.Is<Guid>(p => p == request.CollegeApplicationCycleId))).ReturnsAsync(dtoSpecialCodes);

            var handler = new GetSpecialCodesHandler(Mock.Of<ILogger<GetSpecialCodesHandler>>(), _lookupsCache, Mock.Of<IUserAuthorization>(), domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNull()
                .And.BeOfType<PagedResult<SpecialCode>>();
            results.TotalCount.Should().Be(dtoSpecialCodes.Count);
            results.Items.Should().NotBeNullOrEmpty()
                .And.HaveSameCount(dtoSpecialCodes)
                .And.BeInAscendingOrder(i => i.Description);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetSpecialCodesHandler_ShouldPass_When_OrderDescending()
        {
            // Arrange
            var collegeAppCycle = _faker.PickRandom(_modelFaker.AllAdminLookups.CollegeApplicationCycles);
            var request = new GetSpecialCodes
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                Params = new GetSpecialCodeOptions
                {
                    Search = "The",
                    SortDirection = SortDirection.Descending
                },
                User = _user
            };

            var dtoSpecialCodes = new Faker<Dto.ProgramSpecialCode>()
                .RuleFor(o => o.CollegeApplicationId, collegeAppCycle.Id)
                .RuleFor(o => o.Code, f => f.Random.AlphaNumeric(2))
                .RuleFor(o => o.LocalizedName, f => $"The {f.Random.Words(3)}")
                .Generate(5);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetProgramSpecialCodes(It.Is<Guid>(p => p == request.CollegeApplicationCycleId))).ReturnsAsync(dtoSpecialCodes);

            var handler = new GetSpecialCodesHandler(Mock.Of<ILogger<GetSpecialCodesHandler>>(), _lookupsCache, Mock.Of<IUserAuthorization>(), domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNull()
                .And.BeOfType<PagedResult<SpecialCode>>();
            results.TotalCount.Should().Be(dtoSpecialCodes.Count);
            results.Items.Should().NotBeNullOrEmpty()
                .And.HaveSameCount(dtoSpecialCodes)
                .And.BeInDescendingOrder(i => i.Code);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetSpecialCodesHandler_ShouldFail_When_CollegeApplicationCycleId_NotFound()
        {
            // Arrange
            var request = new GetSpecialCodes
            {
                CollegeApplicationCycleId = Guid.NewGuid(),
                Params = new GetSpecialCodeOptions(),
                User = _user
            };

            var handler = new GetSpecialCodesHandler(Mock.Of<ILogger<GetSpecialCodesHandler>>(), _lookupsCache, Mock.Of<IUserAuthorization>(), Mock.Of<IDomesticContext>(), _requestCache, _apiMapper);

            // Act
            Func<Task> action = () => handler.Handle(request, CancellationToken.None);

            // Assert
            action.Should().Throw<NotFoundException>()
                .WithMessage($"'College Application Cycle Id' not found: {request.CollegeApplicationCycleId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetSpecialCodesHandler_ShouldFail_When_User_Cannot_AccessCollege()
        {
            // Arrange
            var testCollege = _modelFaker.AllAdminLookups.Colleges.First(c => c.Code == TestConstants.TestUser.College.PartnerId);
            var openColleges = _modelFaker.AllAdminLookups.Colleges;
            var collegeAppCycles = _modelFaker.AllAdminLookups.CollegeApplicationCycles.Where(c => openColleges.Any(x => x.Id == c.CollegeId)).ToList();
            var collegeAppCycle = _faker.PickRandom(collegeAppCycles.Where(c => c.CollegeId != testCollege.Id).ToList());
            var request = new GetSpecialCodes
            {
                CollegeApplicationCycleId = collegeAppCycle.Id,
                Params = new GetSpecialCodeOptions(),
                User = TestConstants.TestUser.College.TestPrincipal
            };

            var userAuthorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), Mock.Of<IDomesticContext>(), _lookupsCache, _appSettings);

            var handler = new GetSpecialCodesHandler(Mock.Of<ILogger<GetSpecialCodesHandler>>(), _lookupsCache, userAuthorization, Mock.Of<IDomesticContext>(), _requestCache, _apiMapper);

            // Act
            Func<Task> action = () => handler.Handle(request, CancellationToken.None);

            // Assert
            action.Should().Throw<NotAuthorizedException>()
                .WithMessage("User does not have access to college");
        }
    }
}
