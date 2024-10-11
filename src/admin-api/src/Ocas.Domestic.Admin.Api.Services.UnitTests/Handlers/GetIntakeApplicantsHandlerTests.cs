using System;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Models;
using Xunit;
using Xunit.Categories;
using AdminModels = Ocas.Domestic.Apply.Admin.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetIntakeApplicantsHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly IPrincipal _user = Mock.Of<IPrincipal>();
        private readonly ILogger<GetIntakeApplicantsHandler> _logger = Mock.Of<ILogger<GetIntakeApplicantsHandler>>();
        private readonly IUserAuthorization _userAuthorization = Mock.Of<IUserAuthorization>();
        private readonly ILookupsCache _lookupCache;

        public GetIntakeApplicantsHandlerTests()
        {
            _lookupCache = XunitInjectionCollection.LookupsCache;
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetIntakeApplicantsHandler_ShouldPass()
        {
            // Arrange
            var request = new GetIntakeApplicants
            {
                IntakeId = Guid.NewGuid(),
                Params = new AdminModels.GetIntakeApplicantOptions(),
                User = _user
            };

            var programApplications = new Faker<ProgramApplication>()
                .RuleFor(o => o.IntakeId, request.IntakeId)
                .RuleFor(o => o.ApplicantFirstName, f => f.Person.FirstName)
                .RuleFor(o => o.ApplicantLastName, f => f.Person.LastName)
                .RuleFor(o => o.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.ApplicationNumber, f => f.Random.ReplaceNumbers("200######"))
                .Generate(10);

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetProgramIntake(It.IsAny<Guid>())).ReturnsAsync(new Dto.ProgramIntake { Id = request.IntakeId });
            domesticContext.Setup(m => m.GetProgramApplications(It.IsAny<GetProgramApplicationsOptions>())).ReturnsAsync(programApplications);

            var handler = new GetIntakeApplicantsHandler(_logger, domesticContext.Object, _userAuthorization, _lookupCache, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<AdminModels.PagedResult<AdminModels.IntakeApplicant>>();
            result.TotalCount.Should().Be(programApplications.Count);
            result.Items.Should().NotBeEmpty()
                .And.HaveSameCount(programApplications)
                .And.BeInAscendingOrder(i => i.Number);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetIntakeApplicantsHandler_ShouldPass_When_Paging()
        {
            // Arrange
            var request = new GetIntakeApplicants
            {
                IntakeId = Guid.NewGuid(),
                Params = new AdminModels.GetIntakeApplicantOptions
                {
                    Page = 2,
                    PageSize = 10
                },
                User = _user
            };

            var programApplications = new Faker<ProgramApplication>()
                .RuleFor(o => o.IntakeId, request.IntakeId)
                .RuleFor(o => o.ApplicantFirstName, f => f.Person.FirstName)
                .RuleFor(o => o.ApplicantLastName, f => f.Person.LastName)
                .RuleFor(o => o.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.ApplicationNumber, f => f.Random.ReplaceNumbers("200######"))
                .Generate(25);

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetProgramIntake(It.IsAny<Guid>())).ReturnsAsync(new Dto.ProgramIntake { Id = request.IntakeId });
            domesticContext.Setup(m => m.GetProgramApplications(It.IsAny<GetProgramApplicationsOptions>())).ReturnsAsync(programApplications);

            var handler = new GetIntakeApplicantsHandler(_logger, domesticContext.Object, _userAuthorization, _lookupCache, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<AdminModels.PagedResult<AdminModels.IntakeApplicant>>();
            result.TotalCount.Should().Be(programApplications.Count);
            result.Items.Should().NotBeEmpty()
                .And.HaveCount(10)
                .And.BeInAscendingOrder(i => i.Number);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetIntakeApplicantsHandler_ShouldPass_When_Sorting()
        {
            // Arrange
            var request = new GetIntakeApplicants
            {
                IntakeId = Guid.NewGuid(),
                Params = new AdminModels.GetIntakeApplicantOptions
                {
                    SortBy = IntakeApplicantSortField.FirstName,
                    SortDirection = Enums.SortDirection.Descending
                },
                User = _user
            };

            var programApplications = new Faker<ProgramApplication>()
                .RuleFor(o => o.IntakeId, request.IntakeId)
                .RuleFor(o => o.ApplicantFirstName, f => f.Person.FirstName)
                .RuleFor(o => o.ApplicantLastName, f => f.Person.LastName)
                .RuleFor(o => o.ApplicationId, _ => Guid.NewGuid())
                .RuleFor(x => x.ApplicationNumber, f => f.Random.ReplaceNumbers("200######"))
                .Generate(25);

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetProgramIntake(It.IsAny<Guid>())).ReturnsAsync(new Dto.ProgramIntake { Id = request.IntakeId });
            domesticContext.Setup(m => m.GetProgramApplications(It.IsAny<GetProgramApplicationsOptions>())).ReturnsAsync(programApplications);

            var handler = new GetIntakeApplicantsHandler(_logger, domesticContext.Object, _userAuthorization, _lookupCache, _apiMapper);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<AdminModels.PagedResult<AdminModels.IntakeApplicant>>();
            result.TotalCount.Should().Be(programApplications.Count);
            result.Items.Should().NotBeEmpty()
                .And.HaveSameCount(programApplications)
                .And.BeInDescendingOrder(i => i.FirstName);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void GetIntakeApplicantsHandler_ShouldFail_When_IntakeNotFound()
        {
            // Arrange
            var request = new GetIntakeApplicants
            {
                IntakeId = Guid.NewGuid(),
                Params = new AdminModels.GetIntakeApplicantOptions(),
                User = _user
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetProgramIntake(It.IsAny<Guid>())).ReturnsAsync((Dto.ProgramIntake)null);

            var handler = new GetIntakeApplicantsHandler(_logger, domesticContext.Object, _userAuthorization, _lookupCache, _apiMapper);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .WithMessage($"Intake {request.IntakeId} not found.");
        }
    }
}
