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
using Ocas.Domestic.Apply.Admin.Api.Services.Handlers;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.AppSettings.Extras;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;
using DtoEnum = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests.Handlers
{
    public class GetApplicantSummaryHandlerTests
    {
        private readonly DataFakerFixture _dataFaker;
        private readonly TestFramework.ModelFakerFixture _modelFaker;
        private readonly ILogger<GetApplicantSummaryHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IApiMapper _apiMapper;
        private readonly IDtoMapper _dtoMapper;
        private readonly RequestCacheMock _requestCache;
        private readonly ILookupsCache _lookupsCache;
        private readonly IAppSettingsExtras _appSettingsExtras;

        public GetApplicantSummaryHandlerTests()
        {
            _dataFaker = XunitInjectionCollection.DataFakerFixture;
            _modelFaker = XunitInjectionCollection.ModelFakerFixture;
            _logger = Mock.Of<ILogger<GetApplicantSummaryHandler>>();
            _mapper = XunitInjectionCollection.AutoMapperFixture.CreateMapper();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _dtoMapper = XunitInjectionCollection.AutoMapperFixture.CreateDtoMapper();
            _requestCache = new RequestCacheMock();
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _appSettingsExtras = new AppSettingsExtras(new AppSettingsMock());
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetApplicantSummaryHandler_ShouldPass_When_CollegeUser()
        {
            // Arrange
            var applicant = _modelFaker.GetApplicant().Generate();
            var request = new GetApplicantSummary
            {
                ApplicantId = applicant.Id,
                User = TestConstants.TestUser.College.TestPrincipal
            };
            var college = _modelFaker.AllAdminLookups.Colleges.First(c => c.Code == TestConstants.TestUser.College.PartnerId);

            var application = new Dto.Application
            {
                Id = Guid.NewGuid(),
                ApplicantId = request.ApplicantId,
                ApplicationCycleId = _dataFaker.Faker.PickRandom(_modelFaker.AllAdminLookups.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active)).Id,
                ApplicationStatusId = _modelFaker.AllAdminLookups.ApplicationStatuses.First(a => a.Code == Constants.ApplicationStatuses.Active).Id
            };

            var eduHighschool = _modelFaker.GetEducation().Generate("default, Canadian, Ontario, Highschool");
            var dtoEduHighschool = _mapper.Map<Dto.Education>(eduHighschool);
            var dtoTrHighSchool = new Dto.TranscriptRequest { Id = Guid.NewGuid(), ApplicantId = request.ApplicantId, ApplicationId = application.Id, FromSchoolId = eduHighschool.InstituteId.Value };

            var eduUniversity = _modelFaker.GetEducation().Generate("default, Canadian, Ontario, University");
            var dtoEduUniversity = _mapper.Map<Dto.Education>(eduHighschool);
            var dtoTrUniversity = new Dto.TranscriptRequest { Id = Guid.NewGuid(), ApplicantId = request.ApplicantId, ApplicationId = application.Id, FromSchoolId = eduUniversity.InstituteId.Value, ToSchoolId = college.Id };

            var dtoContact = new Dto.Contact { Id = applicant.Id, BirthDate = applicant.BirthDate.ToDateTime() };

            var dtoAcademicRecord = new Dto.AcademicRecord();
            _dtoMapper.PatchAcademicRecordBase(dtoAcademicRecord, _modelFaker.GetAcademicRecordBase().Generate());

            var dtoIntake = new Dto.ProgramIntake
            {
                Id = Guid.NewGuid(),
                ApplicationCycleId = application.ApplicationCycleId,
                CollegeId = college.Id,
                CollegeApplicationCycleId = _modelFaker.AllAdminLookups.CollegeApplicationCycles.First(c => c.MasterId == application.ApplicationCycleId && c.CollegeId == college.Id).Id,
                DefaultEntrySemesterId = _modelFaker.AllAdminLookups.EntryLevels.First(e => e.Code == "01").Id,
                EntryLevels = _modelFaker.AllAdminLookups.EntryLevels.Select(e => e.Id).ToList()
            };

            var programChoice = _modelFaker.GetProgramChoice().Generate();
            programChoice.CollegeId = college.Id;
            var dtoProgramChoice = _mapper.Map<Dto.ProgramChoice>(programChoice);

            var dtoTranscript = new Dto.Transcript
            {
                Id = Guid.NewGuid(),
                TranscriptType = DtoEnum.TranscriptType.OntarioHighSchoolTranscript,
                ContactId = request.ApplicantId,
                PartnerId = eduHighschool.InstituteId
            };

            var dtoApplicantSummary = new Dto.ApplicantSummary
            {
                AcademicRecords = new List<Dto.AcademicRecord> { dtoAcademicRecord },
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id },
                        ProgramChoices = new List<Dto.ProgramChoice> { dtoProgramChoice },
                        Offers = new List<Dto.Offer>(),
                        FinancialTransactions = new List<Dto.FinancialTransaction>(),
                        ShoppingCartDetails = new List<Dto.ShoppingCartDetail>(),
                        TranscriptRequests = new List<Dto.TranscriptRequest> { dtoTrHighSchool, dtoTrUniversity }
                    }
                } as IList<Dto.ApplicationSummary>,
                Contact = new Dto.Contact
                {
                    Id = application.ApplicantId,
                    BirthDate = DateTime.UtcNow.AddYears(-25),
                    SourceId = _modelFaker.AllAdminLookups.Sources.First(s => s.Code == Constants.Sources.A2C2).Id
                },
                Educations = new List<Dto.Education> { dtoEduHighschool, dtoEduUniversity } as IList<Dto.Education>,
                OntarioStudentCourseCredits = new List<Dto.OntarioStudentCourseCredit>() as IList<Dto.OntarioStudentCourseCredit>,
                Transcripts = new List<Dto.Transcript> { dtoTranscript } as IList<Dto.Transcript>,
                Tests = new List<Dto.Test>() as IList<Dto.Test>,
                SupportingDocuments = new List<Dto.SupportingDocument>() as IList<Dto.SupportingDocument>
            };

            var domesticContext = new DomesticContextMock();
            domesticContext.Setup(m => m.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(dtoApplicantSummary);
            domesticContext.Setup(m => m.GetProgramIntakes(It.IsAny<Dto.GetProgramIntakeOptions>())).ReturnsAsync(new List<Dto.ProgramIntake> { dtoIntake } as IList<Dto.ProgramIntake>);

            var userAuthorization = new Mock<IUserAuthorization>();
            userAuthorization.Setup(m => m.GetUserType(It.IsAny<IPrincipal>())).Returns(DtoEnum.UserType.CollegeUser);

            var handler = new GetApplicantSummaryHandler(_logger, domesticContext.Object, _apiMapper, _lookupsCache, _appSettingsExtras, userAuthorization.Object, _requestCache);

            // Act
            var applicantSummary = await handler.Handle(request, CancellationToken.None);

            // Assert
            applicantSummary.Should().NotBeNull().And.BeOfType<ApplicantSummary>();

            applicantSummary.Applicant.Should().NotBeNull().And.BeOfType<Applicant>();
            applicantSummary.Applicant.Source.Should().BeEmpty();
            applicantSummary.Applicant.AccountStatusId.Should().BeEmpty();

            applicantSummary.AcademicRecord.Should().NotBeNull().And.BeOfType<AcademicRecord>();
            applicantSummary.Educations.Should().NotBeNullOrEmpty().And.HaveCount(2);
            applicantSummary.OntarioStudentCourseCredits.Should().NotBeNull().And.BeOfType<List<OntarioStudentCourseCredit>>();

            applicantSummary.ApplicationSummaries.Should().ContainSingle();
            var applicationSummary = applicantSummary.ApplicationSummaries.First();

            var seneCollegeId = _modelFaker.AllAdminLookups.Colleges.First(c => c.Code == TestConstants.TestUser.College.PartnerId).Id;
            applicationSummary.ProgramChoices.Should().OnlyContain(c => c.CollegeId == seneCollegeId);
            applicationSummary.TranscriptRequests.Should().OnlyContain(t => t.ToInstituteId == seneCollegeId || t.ToInstituteId == null);
            applicationSummary.ShoppingCartDetails.Should().BeNull();
            applicationSummary.Offers.Should().BeEmpty();
        }
    }
}
