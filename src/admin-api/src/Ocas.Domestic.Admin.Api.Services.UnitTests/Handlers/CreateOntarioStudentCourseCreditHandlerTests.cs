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
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Enums;
using Xunit;
using Xunit.Categories;
using AdminTestFramework = Ocas.Domestic.Apply.Admin.TestFramework;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class CreateOntarioStudentCourseCreditHandlerTests
    {
        private readonly IApiMapper _apiMapper;
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly IDtoMapper _dtoMapper;
        private readonly ILookupsCache _lookupsCache;
        private readonly AdminTestFramework.ModelFakerFixture _models;
        private readonly RequestCache _requestCache;

        public CreateOntarioStudentCourseCreditHandlerTests()
        {
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _requestCache = new RequestCacheMock();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task CreateOntarioStudentCourseCredit_ShouldPass_When_HSUser_With_Transcript()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();

            var request = new CreateOntarioStudentCourseCredit
            {
                User = TestConstants.TestUser.HsUser.TestPrincipal,
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };

            var highSchool = _models.AllAdminLookups.HighSchools.FirstOrDefault(x => x.Mident == request.User.GetClaimValue("partner_id"));
            var transcript = new Dto.Transcript
            {
                ContactId = ontarioStudentCourseCreditBase.ApplicantId,
                Id = Guid.NewGuid(),
                TranscriptType = TranscriptType.OntarioHighSchoolTranscript,
                PartnerId = highSchool.Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = request.OntarioStudentCourseCredit.ApplicantId } });
            domesticContextMock.Setup(m => m.GetTranscript(It.IsAny<Guid>())).ReturnsAsync(transcript);
            domesticContextMock.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(new List<Dto.Transcript>
            {
                transcript
            });
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId,
                TranscriptId = transcript.Id
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolUser);
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<OntarioStudentCourseCredit>();
            result.ApplicantId.Should().Be(ontarioStudentCourseCreditBase.ApplicantId);
            result.TranscriptId.Should().Be(transcript.Id);
            domesticContextMock.Verify(e => e.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>()), Times.Once);
            domesticContextMock.Verify(e => e.UpdateTranscript(It.IsAny<Dto.Transcript>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateOntarioStudentCourseCredit_ShouldThrow_When_HSUser_When_No_Transcript()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();

            var request = new CreateOntarioStudentCourseCredit
            {
                User = TestConstants.TestUser.HsUser.TestPrincipal,
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId,
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = request.OntarioStudentCourseCredit.ApplicantId } });
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolUser);
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            domesticContextMock.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(new List<Dto.Transcript>
            {
                new Dto.Transcript()
            });

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().WithMessage($"Ontario HS Transcript not found for applicant: {request.OntarioStudentCourseCredit.ApplicantId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateOntarioStudentCourseCredit_ShouldThrow_When_HSUser_With_Transcript_International()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();

            var request = new CreateOntarioStudentCourseCredit
            {
                User = TestConstants.TestUser.HsUser.TestPrincipal,
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId,
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };

            var highSchool = _models.AllAdminLookups.HighSchools.FirstOrDefault(x => x.Mident == request.User.GetClaimValue("partner_id"));
            var transcript = new Dto.Transcript
            {
                ContactId = ontarioStudentCourseCreditBase.ApplicantId,
                Id = Guid.NewGuid(),
                TranscriptType = TranscriptType.InternationalHighSchoolTranscript,
                PartnerId = highSchool.Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = request.OntarioStudentCourseCredit.ApplicantId } });
            domesticContextMock.Setup(m => m.GetTranscript(It.IsAny<Guid>())).ReturnsAsync(transcript);
            domesticContextMock.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(new List<Dto.Transcript>
            {
                transcript
            });
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId,
                TranscriptId = transcript.Id
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolUser);
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);
            func.Should().Throw<NotFoundException>().WithMessage($"Ontario HS Transcript not found for applicant: {request.OntarioStudentCourseCredit.ApplicantId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateOntarioStudentCourseCredit_ShouldThrow_When_HSUser_No_Supplier_Found()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();
            var request = new CreateOntarioStudentCourseCredit
            {
                User = Mock.Of<IPrincipal>(),
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = request.OntarioStudentCourseCredit.ApplicantId } });
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolUser);
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().WithMessage("Supplier mident not found");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task CreateOntarioStudentCourseCredit_ShouldPass_When_HSBoardUser_With_Transcript()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();

            var request = new CreateOntarioStudentCourseCredit
            {
                User = TestConstants.TestUser.HsUser.TestPrincipal,
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };

            var highSchool = _models.AllAdminLookups.HighSchools.FirstOrDefault(x => x.Mident == request.User.GetClaimValue("partner_id"));
            var transcript = new Dto.Transcript
            {
                ContactId = ontarioStudentCourseCreditBase.ApplicantId,
                Id = Guid.NewGuid(),
                TranscriptType = TranscriptType.OntarioHighSchoolTranscript,
                PartnerId = highSchool.Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = request.OntarioStudentCourseCredit.ApplicantId } });
            domesticContextMock.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(new List<Dto.Transcript>
            {
                transcript
            });
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId,
                TranscriptId = transcript.Id
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolBoardUser);
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<OntarioStudentCourseCredit>();
            result.ApplicantId.Should().Be(ontarioStudentCourseCreditBase.ApplicantId);
            result.TranscriptId.Should().Be(transcript.Id);
            domesticContextMock.Verify(e => e.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>()), Times.Once);
            domesticContextMock.Verify(e => e.UpdateTranscript(It.IsAny<Dto.Transcript>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateOntarioStudentCourseCredit_ShouldThrow_When_HSBoardUser_When_No_Transcript()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();

            var request = new CreateOntarioStudentCourseCredit
            {
                User = Mock.Of<IPrincipal>(),
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = request.OntarioStudentCourseCredit.ApplicantId } });
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolBoardUser);
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().WithMessage("Supplier mident not found");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateOntarioStudentCourseCredit_ShouldThrow_When_HSBoardUser_With_Transcript_International()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();

            var request = new CreateOntarioStudentCourseCredit
            {
                User = TestConstants.TestUser.HsUser.TestPrincipal,
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId,
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };

            var highSchool = _models.AllAdminLookups.HighSchools.FirstOrDefault(x => x.Mident == request.User.GetClaimValue("partner_id"));
            var transcript = new Dto.Transcript
            {
                ContactId = ontarioStudentCourseCreditBase.ApplicantId,
                Id = Guid.NewGuid(),
                TranscriptType = TranscriptType.InternationalHighSchoolTranscript,
                PartnerId = highSchool.Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = request.OntarioStudentCourseCredit.ApplicantId } });
            domesticContextMock.Setup(m => m.GetTranscript(It.IsAny<Guid>())).ReturnsAsync(transcript);
            domesticContextMock.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(new List<Dto.Transcript>
            {
                transcript
            });
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId,
                TranscriptId = transcript.Id
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.HighSchoolBoardUser);
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);
            func.Should().Throw<NotFoundException>().WithMessage($"Ontario HS Transcript not found for applicant: {request.OntarioStudentCourseCredit.ApplicantId}");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task CreateOntarioStudentCourseCredit_ShouldPass_When_OcasUser_With_Transcript()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();

            var request = new CreateOntarioStudentCourseCredit
            {
                User = TestConstants.TestUser.HsUser.TestPrincipal,
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };

            var highSchool = _models.AllAdminLookups.HighSchools.FirstOrDefault(x => x.Mident == request.User.GetClaimValue("partner_id"));
            var transcript = new Dto.Transcript
            {
                ContactId = ontarioStudentCourseCreditBase.ApplicantId,
                Id = Guid.NewGuid(),
                TranscriptType = TranscriptType.OntarioHighSchoolTranscript,
                PartnerId = highSchool.Id
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = request.OntarioStudentCourseCredit.ApplicantId } });
            domesticContextMock.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(new List<Dto.Transcript>
            {
                transcript
            });
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId,
                TranscriptId = transcript.Id
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<OntarioStudentCourseCredit>();
            result.ApplicantId.Should().Be(ontarioStudentCourseCreditBase.ApplicantId);
            result.TranscriptId.Should().Be(transcript.Id);
            domesticContextMock.Verify(e => e.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>()), Times.Once);
            domesticContextMock.Verify(e => e.UpdateTranscript(It.IsAny<Dto.Transcript>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task CreateOntarioStudentCourseCredit_ShouldPass_Create_Transcript_When_OcasUser_When_No_Transcript()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();

            var request = new CreateOntarioStudentCourseCredit
            {
                User = TestConstants.TestUser.HsUser.TestPrincipal,
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };
            var transcriptSources = new List<Dto.TranscriptSource>
            {
                new Dto.TranscriptSource { Id = Guid.NewGuid(), Code = Constants.TrancriptSources.OcasManual },
                new Dto.TranscriptSource { Id = Guid.NewGuid(), Code = Constants.TrancriptSources.Unknown }
            };

            var transcript = new Dto.Transcript
            {
                Id = Guid.NewGuid(),
                TranscriptSourceId = transcriptSources.FirstOrDefault(x => x.Code == Constants.TrancriptSources.OcasManual)?.Id,
                TranscriptType = TranscriptType.InternationalCollegeUniversityTranscript,
                ContactId = request.OntarioStudentCourseCredit.ApplicantId
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = request.OntarioStudentCourseCredit.ApplicantId } });
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId,
                TranscriptId = transcript.Id
            });
            domesticContextMock.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(new List<Dto.Transcript>
            {
                transcript
            });
            domesticContextMock.Setup(m => m.CreateTranscript(It.IsAny<Dto.TranscriptBase>())).ReturnsAsync(transcript);

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull()
                .And.BeOfType<OntarioStudentCourseCredit>();
            result.ApplicantId.Should().Be(ontarioStudentCourseCreditBase.ApplicantId);
            result.TranscriptId.Should().Be(transcript.Id);
            domesticContextMock.Verify(e => e.CreateTranscript(It.IsAny<Dto.TranscriptBase>()), Times.Once);
            domesticContextMock.Verify(e => e.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>()), Times.Once);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateOntarioStudentCourseCredit_ShouldThrow_When_OcasUser_No_Supplier_Found()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();
            var request = new CreateOntarioStudentCourseCredit
            {
                User = Mock.Of<IPrincipal>(),
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit>());
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.GetUserType(It.IsAny<IPrincipal>())).Returns(UserType.OcasUser);
            userAuthorizationMock.Setup(x => x.IsOcasTier2User(It.IsAny<IPrincipal>())).Returns(true);

            var lookupsCacheMock = new AdminTestFramework.LookupsCacheMock();
            lookupsCacheMock.Setup(m => m.GetHighSchools(It.IsAny<string>())).ReturnsAsync(new List<HighSchool>());

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, lookupsCacheMock.Object, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<NotFoundException>().WithMessage("Supplier mident not found");
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateOntarioStudentCourseCredit_ShouldThrow_When_DuplicateCourseAndDate()
        {
            // Arrange
            var ontarioStudentCourseCreditBase = _models.GetOntarioStudentCourseCreditBase().Generate();

            var request = new CreateOntarioStudentCourseCredit
            {
                User = Mock.Of<IPrincipal>(),
                OntarioStudentCourseCredit = ontarioStudentCourseCreditBase
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(m => m.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit> { new Dto.OntarioStudentCourseCredit { ApplicantId = ontarioStudentCourseCreditBase.ApplicantId, CourseCode = ontarioStudentCourseCreditBase.CourseCode, CompletedDate = ontarioStudentCourseCreditBase.CompletedDate } });
            domesticContextMock.Setup(m => m.GetTranscript(It.IsAny<Guid>())).ReturnsAsync(new Dto.Transcript());
            domesticContextMock.Setup(m => m.UpdateTranscript(It.IsAny<Dto.Transcript>())).ReturnsAsync((Dto.Transcript x) => x);
            domesticContextMock.Setup(m => m.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>())).ReturnsAsync(new Dto.OntarioStudentCourseCredit
            {
                ApplicantId = ontarioStudentCourseCreditBase.ApplicantId,
                CourseCode = ontarioStudentCourseCreditBase.CourseCode,
                CompletedDate = ontarioStudentCourseCreditBase.CompletedDate
            });

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(true);

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ValidationException>()
                .WithMessage($"OntarioStudentCourseCredit.CourseCode already exists for this Applicant: {request.OntarioStudentCourseCredit.CourseCode}");
            domesticContextMock.Verify(e => e.CreateOntarioStudentCourseCredit(It.IsAny<Dto.OntarioStudentCourseCreditBase>()), Times.Never);
        }

        [Fact]
        [UnitTest("Handlers")]
        public void CreateOntarioStudentCourseCredit_ShouldThrow_When_NoAccess()
        {
            // Arrange
            var request = new CreateOntarioStudentCourseCredit
            {
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();

            var userAuthorizationMock = new Mock<IUserAuthorization>();
            userAuthorizationMock.Setup(x => x.IsHighSchoolUser(It.IsAny<IPrincipal>())).Returns(false);
            userAuthorizationMock.Setup(x => x.IsOcasTier2User(It.IsAny<IPrincipal>())).Returns(false);

            var handler = new CreateOntarioStudentCourseCreditHandler(Mock.Of<ILogger<CreateOntarioStudentCourseCreditHandler>>(), domesticContextMock.Object, _lookupsCache, _dtoMapper, userAuthorizationMock.Object, _apiMapper, _requestCache);

            // Act
            Func<Task> func = () => handler.Handle(request, CancellationToken.None);

            // Assert
            func.Should().Throw<ForbiddenException>();
        }
    }
}