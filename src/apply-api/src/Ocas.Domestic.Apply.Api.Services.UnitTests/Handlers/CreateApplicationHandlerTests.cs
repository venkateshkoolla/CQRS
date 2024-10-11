using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class CreateApplicationHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly ModelFakerFixture _models;

        public CreateApplicationHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task CreateApplicationHandler_ShouldPass_When_ApplicationExistsForCycle()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            application.ProgramsComplete = true;
            application.TranscriptsComplete = true;
            var applicationCycleId = _models.AllApplyLookups.ApplicationCycles.First(x => x.Id == application.ApplicationCycleId).Id;
            var request = new CreateApplication
            {
                User = new ClaimsPrincipal(),
                ApplicantId = application.ApplicantId,
                ApplicationCycleId = application.ApplicationCycleId
            };
            var applications = new List<Dto.Application> { new Dto.Application
            {
                ApplicantId = application.ApplicantId,
                ApplicationCycleId = application.ApplicationCycleId,
                ApplicationNumber = application.ApplicationNumber,
                ApplicationStatusId = application.ApplicationStatusId,
                CompletedSteps = 7,
                EffectiveDate = application.EffectiveDate.ToDateTime(),
                Id = application.Id,
                ModifiedBy = application.ModifiedBy,
                ModifiedOn = application.ModifiedOn
            } } as IList<Dto.Application>;
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplications(It.IsAny<Guid>())).ReturnsAsync(applications);

            var handler = new CreateApplicationHandler(Mock.Of<ILogger<CreateApplicationHandler>>(), domesticContextMock.Object, _apiMapper, _lookupsCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(application, opts => opts.Excluding(x => x.TranscriptsComplete).Excluding(x => x.ProgramsComplete));
        }
    }
}
