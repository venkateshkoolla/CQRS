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
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.TestFramework;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class UpsertAcademicRecordHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly IDtoMapper _dtoMapper;
        private readonly ILookupsCache _lookupsCache;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly IUserAuthorization _userAuthorization;
        private readonly RequestCache _requestCache;
        private readonly IPrincipal _user;

        public UpsertAcademicRecordHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _requestCache = new RequestCacheMock();
            _user = Mock.Of<IPrincipal>();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpsertAcademicRecord_ShouldPass_When_Creating()
        {
            // Arrange
            var academicRecord = _models.GetAcademicRecordBase().Generate();
            var applicant = _models.GetApplicant().Generate();
            applicant.Id = academicRecord.ApplicantId;

            var request = new UpsertAcademicRecord
            {
                AcademicRecord = academicRecord,
                ApplicantId = academicRecord.ApplicantId,
                User = _user
            };

            var dtoAcademicRecord = new Dto.AcademicRecord
            {
                ApplicantId = academicRecord.ApplicantId,
                CommunityInvolvementId = academicRecord.CommunityInvolvementId,
                DateCredentialAchieved = academicRecord.DateCredentialAchieved.ToNullableDateTime(),
                HighestEducationId = academicRecord.HighestEducationId,
                HighSkillsMajorId = academicRecord.HighSkillsMajorId,
                LiteracyTestId = academicRecord.LiteracyTestId
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetAcademicRecords(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.AcademicRecord>() as IList<Dto.AcademicRecord>);
            domesticContext.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicant.Id, AccountNumber = "2000123987" });
            domesticContext.Setup(m => m.CreateAcademicRecord(It.IsAny<Dto.AcademicRecordBase>())).ReturnsAsync(dtoAcademicRecord);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new UpsertAcademicRecordHandler(Mock.Of<ILogger<UpsertAcademicRecordHandler>>(), domesticContext.Object, userAuthorizationMock.Object, _lookupsCache, _dtoMapper, _apiMapper, _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(academicRecord, opt =>
                opt.Excluding(z => z.SchoolId)
                .ExcludingMissingMembers());

            domesticContext.Verify(m => m.CreateTranscript(It.IsAny<Dto.TranscriptBase>()), Times.Once);
            domesticContext.Verify(m => m.CreateAcademicRecord(It.IsAny<Dto.AcademicRecordBase>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task UpsertAcademicRecord_ShouldPass_When_Updating()
        {
            // Arrange
            var academicRecord = _models.GetAcademicRecordBase().Generate();
            academicRecord.SchoolId = null;

            var request = new UpsertAcademicRecord
            {
                AcademicRecord = academicRecord,
                ApplicantId = academicRecord.ApplicantId,
                User = _user
            };

            var dtoAcademicRecord = new Dto.AcademicRecord
            {
                ApplicantId = academicRecord.ApplicantId,
                CommunityInvolvementId = academicRecord.CommunityInvolvementId,
                DateCredentialAchieved = academicRecord.DateCredentialAchieved.ToNullableDateTime(),
                HighestEducationId = academicRecord.HighestEducationId,
                HighSkillsMajorId = academicRecord.HighSkillsMajorId,
                LiteracyTestId = academicRecord.LiteracyTestId
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = academicRecord.ApplicantId, AccountNumber = "2000123987" });
            domesticContext.Setup(m => m.GetAcademicRecords(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.AcademicRecord> { dtoAcademicRecord } as IList<Dto.AcademicRecord>);
            domesticContext.Setup(m => m.UpdateAcademicRecord(It.IsAny<Dto.AcademicRecord>())).ReturnsAsync(dtoAcademicRecord);
            domesticContext.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(new List<Dto.Transcript>() as IList<Dto.Transcript>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new UpsertAcademicRecordHandler(Mock.Of<ILogger<UpsertAcademicRecordHandler>>(), domesticContext.Object, userAuthorizationMock.Object, _lookupsCache, _dtoMapper, _apiMapper, _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(academicRecord, opt =>
                opt.Excluding(z => z.SchoolId)
                .ExcludingMissingMembers());
            domesticContext.Verify(m => m.UpdateAcademicRecord(It.IsAny<Dto.AcademicRecord>()), Times.Once);

            domesticContext.Verify(m => m.CreateAcademicRecord(It.IsAny<Dto.AcademicRecordBase>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpsertAcademicRecord_ShouldPass_When_Updating_Then_Log_MultipleRecords()
        {
            // Arrange
            var academicRecord = _models.GetAcademicRecordBase().Generate();
            academicRecord.SchoolId = null;
            var request = new UpsertAcademicRecord
            {
                AcademicRecord = academicRecord,
                ApplicantId = academicRecord.ApplicantId,
                User = _user
            };

            var dtoAcademicRecords = new List<Dto.AcademicRecord>
            {
                new Dto.AcademicRecord
                {
                    Id = Guid.NewGuid(),
                    ApplicantId = academicRecord.ApplicantId,
                    CommunityInvolvementId = academicRecord.CommunityInvolvementId,
                    DateCredentialAchieved = academicRecord.DateCredentialAchieved.ToNullableDateTime(),
                    HighestEducationId = academicRecord.HighestEducationId,
                    HighSkillsMajorId = academicRecord.HighSkillsMajorId,
                    LiteracyTestId = academicRecord.LiteracyTestId,
                    ModifiedOn = DateTime.UtcNow.AddMonths(-5)
                },
                new Dto.AcademicRecord
                {
                    Id = Guid.NewGuid(),
                    ApplicantId = academicRecord.ApplicantId,
                    CommunityInvolvementId = academicRecord.CommunityInvolvementId,
                    DateCredentialAchieved = academicRecord.DateCredentialAchieved.ToNullableDateTime(),
                    HighestEducationId = academicRecord.HighestEducationId,
                    HighSkillsMajorId = academicRecord.HighSkillsMajorId,
                    LiteracyTestId = academicRecord.LiteracyTestId,
                    ModifiedOn = DateTime.UtcNow.AddMonths(-3)
                }
            } as IList<Dto.AcademicRecord>;
            var dtoAcademicRecord = dtoAcademicRecords.OrderByDescending(a => a.ModifiedOn).First();

            var logger = new Mock<ILogger<UpsertAcademicRecordHandler>>();

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = academicRecord.ApplicantId, AccountNumber = "2000123987" });
            domesticContext.Setup(m => m.GetAcademicRecords(It.IsAny<Guid>())).ReturnsAsync(dtoAcademicRecords);
            domesticContext.Setup(m => m.UpdateAcademicRecord(It.IsAny<Dto.AcademicRecord>())).ReturnsAsync(dtoAcademicRecord);
            domesticContext.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(new List<Dto.Transcript>() as IList<Dto.Transcript>);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new UpsertAcademicRecordHandler(logger.Object, domesticContext.Object, userAuthorizationMock.Object, _lookupsCache, _dtoMapper, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().NotThrow();
            var loggerMessage = $"Applicant {request.ApplicantId} has more than one academic record. Latest record of {dtoAcademicRecord.Id} going to be updated.";
            logger.VerifyLogInformation(loggerMessage);
            domesticContext.Verify(m => m.UpdateAcademicRecord(It.IsAny<Dto.AcademicRecord>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpsertAcademicRecord_ShouldThrow_When_Creating_And_SchoolIdEmpty()
        {
            // Arrange
            var academicRecord = _models.GetAcademicRecordBase().Generate();
            academicRecord.SchoolId = null;
            var applicant = _models.GetApplicant().Generate();
            applicant.Id = academicRecord.ApplicantId;

            var request = new UpsertAcademicRecord
            {
                AcademicRecord = academicRecord,
                ApplicantId = academicRecord.ApplicantId,
                User = _user
            };

            var dtoAcademicRecord = new Dto.AcademicRecord
            {
                ApplicantId = academicRecord.ApplicantId,
                CommunityInvolvementId = academicRecord.CommunityInvolvementId,
                DateCredentialAchieved = academicRecord.DateCredentialAchieved.ToNullableDateTime(),
                HighestEducationId = academicRecord.HighestEducationId,
                HighSkillsMajorId = academicRecord.HighSkillsMajorId,
                LiteracyTestId = academicRecord.LiteracyTestId
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetAcademicRecords(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.AcademicRecord>() as IList<Dto.AcademicRecord>);
            domesticContext.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = applicant.Id, AccountNumber = "2000123987" });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new UpsertAcademicRecordHandler(Mock.Of<ILogger<UpsertAcademicRecordHandler>>(), domesticContext.Object, userAuthorizationMock.Object, _lookupsCache, _dtoMapper, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("'School Id' must not be empty when creating academic record");

            domesticContext.Verify(m => m.CreateTranscript(It.IsAny<Dto.TranscriptBase>()), Times.Never);
            domesticContext.Verify(m => m.CreateAcademicRecord(It.IsAny<Dto.AcademicRecordBase>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpsertAcademicRecord_ShouldThrow_When_Updating_And_SchoolIdNotEmpty()
        {
            // Arrange
            var academicRecord = _models.GetAcademicRecordBase().Generate();
            academicRecord.SchoolId = Guid.NewGuid();
            var request = new UpsertAcademicRecord
            {
                AcademicRecord = academicRecord,
                ApplicantId = academicRecord.ApplicantId,
                User = _user
            };

            var dtoAcademicRecord = new Dto.AcademicRecord
            {
                ApplicantId = academicRecord.ApplicantId,
                CommunityInvolvementId = academicRecord.CommunityInvolvementId,
                DateCredentialAchieved = academicRecord.DateCredentialAchieved.ToNullableDateTime(),
                HighestEducationId = academicRecord.HighestEducationId,
                HighSkillsMajorId = academicRecord.HighSkillsMajorId,
                LiteracyTestId = academicRecord.LiteracyTestId
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = academicRecord.ApplicantId, AccountNumber = "2000123987" });
            domesticContext.Setup(m => m.GetAcademicRecords(It.IsAny<Guid>())).ReturnsAsync(new List<Dto.AcademicRecord> { dtoAcademicRecord } as IList<Dto.AcademicRecord>);
            domesticContext.Setup(m => m.UpdateAcademicRecord(It.IsAny<Dto.AcademicRecord>())).ReturnsAsync(dtoAcademicRecord);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new UpsertAcademicRecordHandler(Mock.Of<ILogger<UpsertAcademicRecordHandler>>(), domesticContext.Object, userAuthorizationMock.Object, _lookupsCache, _dtoMapper, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage("'School Id' must be empty when updating academic record");

            domesticContext.Verify(m => m.UpdateAcademicRecord(It.IsAny<Dto.AcademicRecord>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void UpsertAcademicRecord_ShouldThrow_When_NoAccess()
        {
            // Arrange
            var academicRecord = _models.GetAcademicRecordBase().Generate();
            var request = new UpsertAcademicRecord
            {
                AcademicRecord = academicRecord,
                ApplicantId = academicRecord.ApplicantId,
                User = _user
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetContact(It.IsAny<Guid>())).ReturnsAsync(new Dto.Contact { Id = academicRecord.ApplicantId });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(false);
            userAuthorizationMock.Setup(x => x.IsOcasTier2User(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new UpsertAcademicRecordHandler(Mock.Of<ILogger<UpsertAcademicRecordHandler>>(), domesticContext.Object, userAuthorizationMock.Object, _lookupsCache, _dtoMapper, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }
    }
}
