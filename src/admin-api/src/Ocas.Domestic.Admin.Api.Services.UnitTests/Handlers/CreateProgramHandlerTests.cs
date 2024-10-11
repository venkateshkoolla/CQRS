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
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Admin.Services.Handlers;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class CreateProgramHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly IDtoMapper _dtoMapper;
        private readonly RequestCache _requestCache;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly ILogger<CreateProgramHandler> _logger;
        private readonly TestFramework.ModelFakerFixture _models;

        public CreateProgramHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _logger = Mock.Of<ILogger<CreateProgramHandler>>();
            _models = XunitInjectionCollection.ModelFakerFixture;
            _requestCache = XunitInjectionCollection.RequestCacheMock;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task CreateProgramHandler_ShouldPass_With_Program()
        {
            // Arrange
            var programBase = _models.GetProgramBase().Generate();
            programBase.Intakes = _models.GetProgramIntake(programBase).Generate(3);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.CreateProgram(It.IsAny<Dto.ProgramBase>())).ReturnsAsync(new Dto.Program());
            domesticContextMock.Setup(m => m.CreateProgramIntake(It.IsAny<Dto.ProgramIntakeBase>())).ReturnsAsync(new Dto.ProgramIntake());
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program());
            domesticContextMock.Setup(m => m.GetProgramApplications(It.IsAny<Dto.GetProgramApplicationsOptions>())).ReturnsAsync(new List<Dto.ProgramApplication>());
            domesticContextMock.Setup(m => m.GetPrograms(It.IsAny<Dto.GetProgramsOptions>())).ReturnsAsync(new List<Dto.Program>());
            domesticContextMock.Setup(m => m.GetProgramEntryLevels(It.IsAny<Dto.GetProgramEntryLevelOptions>())).ReturnsAsync(new List<Dto.ProgramEntryLevel>());
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.ProgramIntake>());
            domesticContextMock.Setup(m => m.UpdateProgram(It.IsAny<Dto.Program>())).ReturnsAsync((Dto.Program program) => program);

            var handler = new CreateProgramHandler(_logger, domesticContextMock.Object, _lookupsCache, _dtoMapper, _userAuthorization, _apiMapper, _requestCache);
            var message = new CreateProgram
            {
                Program = programBase,
                User = _user
            };

            // Act
            var result = await handler.Handle(message, CancellationToken.None);

            // Assert
            domesticContextMock.Verify(m => m.CreateProgram(It.IsAny<Dto.ProgramBase>()), Times.Once);
            result.Should().NotBeNull()
                .And.BeOfType<Program>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramHandler_ShouldThrow_When_User_CannotAccess_College()
        {
            // Arrange
            var programBase = _models.GetProgramBase().Generate();
            programBase.Intakes = _models.GetProgramIntake(programBase).Generate(3);
            programBase.CollegeId = _models.AllAdminLookups.Colleges.First(c => c.Code != TestConstants.TestUser.College.PartnerId).Id;

            var domesticContextMock = new DomesticContextMock();
            var appSettings = new AppSettingsMock();
            var userAuthorization = new UserAuthorization(Mock.Of<ILogger<UserAuthorization>>(), domesticContextMock.Object, _lookupsCache, appSettings);

            var handler = new CreateProgramHandler(_logger, domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorization, _apiMapper, _requestCache);
            var message = new CreateProgram
            {
                Program = programBase,
                User = TestConstants.TestUser.College.TestPrincipal
            };

            // Act
            Func<Task> action = () => handler.Handle(message, CancellationToken.None);

            // Assert
            action.Should().Throw<NotAuthorizedException>()
                    .WithMessage("User does not have access to college");
        }
    }
}
