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
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Handlers;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.Apply.Services.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Data.Extras;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class UpdateProgramChoicesHandlerTests
    {
        private readonly AllLookups _lookups;
        private readonly IApiMapperBase _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly IDtoMapperBase _dtoMapper;
        private readonly ModelFakerFixture _models;
        private readonly ILogger<UpdateProgramChoicesHandler> _logger;
        private readonly IMapper _mapper;
        private readonly ILookupsCacheBase _lookupsCache;
        private readonly IPrincipal _user;
        private readonly RequestCache _requestCache;

        public UpdateProgramChoicesHandlerTests()
        {
            _logger = Mock.Of<ILogger<UpdateProgramChoicesHandler>>();
            _mapper = XunitInjectionCollection.AutoMapperFixture.CreateMapper();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapperBase();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapperBase();
            _models = XunitInjectionCollection.ModelFakerFixture;
            _lookups = _models.AllApplyLookups;
            _user = Mock.Of<IPrincipal>();
            _requestCache = new RequestCacheMock();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateProgramChoicesHandler_ShouldPass()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(3);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var programId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationStatusId, ApplicantId = applicantId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId, programId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId, BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .Returns(Task.FromResult(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>))
                .ReturnsAsync(_models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().ContainSingle();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateProgramChoicesHandler_ShouldPass_When_Supplementalfee_Not_Paid()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var programId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();
            var shoppingCartDetails = new List<Dto.ShoppingCartDetail>
                                {
                                    new Dto.ShoppingCartDetail
                                    {
                                        ApplicationId = request.ApplicationId,
                                        ReferenceId = collegeId,
                                        Description = "ShoppingCart Detail test Description"
                                    }
                                };
            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationStatusId, ApplicantId = applicantId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId, programId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId, BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(new List<Dto.ProgramChoice>())
                .ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { SupplementalFeePaid = false, CollegeId = collegeId } });
            domesticContextMock.Setup(m => m.GetShoppingCartDetails(It.IsAny<Dto.GetShoppingCartDetailOptions>(), It.IsAny<Locale>()))
                .ReturnsAsync(shoppingCartDetails);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().ContainSingle();
            result.Should().OnlyContain(x => x.SupplementalFeeDescription == shoppingCartDetails.FirstOrDefault(z => z.ReferenceId == collegeId).Description);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldUpdateBoA()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabilityOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var programId = Guid.NewGuid();
            var applicant = new Dto.Contact
            {
                Id = Guid.NewGuid(),
                BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime(),
                HighSchoolEnrolled = false,
                HighSchoolGraduated = true
            };
            var application = new Dto.Application
            {
                ApplicationCycleId = applicationCycleId,
                ApplicationStatusId = applicationStatusId,
                ApplicantId = applicant.Id
            };
            var newProgramChoices = _models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList();

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabilityOpenId, collegeAppCycleId, programId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(applicant);
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .Returns(Task.FromResult(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>))
                .Returns(Task.FromResult(newProgramChoices as IList<Dto.ProgramChoice>))
                .ReturnsAsync(newProgramChoices as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.UpdateApplication(It.IsAny<Dto.Application>()), Times.Once);
            application.BasisForAdmissionId.Should().NotBeNull();
            application.CurrentId.Should().NotBeNull();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateProgramChoiceHandler_ShouldPass_When_AlternateChoice()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(Constants.ProgramChoices.MaxCollegeChoices);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var offerStatusAcceptedId = _lookups.OfferStatuses.First(x => x.Code == Constants.Offers.Status.Accepted).Id;
            var programId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationStatusId, ApplicantId = applicantId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId, programId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(() =>
                {
                    var seq = 1;
                    var list = programChoices.Select(c =>
                    {
                        var fullChoice = _models.GetProgramChoice().Generate();
                        fullChoice.IntakeId = c.IntakeId;
                        fullChoice.EntryLevelId = Guid.NewGuid();
                        fullChoice.ApplicantId = c.ApplicantId;
                        fullChoice.ApplicationId = c.ApplicationId;
                        fullChoice.OfferStatusId = _lookups.OfferStatuses.First(x => x.Code == Constants.Offers.Status.NoDecision).Id;
                        var dtoChoice = _mapper.Map<Dto.ProgramChoice>(fullChoice);
                        dtoChoice.SequenceNumber = seq++;
                        return dtoChoice;
                    }).ToList();

                    //Alternate choice
                    var alternateChoice = _dataFakerFixture.Faker.PickRandom(programChoices);
                    list.Add(new Dto.ProgramChoice
                    {
                        ProgramIntakeId = alternateChoice.IntakeId,
                        EntryLevelId = Guid.NewGuid(),
                        ApplicantId = alternateChoice.ApplicantId,
                        ApplicationId = alternateChoice.ApplicationId,
                        OfferStatusId = offerStatusAcceptedId,
                        SequenceNumber = 6
                    });

                    return list as IList<Dto.ProgramChoice>;
                });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId, BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().HaveSameCount(programChoices, "alternate choices are not returned");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateProgramChoicesHandler_ShouldPass_When_NoChoice()
        {
            // Arrange
            var programChoice = _models.GetProgramChoice().Generate();
            programChoice.OfferStatusId = _dataFakerFixture.Faker.PickRandom(_lookups.OfferStatuses.Where(s => s.Code != Constants.Offers.Status.Accepted)).Id;
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = new List<ProgramChoiceBase>(),
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var programId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationStatusId, ApplicantId = applicantId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake> { GenerateChoiceIntake(programChoice, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId, programId) } as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId, BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(new List<Dto.ProgramChoice> { _mapper.Map<Dto.ProgramChoice>(programChoice) } as IList<Dto.ProgramChoice>)
                .ReturnsAsync(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateProgramChoiceHandler_ShouldPass_When_IntakeEntryLevels_Changed_OnExistingChoice()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(Constants.ProgramChoices.MaxCollegeChoices);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var offerStatusAcceptedId = _lookups.OfferStatuses.First(x => x.Code == Constants.Offers.Status.Accepted).Id;
            var programId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();

            // Select entrylevels that differ from the choices
            var intakes = programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId, programId)).ToList();
            foreach (var intake in intakes)
            {
                intake.EntryLevels = new List<Guid> { _lookups.EntryLevels.FirstOrDefault(e => !programChoices.Any(c => intake.Id == c.IntakeId && c.EntryLevelId == e.Id)).Id };
            }

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationStatusId, ApplicantId = applicantId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(intakes as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(() =>
                {
                    var seq = 1;
                    var list = programChoices.Select(c =>
                    {
                        var fullChoice = _models.GetProgramChoice().Generate();
                        fullChoice.IntakeId = c.IntakeId;
                        fullChoice.EntryLevelId = c.EntryLevelId;
                        fullChoice.ApplicantId = c.ApplicantId;
                        fullChoice.ApplicationId = c.ApplicationId;
                        fullChoice.OfferStatusId = _lookups.OfferStatuses.First(x => x.Code == Constants.Offers.Status.NoDecision).Id;
                        var dtoChoice = _mapper.Map<Dto.ProgramChoice>(fullChoice);
                        dtoChoice.SequenceNumber = seq++;
                        return dtoChoice;
                    }).ToList();
                    return list as IList<Dto.ProgramChoice>;
                });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId, BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().HaveSameCount(programChoices);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateProgramChoicesHandler_ShouldPass_When_IntakeEntryLevels_Unchanged_OnIntakeExpired()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var programChoices = _models.GetProgramChoice().Generate(1);

            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyClosedId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Closed).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var offerStatusAcceptedId = _lookups.OfferStatuses.First(x => x.Code == Constants.Offers.Status.Accepted).Id;
            var programIntakes = programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyClosedId, collegeAppCycleId)).ToList();
            var programId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();
            var request = new UpdateProgramChoices
            {
                ApplicationId = applicationId,
                ProgramChoices = new List<ProgramChoiceBase>
                {
                    new ProgramChoiceBase
                    {
                        ApplicantId = programChoices[0].ApplicantId,
                        ApplicationId = applicationId,
                        EffectiveDate = programChoices[0].EffectiveDate,
                        EntryLevelId = programChoices[0].EntryLevelId,
                        IntakeId = programIntakes[0].Id,
                        PreviousYearApplied = programChoices[0].PreviousYearApplied,
                        PreviousYearAttended = programChoices[0].PreviousYearAttended
                    }
                },
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationStatusId, ApplicantId = applicantId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyClosedId, collegeAppCycleId, programId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId, BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(programChoices.Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().HaveSameCount(programChoices);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpdateProgramChoicesHandler_ShouldPass_When_CollegeMatchsSourcePartner()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var programId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationStatusId, ApplicantId = applicantId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId, programId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId, BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .Returns(Task.FromResult(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>))
                .ReturnsAsync(_models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var sourcePartner = _lookups.Colleges.First(x => x.Id == collegeId).Code;
            var requestCacheMock = new RequestCache();
            requestCacheMock.AddOrUpdate(Constants.RequestCacheKeys.Partner, sourcePartner);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), requestCacheMock);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().ContainSingle();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_ApplicantCollegeHistoryBeforeDOB()
        {
            // Arrange
            var applicant = new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() };
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            programChoices.ForEach(c =>
            {
                c.PreviousYearApplied = _dataFakerFixture.Faker.Date.Past(1, applicant.BirthDate.AddYears(-1)).Year;
                c.PreviousYearAttended = _dataFakerFixture.Faker.Date.Past(2, applicant.BirthDate.AddYears(-1)).Year;
            });
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyClosedId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Closed).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCycleActiveStatusId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(applicant);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"College previous year(s) cannot be before applicant year of birth: {applicant.BirthDate.Year}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_ApplicantCollegeHistoryFuture()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            programChoices.ForEach(c =>
            {
                c.PreviousYearApplied = _dataFakerFixture.Faker.Date.Future(2, DateTime.UtcNow.AddYears(1)).Year;
                c.PreviousYearAttended = _dataFakerFixture.Faker.Date.Future(1, DateTime.UtcNow.AddYears(1)).Year;
            });
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyClosedId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Closed).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCycleActiveStatusId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("College previous year(s) cannot be in the future.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_ApplicationCycleNotFound()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var applicationCycleId = Guid.NewGuid();
            var applicationCyclePreviousStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Previous).Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCyclePreviousStatusId });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Application cycle not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_ApplicationNotActive()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Previous)).Id;
            var applicationCyclePreviousStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Previous).Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCyclePreviousStatusId });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Application cycle must be active.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_CollegeNotPartner()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen && x.AllowCba && !x.AllowCbaMultiCollegeApply).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCycleActiveStatusId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).Returns(Task.FromResult(programChoices.Select(pc =>
            {
                var intake = GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId);
                intake.EntryLevels.Clear();
                intake.EntryLevels.Add(Guid.NewGuid());
                return intake;
            }).ToList() as IList<Dto.ProgramIntake>));
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>)
                .ReturnsAsync(_models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var sourcePartner = _lookups.Colleges.First(x => x.Id != collegeId && x.IsOpen && x.AllowCba && !x.AllowCbaMultiCollegeApply).Code;
            var requestCacheMock = new RequestCache();
            requestCacheMock.AddOrUpdate(Constants.RequestCacheKeys.Partner, sourcePartner);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), requestCacheMock);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Program college must match {sourcePartner}.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_ChoiceEntryLevelIdNotIntake()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCycleActiveStatusId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).Returns(Task.FromResult(programChoices.Select(pc =>
            {
                var intake = GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId);
                intake.EntryLevels.Clear();
                intake.EntryLevels.Add(Guid.NewGuid());
                return intake;
            }).ToList() as IList<Dto.ProgramIntake>));
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .Returns(Task.FromResult(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>))
                .ReturnsAsync(_models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Program choice's entry level is not on intake.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_GreaterThanMaxCollegeChoices()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(Constants.ProgramChoices.MaxCollegeChoices + 1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCycleActiveStatusId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>)
                .ReturnsAsync(_models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"No more than {Constants.ProgramChoices.MaxCollegeChoices} choices per college.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_IntakeExpired()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(2);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyClosedId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Closed).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var programId = Guid.NewGuid();
            var applicantId = Guid.NewGuid();

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationStatusId, ApplicantId = applicantId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyClosedId, collegeAppCycleId, programId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicantId, BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                                .ReturnsAsync(() =>
                                {
                                    var seq = 1;
                                    var list = programChoices.Take(1).Select(c =>
                                    {
                                        var fullChoice = _models.GetProgramChoice().Generate();
                                        fullChoice.IntakeId = c.IntakeId;
                                        fullChoice.EntryLevelId = c.EntryLevelId;
                                        fullChoice.ApplicantId = c.ApplicantId;
                                        fullChoice.ApplicationId = c.ApplicationId;
                                        fullChoice.OfferStatusId = _lookups.OfferStatuses.First(x => x.Code == Constants.Offers.Status.NoDecision).Id;
                                        var dtoChoice = _mapper.Map<Dto.ProgramChoice>(fullChoice);
                                        dtoChoice.SequenceNumber = seq++;
                                        return dtoChoice;
                                    }).ToList();
                                    return list as IList<Dto.ProgramChoice>;
                                });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Intakes must be open or waitlisted.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoiceHandler_ShouldThrow_When_IntakeEntryLevels_Changed_OnExistingChoice_OfInactiveIntake()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            var programChoices = _models.GetProgramChoice().Generate(1);
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyClosedId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Closed).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeId = _dataFakerFixture.Faker.PickRandom(_lookups.Colleges.Where(x => x.IsOpen)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var programIntakes = programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyClosedId, collegeAppCycleId)).ToList();
            var request = new UpdateProgramChoices
            {
                ApplicationId = applicationId,
                ProgramChoices = new List<ProgramChoiceBase>
                {
                    new ProgramChoiceBase
                    {
                        ApplicantId = programChoices[0].ApplicantId,
                        ApplicationId = applicationId,
                        EffectiveDate = programChoices[0].EffectiveDate,
                        EntryLevelId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.EntryLevels.Where(e => e.Id != programChoices[0].EntryLevelId)).Id,
                        IntakeId = programIntakes[0].Id,
                        PreviousYearApplied = programChoices[0].PreviousYearApplied,
                        PreviousYearAttended = programChoices[0].PreviousYearAttended
                    }
                },
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCycleActiveStatusId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programIntakes as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = Guid.NewGuid(), CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(programChoices.Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Choice for a closed intake cannot update entry level id.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_IntakeCycleIdNotMatchApplication()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCycleActiveStatusId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).Returns(Task.FromResult(programChoices.Select(pc =>
            {
                var intake = GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, applicationCycleId);
                intake.CollegeApplicationCycleId = Guid.NewGuid();
                return intake;
            }).ToList() as IList<Dto.ProgramIntake>));
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>)
                .ReturnsAsync(_models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Intake must be in the application's cycle.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_IntakePromotional()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusCancelledId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Cancelled).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var promotionId = _lookups.Promotions.First(p => p.Code == Constants.Promotions.Promotional)?.Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCycleActiveStatusId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).Returns(Task.FromResult(programChoices.Select(pc =>
            {
                var intake = GenerateChoiceIntake(pc, intakeStatusCancelledId, intakeAvailabiltyOpenId, applicationCycleId);
                intake.PromotionId = promotionId;
                return intake;
            }).ToList() as IList<Dto.ProgramIntake>));
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>)
                .ReturnsAsync(_models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Intakes cannot be promotional.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_IntakeStatusCancelled()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusCancelledId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Cancelled).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationCycleActiveStatusId });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusCancelledId, intakeAvailabiltyOpenId, applicationCycleId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });
            domesticContextMock.SetupSequence(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .ReturnsAsync(new List<Dto.ProgramChoice>() as IList<Dto.ProgramChoice>)
                .ReturnsAsync(_models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).ToList() as IList<Dto.ProgramChoice>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Intakes must be active.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoicesHandler_ShouldThrow_When_NoApplication()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).Returns(Task.FromResult<Dto.Application>(null));

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be("Application Id not found");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoiceHandler_ShouldThrow_When_RemoveOfferAcceptedChoice()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var offerStatusAcceptedId = _lookups.OfferStatuses.First(x => x.Code == Constants.Offers.Status.Accepted).Id;
            var programId = Guid.NewGuid();

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationStatusId, ApplicantId = Guid.NewGuid() });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId, programId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                 .Returns(Task.FromResult(programChoices.Select(c =>
                 {
                     var fullChoice = _models.GetProgramChoice().Generate();
                     fullChoice.IntakeId = c.IntakeId;
                     fullChoice.EntryLevelId = Guid.NewGuid();
                     fullChoice.ApplicantId = c.ApplicantId;
                     fullChoice.ApplicationId = c.ApplicationId;
                     fullChoice.OfferStatusId = offerStatusAcceptedId;
                     return _mapper.Map<Dto.ProgramChoice>(fullChoice);
                 }).ToList() as IList<Dto.ProgramChoice>));
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Intake offer must not be accepted for intake id(s): {string.Join(", ", programChoices.Select(o => o.IntakeId))}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpdateProgramChoiceHandler_ShouldThrow_When_MatchingAlternateChoice()
        {
            // Arrange
            var programChoices = _models.GetProgramChoiceBase().Generate(1);
            var request = new UpdateProgramChoices
            {
                ApplicationId = Guid.NewGuid(),
                ProgramChoices = programChoices,
                User = _user
            };
            var domesticContextMock = new DomesticContextMock();
            var applicationStatusId = _lookups.ApplicationStatuses.First(s => s.Code == Constants.ApplicationStatuses.Active).Id;
            var applicationCycleActiveStatusId = _lookups.ApplicationCycleStatuses.First(s => s.Code == Constants.ApplicationCycleStatuses.Active).Id;
            var intakeStatusActiveId = _lookups.ProgramIntakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            var intakeAvailabiltyOpenId = _lookups.ProgramIntakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Open).Id;
            var collegeId = _lookups.Colleges.First(x => x.IsOpen).Id;
            var applicationCycleId = _dataFakerFixture.Faker.PickRandom(_lookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id;
            var collegeAppCycleId = _lookups.CollegeApplicationCycles.First(c => c.MasterId == applicationCycleId && c.CollegeId == collegeId).Id;
            var offerStatusId = _lookups.OfferStatuses.First(x => x.Code == Constants.Offers.Status.NoDecision).Id;
            var programId = Guid.NewGuid();

            domesticContextMock.Setup(m => m.GetApplication(request.ApplicationId)).ReturnsAsync(new Dto.Application { ApplicationCycleId = applicationCycleId, ApplicationStatusId = applicationStatusId, ApplicantId = Guid.NewGuid() });
            domesticContextMock.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(programChoices.Select(pc => GenerateChoiceIntake(pc, intakeStatusActiveId, intakeAvailabiltyOpenId, collegeAppCycleId, programId)).ToList() as IList<Dto.ProgramIntake>);
            domesticContextMock.Setup(m => m.GetProgram(It.IsAny<Guid>())).ReturnsAsync(new Dto.Program { Id = programId, CollegeId = collegeId });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>()))
                .Returns(Task.FromResult(programChoices.Select(c =>
                {
                    var fullChoice = _models.GetProgramChoice().Generate();
                    fullChoice.IntakeId = c.IntakeId;
                    fullChoice.EntryLevelId = c.EntryLevelId;
                    fullChoice.ApplicantId = c.ApplicantId;
                    fullChoice.ApplicationId = c.ApplicationId;
                    fullChoice.ApplicationId = c.ApplicationId;
                    fullChoice.OfferStatusId = offerStatusId;
                    var dtoChoice = _mapper.Map<Dto.ProgramChoice>(fullChoice);
                    dtoChoice.SequenceNumber = Constants.ProgramChoices.MaxTotalChoices + 1;
                    return dtoChoice;
                }).ToList() as IList<Dto.ProgramChoice>));
            var offerIntake = _dataFakerFixture.Faker.PickRandom(programChoices);
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { BirthDate = _dataFakerFixture.Faker.Date.Past(90, DateTime.UtcNow.AddYears(-18)).ToUniversalTime() });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsOcasUser(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpdateProgramChoicesHandler(_logger, userAuthorizationMock.Object, domesticContextMock.Object, _lookupsCache, _apiMapper, _dtoMapper, new DomesticContextExtras(domesticContextMock.Object), _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Alternate choice(s) already exists for: IntakeId: {offerIntake.IntakeId} with EntryLevel: {offerIntake.EntryLevelId}");
        }

        private static Dto.ProgramIntake GenerateChoiceIntake(ProgramChoiceBase pc, Guid intakeStatusId, Guid intakeAvailabiltyId, Guid collegeApplicationCycleId, Guid? programId = null)
        {
            var actualProgramId = programId ?? Guid.NewGuid();
            return new Dto.ProgramIntake
            {
                Id = pc.IntakeId,
                ProgramIntakeStatusId = intakeStatusId,
                AvailabilityId = intakeAvailabiltyId,
                ProgramId = actualProgramId,
                CollegeApplicationCycleId = collegeApplicationCycleId,
                EntryLevels = new List<Guid> { pc.EntryLevelId },
                State = State.Active,
                Status = Status.Active,
                StartDate = DateTime.UtcNow.ToStringOrDefault(Constants.DateFormat.IntakeStartDate)
            };
        }
    }
}
