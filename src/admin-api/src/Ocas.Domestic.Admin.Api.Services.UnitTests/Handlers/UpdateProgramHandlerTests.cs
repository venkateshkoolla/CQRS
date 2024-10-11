using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply;
using Ocas.Domestic.Apply.Admin;
using Ocas.Domestic.Apply.Admin.Api.Services;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Api.Services.UnitTests;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Admin.Services.Handlers;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Admin.Api.Services.UnitTests.Handlers
{
    public class UpdateProgramHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly ILookupsCache _lookupsCache;
        private readonly IDtoMapper _dtoMapper;
        private readonly RequestCache _requestCache;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly ILogger<UpdateProgramHandler> _logger;

        public UpdateProgramHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _logger = Mock.Of<ILogger<UpdateProgramHandler>>();
            _requestCache = XunitInjectionCollection.RequestCacheMock;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task Handle_ShouldPass_With_Program()
        {
            // Arrange
            var program = _models.GetProgram().Generate();
            program.Intakes = _models.GetProgramIntake(program).Generate(3);

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.CreateProgramIntake(It.IsAny<Dto.ProgramIntakeBase>())).ReturnsAsync((Dto.ProgramIntakeBase intake) => new Dto.ProgramIntake());
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program());
            domesticContextMock.Setup(m => m.GetProgramApplications(It.IsAny<Dto.GetProgramApplicationsOptions>())).ReturnsAsync(new List<Dto.ProgramApplication>());
            domesticContextMock.Setup(m => m.GetPrograms(It.IsAny<Dto.GetProgramsOptions>())).ReturnsAsync(new List<Dto.Program>());
            domesticContextMock.Setup(m => m.GetProgramEntryLevels(It.IsAny<Dto.GetProgramEntryLevelOptions>())).ReturnsAsync(new List<Dto.ProgramEntryLevel>());
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.ProgramIntake>());
            domesticContextMock.Setup(m => m.UpdateProgram(It.IsAny<Dto.Program>())).ReturnsAsync((Dto.Program p) => p);

            var handler = new UpdateProgramHandler(_logger, domesticContextMock.Object, _lookupsCache, _dtoMapper, _userAuthorization, _apiMapper, _requestCache);
            var message = new UpdateProgram
            {
                ProgramId = program.Id,
                Program = program,
                User = _user
            };

            // Act
            var result = await handler.Handle(message, CancellationToken.None);

            // Assert
            domesticContextMock.Verify(m => m.UpdateProgram(It.IsAny<Dto.Program>()), Times.Once);
            result.Should().NotBeNull()
                .And.BeOfType<Program>();
        }
    }
}
