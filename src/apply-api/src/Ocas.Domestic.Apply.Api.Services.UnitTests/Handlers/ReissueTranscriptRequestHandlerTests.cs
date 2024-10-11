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
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class ReissueTranscriptRequestHandlerTests
    {
        private readonly ILogger<ReissueTranscriptRequestHandler> _logger;
        private readonly ModelFakerFixture _models;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IPrincipal _user;
        private readonly ILookupsCache _lookupsCache;
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;

        public ReissueTranscriptRequestHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _logger = Mock.Of<ILogger<ReissueTranscriptRequestHandler>>();
            _models = XunitInjectionCollection.ModelFakerFixture;
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _user = Mock.Of<IPrincipal>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task ReissueTranscriptRequest_ShouldPass()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new ReissueTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var statuses = await _lookupsCache.GetTranscriptRequestStatuses(Constants.Localization.EnglishCanada);
            var notFoundStatusId = statuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.TranscriptNotFound).Id;
            var applicant = _models.GetApplicant().Generate();
            var application = _models.GetApplication().Generate();
            var education = _models.GetEducation().Generate();
            var contact = new Dto.Contact
            {
                Id = applicant.Id,
                AccountNumber = applicant.AccountNumber
            };
            var dtoApplication = new Dto.Application
            {
                Id = application.Id,
                ApplicationNumber = application.ApplicationNumber
            };
            var dtoEducation = new Dto.Education
            {
                Id = education.Id
            };
            var transcriptRequest = new Dto.TranscriptRequest
            {
                ApplicantId = contact.Id,
                ApplicationId = dtoApplication.Id,
                EtmsTranscriptRequestId = Guid.NewGuid(),
                FromSchoolType = TranscriptSchoolType.HighSchool,
                TranscriptRequestStatusId = notFoundStatusId
            };
            var etmsTranscriptRequest = new Dto.EtmsTranscriptRequest
            {
                Id = transcriptRequest.EtmsTranscriptRequestId.Value
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(transcriptRequest);
            domesticContextMock.Setup(m => m.GetTranscriptRequests(It.IsAny<Dto.GetTranscriptRequestOptions>())).ReturnsAsync(new List<Dto.TranscriptRequest> { transcriptRequest });
            domesticContextMock.Setup(m => m.GetEtmsTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(etmsTranscriptRequest);
            domesticContextMock.Setup(m => m.GetEtmsTranscriptRequestProcesses(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.EtmsTranscriptRequestProcess>
            {
                new Dto.EtmsTranscriptRequestProcess
                {
                    Id = Guid.NewGuid(),
                    ProcessStartDate = DateTime.UtcNow,
                    EtmsProcessType = EtmsProcessType.TranscriptRequest
                }
            });
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(dtoApplication);

            var handler = new ReissueTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            domesticContextMock.Verify(e => e.UpdateEtmsTranscriptRequest(It.IsAny<Dto.EtmsTranscriptRequest>()), Times.Once);
            domesticContextMock.Verify(e => e.UpdateEtmsTranscriptRequestProcess(It.IsAny<Dto.EtmsTranscriptRequestProcess>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void ReissueTranscriptRequest_ShouldThrow_WhenRequestNotFound()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new ReissueTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync((Dto.TranscriptRequest)null);

            var handler = new ReissueTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>()
                .And.Message.Should().Be("Transcript request not found.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void ReissueTranscriptRequest_ShouldThrow_WhenTrHasNoApplicant()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new ReissueTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(new Dto.TranscriptRequest
            {
                Id = transcriptRequestId,
                ApplicantId = null
            });

            var handler = new ReissueTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Transcript request {transcriptRequestId} does not have an applicant.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void ReissueTranscriptRequest_ShouldThrow_WhenTrHasNoApplication()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new ReissueTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(new Dto.TranscriptRequest
            {
                Id = transcriptRequestId,
                ApplicantId = Guid.NewGuid(),
                ApplicationId = null
            });

            var handler = new ReissueTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Transcript request {transcriptRequestId} does not have an application.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void ReissueTranscriptRequest_ShouldThrow_WhenNoEtmsTr()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new ReissueTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(new Dto.TranscriptRequest
            {
                Id = transcriptRequestId,
                ApplicantId = Guid.NewGuid(),
                ApplicationId = Guid.NewGuid(),
                EtmsTranscriptRequestId = null
            });

            var handler = new ReissueTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Transcript request {transcriptRequestId} does not have an associated eTMS TR.");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task ReissueTranscriptRequest_ShouldThrow_WhenInvalidHighSchoolStatus()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new ReissueTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var statuses = await _lookupsCache.GetTranscriptRequestStatuses(Constants.Localization.EnglishCanada);
            var invalidStatuses = statuses
                .Where(x => x.Code != Constants.TranscriptRequestStatuses.TranscriptNotFound)
                .ToList();
            var transcriptRequestStatusId = _dataFakerFixture.Faker.PickRandom(invalidStatuses).Id;
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(new Dto.TranscriptRequest
            {
                ApplicantId = Guid.NewGuid(),
                ApplicationId = Guid.NewGuid(),
                EtmsTranscriptRequestId = Guid.NewGuid(),
                FromSchoolType = TranscriptSchoolType.HighSchool,
                TranscriptRequestStatusId = transcriptRequestStatusId
            });

            var handler = new ReissueTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Cannot re-issue a High School TR in status: {transcriptRequestStatusId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task ReissueTranscriptRequest_ShouldThrow_WhenInvalidPostSecondaryStatus()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new ReissueTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var fromSchoolType = _dataFakerFixture.Faker.Random.Bool() ? TranscriptSchoolType.College : TranscriptSchoolType.University;
            var statuses = await _lookupsCache.GetTranscriptRequestStatuses(Constants.Localization.EnglishCanada);
            var invalidStatuses = statuses
                .Where(x => x.Code != Constants.TranscriptRequestStatuses.TranscriptNotFound && x.Code != Constants.TranscriptRequestStatuses.NoGradesOnRecord)
                .ToList();
            var transcriptRequestStatusId = _dataFakerFixture.Faker.PickRandom(invalidStatuses).Id;
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(new Dto.TranscriptRequest
            {
                ApplicantId = Guid.NewGuid(),
                ApplicationId = Guid.NewGuid(),
                EtmsTranscriptRequestId = Guid.NewGuid(),
                FromSchoolType = fromSchoolType,
                TranscriptRequestStatusId = transcriptRequestStatusId
            });

            var handler = new ReissueTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Cannot re-issue a Post-Secondary TR in status: {transcriptRequestStatusId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task ReissueTranscriptRequest_ShouldThrow_WhenNoEtmsProcess()
        {
            // Arrange
            var transcriptRequestId = Guid.NewGuid();
            var request = new ReissueTranscriptRequest
            {
                TranscriptRequestId = transcriptRequestId,
                User = _user
            };

            var statuses = await _lookupsCache.GetTranscriptRequestStatuses(Constants.Localization.EnglishCanada);
            var notFoundStatusId = statuses.Single(x => x.Code == Constants.TranscriptRequestStatuses.TranscriptNotFound).Id;
            var applicant = _models.GetApplicant().Generate();
            var application = _models.GetApplication().Generate();
            var education = _models.GetEducation().Generate();
            var contact = new Dto.Contact
            {
                Id = applicant.Id,
                AccountNumber = applicant.AccountNumber
            };
            var dtoApplication = new Dto.Application
            {
                Id = application.Id,
                ApplicationNumber = application.ApplicationNumber
            };
            var dtoEducation = new Dto.Education
            {
                Id = education.Id
            };
            var transcriptRequest = new Dto.TranscriptRequest
            {
                ApplicantId = contact.Id,
                ApplicationId = dtoApplication.Id,
                EtmsTranscriptRequestId = Guid.NewGuid(),
                FromSchoolType = TranscriptSchoolType.HighSchool,
                TranscriptRequestStatusId = notFoundStatusId
            };
            var etmsTranscriptRequest = new Dto.EtmsTranscriptRequest
            {
                Id = transcriptRequest.EtmsTranscriptRequestId.Value
            };
            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(transcriptRequest);
            domesticContextMock.Setup(m => m.GetTranscriptRequests(It.IsAny<Dto.GetTranscriptRequestOptions>())).ReturnsAsync(new List<Dto.TranscriptRequest> { transcriptRequest });
            domesticContextMock.Setup(m => m.GetEtmsTranscriptRequest(It.IsAny<Guid>())).ReturnsAsync(etmsTranscriptRequest);
            domesticContextMock.Setup(m => m.GetEtmsTranscriptRequestProcesses(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.EtmsTranscriptRequestProcess>());
            domesticContextMock.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(contact);
            domesticContextMock.Setup(m => m.GetApplication(It.IsAny<Guid>())).ReturnsAsync(dtoApplication);

            var handler = new ReissueTranscriptRequestHandler(_logger, domesticContextMock.Object, _userAuthorization, _apiMapper, _lookupsCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .And.Message.Should().Be($"Could not find current EtmsTranscriptRequestProcess for etmsTR.Id: {transcriptRequest.EtmsTranscriptRequestId}");
        }
    }
}
