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
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetProgramHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly RequestCache _requestCache;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly ILogger<GetProgramHandler> _logger;
        private readonly AdminTestFramework.ModelFakerFixture _models;

        public GetProgramHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _logger = Mock.Of<ILogger<GetProgramHandler>>();
            _models = XunitInjectionCollection.ModelFakerFixture;
            _requestCache = XunitInjectionCollection.RequestCacheMock;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetProgramHandler_ShouldPass()
        {
            // Arrange
            var request = new GetProgram
            {
                ProgramId = Guid.NewGuid(),
                User = _user
            };

            var applicationStatusId = _models.AllAdminLookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var applicationCycleActiveStatusId = _models.AllAdminLookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _models.AllAdminLookups.IntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var collegeId = _models.AllAdminLookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_models.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _models.AllAdminLookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;

            var dtoSpecialCode = new Dto.ProgramSpecialCode
            {
                Code = "SC",
                CollegeApplicationId = collegeAppCycleId,
                Id = Guid.NewGuid(),
                LocalizedName = _dataFakerFixture.Faker.Random.Words(5)
            };

            var dtoProgram = new Dto.Program
            {
                Id = request.ProgramId,
                SpecialCodeId = dtoSpecialCode.Id,
                CollegeId = collegeId,
                CollegeApplicationCycleId = collegeAppCycleId
            };

            var dtoProgramIntake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                ProgramIntakeStatusId = intakeStatusActiveId,
                ApplicationCycleId = applicationCycleActiveStatusId,
                ProgramId = dtoProgram.Id,
                CollegeApplicationCycleId = collegeAppCycleId,
                HasSemesterOverride = true,
                DefaultEntrySemesterId = Guid.NewGuid()
            };

            var dtoProgramApplication = new Dto.ProgramApplication
            {
                IntakeId = dtoProgramIntake.Id,
                ApplicationId = Guid.NewGuid(),
                ApplicationNumber = _dataFakerFixture.Faker.Random.ReplaceNumbers("200######"),
                ApplicantFirstName = _dataFakerFixture.Faker.Person.FirstName
            };

            var domesticContextMock = new DomesticContextMock();

            domesticContextMock.Setup(x => x.GetProgram(It.IsAny<Guid>())).ReturnsAsync(dtoProgram);
            domesticContextMock.Setup(x => x.GetProgramSpecialCodes(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.ProgramSpecialCode> { dtoSpecialCode });
            domesticContextMock.Setup(x => x.GetProgramIntakes(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.ProgramIntake> { dtoProgramIntake });
            domesticContextMock.Setup(x => x.GetProgramApplications(It.IsAny<Dto.GetProgramApplicationsOptions>())).ReturnsAsync(new List<Dto.ProgramApplication> { dtoProgramApplication });

            var handler = new GetProgramHandler(_logger, _lookupsCache, _userAuthorization, domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<Program>();
            result.EntryLevelIds.Should().BeSameAs(dtoProgramIntake.EntryLevels);
            result.SpecialCode.Should().BeSameAs(dtoSpecialCode.Code);
            result.Intakes.Should().OnlyContain(x => x.Id == dtoProgramIntake.Id)
                .And.HaveCount(1)
                .And.OnlyContain(x => x.DefaultEntryLevelId == dtoProgramIntake.DefaultEntrySemesterId)
                .And.OnlyContain(x => x.CanDelete == new List<Dto.ProgramApplication> { dtoProgramApplication }
                .All(a => a.IntakeId != x.Id));
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetProgramHandler_ShouldThrow_When_NotFound()
        {
            // Arrange
            var request = new GetProgram
            {
                ProgramId = Guid.NewGuid(),
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var handler = new GetProgramHandler(_logger, _lookupsCache, _userAuthorization, domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            Func<Task> action = () => handler.Handle(request, CancellationToken.None);

            // Assert
            action.Should().Throw<NotFoundException>()
                .WithMessage($"Program {request.ProgramId} not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetProgramHandler_ShouldThrow_When_User_Cannot_Access_College()
        {
            // Arrange
            var testCollege = _models.AllAdminLookups.Colleges.First(c => c.Code != TestConstants.TestUser.College.PartnerId);

            // Arrange
            var request = new GetProgram
            {
                ProgramId = Guid.NewGuid(),
                User = TestConstants.TestUser.College.TestPrincipal
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = request.ProgramId, CollegeId = testCollege.Id });

            var appSettings = new AppSettingsMock();

            var userAuthorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContextMock.Object, XunitInjectionCollection.LookupsCache, appSettings);
            var handler = new GetProgramHandler(_logger, _lookupsCache, userAuthorization, domesticContextMock.Object, _requestCache, _apiMapper);

            // Act
            Func<Task> action = () => handler.Handle(request, CancellationToken.None);

            // Assert
            action.Should().Throw<NotAuthorizedException>()
                .WithMessage("User does not have access to college");
        }
    }
}
