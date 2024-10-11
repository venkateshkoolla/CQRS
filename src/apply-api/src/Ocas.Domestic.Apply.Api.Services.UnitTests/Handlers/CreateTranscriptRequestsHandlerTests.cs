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
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class CreateTranscriptRequestsHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly ILogger<CreateTranscriptRequestsHandler> _logger;
        private readonly ModelFakerFixture _models;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IPrincipal _user;

        public CreateTranscriptRequestsHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = new LookupsCacheMock().Object;
            _logger = Mock.Of<ILogger<CreateTranscriptRequestsHandler>>();
            _models = XunitInjectionCollection.ModelFakerFixture;
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _user = Mock.Of<IPrincipal>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldPass_When_NewApply()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1);
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var application = new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId, ApplicationNumber = "001900123" };
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId, Id = Guid.NewGuid() };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });
            domesticContextMock.Setup(m => m.CreateTranscriptRequest(It.IsAny<Dto.TranscriptRequestBase>())).ReturnsAsync(new Dto.TranscriptRequest
            {
                ApplicantId = applicantId,
                ApplicationId = application.Id,
                EducationId = education.Id,
                FromSchoolId = transcriptRequestBase.First().FromInstituteId,
                ToSchoolId = transcriptRequestBase.First().ToInstituteId,
                ModifiedBy = _user.GetUpnOrEmail(),
                TranscriptRequestStatusId = _models.AllApplyLookups.TranscriptRequestStatuses.First(x => x.Code == Constants.TranscriptRequestStatuses.WaitingPayment).Id,
                TranscriptTransmissionId = transcriptRequestBase.First().TransmissionId
            });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.CreateTranscriptRequestLog(It.IsAny<Dto.TranscriptRequestLogBase>()), Times.Never);
            domesticContextMock.Verify(e => e.CreateTranscriptRequest(It.IsAny<Dto.TranscriptRequestBase>()), Times.Once);
            domesticContextMock.Verify(e => e.UpdateTranscriptRequestLog(It.IsAny<Dto.TranscriptRequestLog>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldPass_When_ApplicationPaid()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1, "default, Highschool");
            transcriptRequestBase.First().FromInstituteId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.HighSchools.Where(h => h.HasEtms && (h.TranscriptFee == 0 || h.TranscriptFee == null))).Id;
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active)).Id;
            var application = new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId, ApplicationNumber = "001900123" };
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId, Id = Guid.NewGuid() };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(application);
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });
            domesticContextMock.Setup(m => m.CreateTranscriptRequestLog(It.IsAny<Dto.TranscriptRequestLogBase>())).ReturnsAsync(new Dto.TranscriptRequestLog
            {
                Id = Guid.NewGuid(),
                ProcessStatus = ProcessStatus.NotProcessed
            });
            domesticContextMock.Setup(m => m.CreateTranscriptRequest(It.IsAny<Dto.TranscriptRequestBase>())).ReturnsAsync(new Dto.TranscriptRequest
            {
                Id = Guid.NewGuid(),
                ApplicantId = applicantId,
                ApplicationId = application.Id,
                EducationId = education.Id,
                FromSchoolId = transcriptRequestBase.First().FromInstituteId,
                ToSchoolId = transcriptRequestBase.First().ToInstituteId,
                ModifiedBy = _user.GetUpnOrEmail(),
                TranscriptRequestStatusId = _models.AllApplyLookups.TranscriptRequestStatuses.First(x => x.Code == Constants.TranscriptRequestStatuses.RequestInit).Id,
                TranscriptTransmissionId = transcriptRequestBase.First().TransmissionId
            });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.CreateTranscriptRequestLog(It.IsAny<Dto.TranscriptRequestLogBase>()), Times.Once);
            domesticContextMock.Verify(e => e.CreateTranscriptRequest(It.IsAny<Dto.TranscriptRequestBase>()), Times.Once);
            domesticContextMock.Verify(e => e.UpdateTranscriptRequestLog(It.IsAny<Dto.TranscriptRequestLog>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_ApplicationMissing()
        {
            // Arrange
            var transcriptRequest = _models.GetTranscriptRequestBase().Generate(1);
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequest,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync((Dto.Application)null);

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be("Application does not exist.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_EducationsMissing()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1);
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education>());

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be($"Applicant does not have education for institute {transcriptRequestBase.First().FromInstituteId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_ChoicesMissing()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1, "default, University");
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId, Id = Guid.NewGuid() };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice>());

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be($"Application does not have choice(s) for institute {transcriptRequestBase.First().ToInstituteId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_InstituteTypeMissing()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1);
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = Guid.NewGuid() };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("Education institute type not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_TransmissionMissing()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1);
            transcriptRequestBase.First().TransmissionId = Guid.NewGuid();
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Transmission {transcriptRequestBase.First().TransmissionId} not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_TransmissionTypeNotMatchesEducationType()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1);
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId };
            var transmission = new Models.TranscriptTransmission { Id = transcriptRequestBase.First().TransmissionId, InstituteTypeId = _models.AllApplyLookups.InstituteTypes.First(x => x.Id != instituteTypeId).Id };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var lookupCacheMock = Mock.Get(_lookupsCache);
            lookupCacheMock.Setup(m => m.GetTranscriptTransmissions(It.IsAny<string>())).ReturnsAsync(new List<Models.TranscriptTransmission> { transmission });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, lookupCacheMock.Object);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("TransmissionId must be match the Institute type of the transcript request.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_ToInstituteIdMissing()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1);
            transcriptRequestBase.First().ToInstituteId = Guid.NewGuid();
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("ToInstituteId must be an exisiting college.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_HighSchoolRequest_ToInstituteIdNotEmpty()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1, "default, Highschool");
            transcriptRequestBase.First().ToInstituteId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Colleges.Where(c => c.HasEtms)).Id;
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("ToInstituteId must be empty for high school transcript request.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_HighSchoolRequest_TransmissionIdNotSendNow()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1, "default, Highschool");
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId };
            var transmission = new Models.TranscriptTransmission { Id = transcriptRequestBase.First().TransmissionId, InstituteTypeId = _models.AllApplyLookups.InstituteTypes.First(x => x.Code == Constants.InstituteTypes.HighSchool).Id, Code = "X" };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            var lookupCacheMock = Mock.Get(_lookupsCache);
            lookupCacheMock.Setup(m => m.GetTranscriptTransmissions(It.IsAny<string>())).ReturnsAsync(new List<Models.TranscriptTransmission> { transmission });

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("TransmissionId must be 'Send Now' for high school transcript request.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_HighSchoolRequest_NotEtms()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1, "default, Highschool");
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId };
            var highSchool = new Models.HighSchool { HasEtms = false, Id = transcriptRequestBase.First().FromInstituteId };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            var lookupCacheMock = Mock.Get(_lookupsCache);
            lookupCacheMock.Setup(m => m.GetHighSchools(It.IsAny<string>())).ReturnsAsync(new List<Models.HighSchool> { highSchool });

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("High School must support ETMS transcript request.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_CollegeRequest_ToInstituteIdEmpty()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1, "default, College");
            transcriptRequestBase.First().ToInstituteId = null;
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("ToInstituteId must exist for college transcript request.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_CollegeRequest_NotEtms()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1, "default, College");
            transcriptRequestBase.First().FromInstituteId = _dataFakerFixture.Faker.PickRandom(_models.AllApplyLookups.Colleges.Where(c => !c.HasEtms)).Id;
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("College must support ETMS transcript request.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_UniversityRequest_ToInstituteIdEmpty()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1, "default, University");
            transcriptRequestBase.First().ToInstituteId = null;
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("ToInstituteId must exist for university transcript request.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateTranscriptRequest_ShouldThrow_When_UniversityRequest_NotEtms()
        {
            // Arrange
            var transcriptRequestBase = _models.GetTranscriptRequestBase().Generate(1, "default, University");
            var request = new CreateTranscriptRequests
            {
                TranscriptRequests = transcriptRequestBase,
                User = _user
            };

            var applicantId = Guid.NewGuid();
            var applicationStatusId = _models.AllApplyLookups.ApplicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.NewApply).Id;
            var instituteTypeId = GetFromInstituteType(transcriptRequestBase.First().FromInstituteId);
            var education = new Dto.Education { ApplicantId = applicantId, InstituteId = transcriptRequestBase.First().FromInstituteId, InstituteTypeId = instituteTypeId };
            var university = new Models.University { HasEtms = false, Id = transcriptRequestBase.First().FromInstituteId };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { HighSchoolEnrolled = false });
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { ApplicantId = applicantId, ApplicationStatusId = applicationStatusId });
            domesticContextMock.Setup(m => m.GetEducations(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.Education> { education });
            domesticContextMock.Setup(m => m.GetProgramChoices(It.IsAny<Dto.GetProgramChoicesOptions>())).ReturnsAsync(new List<Dto.ProgramChoice> { new Dto.ProgramChoice { CollegeId = transcriptRequestBase.First().ToInstituteId } });

            var lookupCacheMock = Mock.Get(_lookupsCache);
            lookupCacheMock.Setup(m => m.GetUniversities()).ReturnsAsync(new List<Models.University> { university });

            var handler = new CreateTranscriptRequestsHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, lookupCacheMock.Object);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be("University must support ETMS transcript request.");
        }

        private Guid GetFromInstituteType(Guid fromInstituteId)
        {
            var college = _models.AllApplyLookups.Colleges.FirstOrDefault(x => x.Id == fromInstituteId);
            if (college != null) return _models.AllApplyLookups.InstituteTypes.First(x => x.Code == Constants.InstituteTypes.College).Id;

            var university = _models.AllApplyLookups.Universities.FirstOrDefault(x => x.Id == fromInstituteId);
            if (university != null) return _models.AllApplyLookups.InstituteTypes.First(x => x.Code == Constants.InstituteTypes.University).Id;

            var highSchool = _models.AllApplyLookups.HighSchools.FirstOrDefault(x => x.Id == fromInstituteId);
            if (highSchool != null) return _models.AllApplyLookups.InstituteTypes.First(x => x.Code == Constants.InstituteTypes.HighSchool).Id;

            throw new Exception($"Institute Id {fromInstituteId} does not have an instityte type.");
        }
    }
}
