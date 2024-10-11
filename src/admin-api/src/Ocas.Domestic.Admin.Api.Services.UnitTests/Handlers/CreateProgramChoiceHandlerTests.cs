using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Apply.TestFramework.RuleCollections;
using Ocas.Domestic.AppSettings.Extras;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class CreateProgramChoiceHandlerTests
    {
        private readonly ILogger<CreateProgramChoiceHandler> _logger;
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFaker;
        private readonly ILookupsCache _lookupsCache;
        private readonly AppSettingsExtras _appSettingsExtras;
        private readonly RequestCacheMock _requestCache;
        private readonly IMapper _mapper;
        private readonly IDtoMapper _dtoMapper;
        private readonly TestFramework.ModelFakerFixture _modelFaker;

        public CreateProgramChoiceHandlerTests()
        {
            _logger = Mock.Of<ILogger<CreateProgramChoiceHandler>>();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dataFaker = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _appSettingsExtras = new AppSettingsExtras(new AppSettingsMock());
            _requestCache = new RequestCacheMock();
            _mapper = XunitInjectionCollection.AutoMapperFixture.CreateMapper();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldPass()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicantId = Guid.NewGuid(),
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id,
                ApplicationStatusId = _modelFaker.AllAdminLookups.ApplicationStatuses.First(a => a.Code == Constants.ApplicationStatuses.Active).Id
            };

            var college = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges);

            var program = new Dto.Program
            {
                Id = choiceRequest.ProgramId,
                Code = _dataFaker.Faker.Random.AlphaNumeric(6),
                CollegeId = college.Id
            };

            var entryLevelIndex = _modelFaker.AllAdminLookups.EntryLevels.FindIndex(x => x.Id == choiceRequest.EntryLevelId);
            var intake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                ApplicationCycleId = application.ApplicationCycleId,
                CollegeId = program.CollegeId,
                CollegeApplicationCycleId = _modelFaker.AllAdminLookups.CollegeApplicationCycles.First(c => c.MasterId == application.ApplicationCycleId && c.CollegeId == program.CollegeId).Id,
                DefaultEntrySemesterId = _modelFaker.AllAdminLookups.EntryLevels.First(e => e.Code == "01").Id,
                EntryLevels = _modelFaker.AllAdminLookups.EntryLevels.Select(e => e.Id).ToList()
            };

            // Create an applicant summary that passing mapping, not validating content
            var applicantSummary = new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact { Id = application.ApplicantId },
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id },
                        ProgramChoices = new List<Dto.ProgramChoice>(),
                        Offers = new List<Dto.Offer>(),
                        FinancialTransactions = new List<Dto.FinancialTransaction>(),
                        ShoppingCartDetails = new List<Dto.ShoppingCartDetail>(),
                        TranscriptRequests = new List<Dto.TranscriptRequest>()
                    }
                } as IList<Dto.ApplicationSummary>
            };

            Dto.ProgramChoiceBase actualChoice = null;

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(program);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake> { intake } as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.CreateProgramChoice(It.IsAny<Dto.ProgramChoiceBase>()))
                .Callback<Dto.ProgramChoiceBase>(c => actualChoice = c);
            domesticContextMock.Setup(m => m.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>()))
                .ReturnsAsync(applicantSummary);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            var sourceId = _modelFaker.AllAdminLookups.Sources.FirstOrDefault(x => x.Code == Constants.Sources.A2C2).Id;
            var expectedChoice = new Dto.ProgramChoiceBase
            {
                ApplicantId = application.ApplicantId,
                ApplicationId = application.Id,
                EffectiveDate = DateTime.UtcNow.ToDateInEstAsUtc(),
                EntryLevelId = choiceRequest.EntryLevelId,
                ModifiedBy = TestConstants.TestUser.Ocas.UpnOrEmail,
                Name = $"{application.ApplicationNumber}-{college.Code}-{program.Code}",
                PreviousYearApplied = null,
                PreviousYearAttended = null,
                ProgramIntakeId = intake.Id,
                SourceId = sourceId,
                SequenceNumber = 1
            };
            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(m => m.CreateProgramChoice(It.IsAny<Dto.ProgramChoiceBase>()), Times.Once);
            actualChoice.Should().NotBeNull()
                .And.BeEquivalentTo(expectedChoice, opt => opt.Excluding(y => y.EffectiveDate));
            actualChoice.EffectiveDate.Should().NotBeNull()
                .And.BeCloseTo(expectedChoice.EffectiveDate.Value);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldPass_When_ExistingChoice()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicantId = Guid.NewGuid(),
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id,
                ApplicationStatusId = _modelFaker.AllAdminLookups.ApplicationStatuses.First(a => a.Code == Constants.ApplicationStatuses.Active).Id
            };

            var college = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges);

            var program = new Dto.Program
            {
                Id = choiceRequest.ProgramId,
                Code = _dataFaker.Faker.Random.AlphaNumeric(6),
                CollegeId = college.Id
            };

            var intake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                ApplicationCycleId = application.ApplicationCycleId,
                CollegeId = program.CollegeId,
                CollegeApplicationCycleId = _modelFaker.AllAdminLookups.CollegeApplicationCycles.First(c => c.MasterId == application.ApplicationCycleId && c.CollegeId == program.CollegeId).Id,
                DefaultEntrySemesterId = _modelFaker.AllAdminLookups.EntryLevels.First(e => e.Code == "01").Id,
                EntryLevels = _modelFaker.AllAdminLookups.EntryLevels.Select(e => e.Id).ToList()
            };

            var existingChoice = _mapper.Map<Dto.ProgramChoice>(_modelFaker.GetProgramChoice().Generate());
            existingChoice.ProgramIntakeId = intake.Id;
            existingChoice.EntryLevelId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.EntryLevels.Where(e => e.Id != choiceRequest.EntryLevelId)).Id;
            existingChoice.PreviousYearApplied = DateTime.Now.Year;
            existingChoice.PreviousYearAttended = DateTime.Now.AddYears(-3).Year;
            existingChoice.CollegeId = college.Id;

            // Create an applicant summary that passing mapping, not validating content
            var applicantSummary = new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact { Id = application.ApplicantId },
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id },
                        ProgramChoices = new List<Dto.ProgramChoice>(),
                        Offers = new List<Dto.Offer>(),
                        FinancialTransactions = new List<Dto.FinancialTransaction>(),
                        ShoppingCartDetails = new List<Dto.ShoppingCartDetail>(),
                        TranscriptRequests = new List<Dto.TranscriptRequest>()
                    }
                } as IList<Dto.ApplicationSummary>
            };

            Dto.ProgramChoiceBase actualChoice = null;

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(program);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { existingChoice } as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake> { intake } as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.CreateProgramChoice(It.IsAny<Dto.ProgramChoiceBase>()))
                .Callback<Dto.ProgramChoiceBase>(c => actualChoice = c);
            domesticContextMock.Setup(m => m.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>()))
                .ReturnsAsync(applicantSummary);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            var sourceId = _modelFaker.AllAdminLookups.Sources.FirstOrDefault(x => x.Code == Constants.Sources.A2C2).Id;
            var expectedChoice = new Dto.ProgramChoiceBase
            {
                ApplicantId = application.ApplicantId,
                ApplicationId = application.Id,
                EffectiveDate = DateTime.UtcNow.ToDateInEstAsUtc(),
                EntryLevelId = choiceRequest.EntryLevelId,
                ModifiedBy = TestConstants.TestUser.Ocas.UpnOrEmail,
                Name = $"{application.ApplicationNumber}-{college.Code}-{program.Code}",
                PreviousYearApplied = existingChoice.PreviousYearApplied,
                PreviousYearAttended = existingChoice.PreviousYearAttended,
                ProgramIntakeId = intake.Id,
                SourceId = sourceId,
                SequenceNumber = 2
            };
            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(m => m.CreateProgramChoice(It.IsAny<Dto.ProgramChoiceBase>()), Times.Once);
            actualChoice.Should().NotBeNull()
                .And.BeEquivalentTo(expectedChoice, opt => opt.Excluding(y => y.EffectiveDate));
            actualChoice.EffectiveDate.Should().NotBeNull()
                .And.BeCloseTo(expectedChoice.EffectiveDate.Value);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_NotOcasUser()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var domesticContextMock = new DomesticContextMock();

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_Application_NotFound()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync((Dto.Application)null);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .WithMessage("Application Id not found");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_Application_ApplicationCycleId_NotFound()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = Guid.NewGuid()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("Application cycle not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_Application_ApplicationCycleId_NotActive()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status != Constants.ApplicationCycleStatuses.Active)).Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("Application cycle must be active.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_ExistingChoices_AtMax()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id
            };

            var program = new Dto.Program
            {
                Id = choiceRequest.ProgramId,
                CollegeId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges).Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(program);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(_modelFaker.GetProgramChoice().Generate(Constants.ProgramChoices.MaxTotalChoices).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage($"No more than {Constants.ProgramChoices.MaxTotalChoices} choices.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_ExistingChoices_AtCollegeMax()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id
            };

            var program = new Dto.Program
            {
                Id = choiceRequest.ProgramId,
                CollegeId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges).Id
            };

            var existingChoices = _modelFaker.GetProgramChoice().Generate(Constants.ProgramChoices.MaxCollegeChoices).Select(_mapper.Map<Dto.ProgramChoice>).ToList();
            existingChoices.ForEach(c => c.CollegeId = program.CollegeId);

            var intake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                ApplicationCycleId = application.ApplicationCycleId,
                CollegeId = program.CollegeId,
                CollegeApplicationCycleId = _modelFaker.AllAdminLookups.CollegeApplicationCycles.First(c => c.MasterId == application.ApplicationCycleId && c.CollegeId == program.CollegeId).Id,
                DefaultEntrySemesterId = _modelFaker.AllAdminLookups.EntryLevels.First(e => e.Code == "01").Id,
                EntryLevels = _modelFaker.AllAdminLookups.EntryLevels.Select(e => e.Id).ToList()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(existingChoices);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(program);
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake> { intake } as IList<Dto.ProgramIntake>);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage($"No more than {Constants.ProgramChoices.MaxCollegeChoices} choices for college: {program.CollegeId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_ExistingChoice_Duplicate()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id
            };

            var program = new Dto.Program
            {
                Id = choiceRequest.ProgramId,
                CollegeId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges).Id
            };

            var existingChoice = new Dto.ProgramChoice
            {
                Id = Guid.NewGuid(),
                IntakeStartDate = choiceRequest.StartDate,
                EntryLevelId = choiceRequest.EntryLevelId,
                SequenceNumber = 1
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(program);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(new List<Dto.ProgramChoice> { existingChoice } as IList<Dto.ProgramChoice>);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("Program choice with intake and entry level already exists.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_ExistingChoice_Alternate()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id
            };

            var program = new Dto.Program
            {
                Id = choiceRequest.ProgramId,
                CollegeId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges).Id
            };

            var existingChoice = new Dto.ProgramChoice
            {
                Id = Guid.NewGuid(),
                IntakeStartDate = choiceRequest.StartDate,
                EntryLevelId = choiceRequest.EntryLevelId,
                SequenceNumber = Constants.ProgramChoices.MaxTotalChoices + 1
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(program);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(new List<Dto.ProgramChoice> { existingChoice } as IList<Dto.ProgramChoice>);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("Alternate program choice with intake and entry level already exists.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_Intakes_NotFound()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(_modelFaker.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = choiceRequest.ProgramId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake>());

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("Intake with requested start date does not exist.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_Intakes_CollegeAppCycleNotFound()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id
            };

            var intake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                ApplicationCycleId = Guid.NewGuid(),
                CollegeId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges).Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(_modelFaker.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = choiceRequest.ProgramId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake> { intake } as IList<Dto.ProgramIntake>);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("College application cycle not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_Intakes_CollegeAppCycleDoesNotMatch()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id
            };

            var intake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                ApplicationCycleId = application.ApplicationCycleId,
                CollegeId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges).Id,
                CollegeApplicationCycleId = Guid.NewGuid()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(_modelFaker.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = choiceRequest.ProgramId, CollegeId = intake.CollegeId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake> { intake } as IList<Dto.ProgramIntake>);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("Intake must be in the application's cycle.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_Intakes_EntryLevelDoesNotMatch()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id
            };

            var program = new Dto.Program
            {
                Id = choiceRequest.ProgramId,
                CollegeId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges).Id
            };

            var intake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                ApplicationCycleId = application.ApplicationCycleId,
                CollegeId = program.CollegeId,
                CollegeApplicationCycleId = _modelFaker.AllAdminLookups.CollegeApplicationCycles.First(c => c.MasterId == application.ApplicationCycleId && c.CollegeId == program.CollegeId).Id,
                EntryLevels = new List<Guid> { Guid.NewGuid() }
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(_modelFaker.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(program);
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake> { intake } as IList<Dto.ProgramIntake>);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("Program choice's entry level is not on intake.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_Intakes_EntryLevelAboveMinimum()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            choiceRequest.EntryLevelId = _modelFaker.AllAdminLookups.EntryLevels.First(e => e.Code == "05").Id;
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id
            };

            var program = new Dto.Program
            {
                Id = choiceRequest.ProgramId,
                CollegeId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges).Id
            };

            var entryLevelIndex = _modelFaker.AllAdminLookups.EntryLevels.FindIndex(x => x.Id == choiceRequest.EntryLevelId);
            var intake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                ApplicationCycleId = application.ApplicationCycleId,
                CollegeId = program.CollegeId,
                CollegeApplicationCycleId = _modelFaker.AllAdminLookups.CollegeApplicationCycles.First(c => c.MasterId == application.ApplicationCycleId && c.CollegeId == program.CollegeId).Id,
                DefaultEntrySemesterId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.EntryLevels.Where((_, i) => i > entryLevelIndex)).Id,
                EntryLevels = _modelFaker.AllAdminLookups.EntryLevels.Select(e => e.Id).ToList()
            };

            var logger = new Mock<ILogger<CreateProgramChoiceHandler>>();

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(program);
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake> { intake } as IList<Dto.ProgramIntake>);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(logger.Object, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            var errorMessage = $"Program choice entry level selected  {choiceRequest.EntryLevelId} is below the default level  {intake.DefaultEntrySemesterId} .";
            func.Should().Throw<ValidationException>()
                .WithMessage(errorMessage);
            logger.VerifyLogCritical(errorMessage);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateProgramChoiceHandler_ShouldFail_When_ApplicationStatus_NotFound()
        {
            // Arrange
            var choiceRequest = _modelFaker.GetCreateProgramChoiceRequest().Generate();
            var request = new CreateProgramChoice
            {
                ApplicationId = choiceRequest.ApplicationId,
                ProgramChoice = choiceRequest,
                User = TestConstants.TestUser.Ocas.TestPrincipal
            };

            var application = new Dto.Application
            {
                Id = request.ApplicationId,
                ApplicantId = Guid.NewGuid(),
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id,
                ApplicationStatusId = Guid.NewGuid()
            };

            var college = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.Colleges.WithAppCycle(_modelFaker.AllAdminLookups.CollegeApplicationCycles, application.ApplicationCycleId));

            var program = new Dto.Program
            {
                Id = choiceRequest.ProgramId,
                Code = _dataFaker.Faker.Random.AlphaNumeric(6),
                CollegeId = college.Id
            };

            var entryLevelIndex = _modelFaker.AllAdminLookups.EntryLevels.FindIndex(x => x.Id == choiceRequest.EntryLevelId);
            var intake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                ApplicationCycleId = application.ApplicationCycleId,
                CollegeId = program.CollegeId,
                CollegeApplicationCycleId = _modelFaker.AllAdminLookups.CollegeApplicationCycles.First(c => c.MasterId == application.ApplicationCycleId && c.CollegeId == program.CollegeId).Id,
                DefaultEntrySemesterId = _modelFaker.AllAdminLookups.EntryLevels.First(e => e.Code == "01").Id,
                EntryLevels = _modelFaker.AllAdminLookups.EntryLevels.Select(e => e.Id).ToList()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(It.Is<Guid>(g => g == choiceRequest.ApplicationId))).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(program);
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>);
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake> { intake } as IList<Dto.ProgramIntake>);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.IsOcasUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateProgramChoiceHandler(_logger, userAuthorization.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, _appSettingsExtras, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage($"Application Status not found:{application.ApplicationStatusId}");
        }
    }
}
