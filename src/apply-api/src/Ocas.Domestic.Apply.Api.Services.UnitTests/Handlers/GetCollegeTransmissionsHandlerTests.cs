using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Ocas.Domestic.Apply.Api.Services.Handlers;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Coltrane.Bds.Provider;
using Xunit;
using Xunit.Categories;
using Dto = Ocas.Domestic.Models;
using DtoEnums = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests.Handlers
{
    public class GetCollegeTransmissionsHandlerTests
    {
        private readonly DataFakerFixture _dataFakerFixture;
        private readonly ILookupsCache _lookupsCache;
        private readonly Faker _faker;
        private readonly ModelFakerFixture _models;
        private readonly AllLookups _lookups;
        private readonly ILogger<GetCollegeTransmissionsHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IApiMapper _apiMapper;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ITranslationsCache _translationsCache;
        private readonly RequestCache _requestCache;

        public GetCollegeTransmissionsHandlerTests()
        {
            _dataFakerFixture = XunitInjectionCollection.DataFakerFixture;
            _lookupsCache = XunitInjectionCollection.LookupsCache;
            _faker = _dataFakerFixture.Faker;
            _models = XunitInjectionCollection.ModelFakerFixture;
            _lookups = _models.AllApplyLookups;
            _logger = Mock.Of<ILogger<GetCollegeTransmissionsHandler>>();
            _mapper = XunitInjectionCollection.AutoMapperFixture.CreateMapper();
            _apiMapper = XunitInjectionCollection.AutoMapperFixture.CreateApiMapper();
            _userAuthorization = Mock.Of<IUserAuthorization>();
            _translationsCache = new TranslationsCacheMock();
            _requestCache = new RequestCacheMock();
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHandler_ShouldPass_When_ProgramChoices()
        {
            // Arrange
            var application = _models.GetApplication().Generate();

            var programChoice = _models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).First();
            programChoice.CollegeCode = _lookups.Colleges.First(c => c.Id == programChoice.CollegeId).Code;
            programChoice.CampusCode = _lookups.Campuses.First(c => c.Id == programChoice.CampusId).Code;
            programChoice.ProgramCode = _faker.Random.AlphaNumeric(8).ToUpperInvariant();
            programChoice.ApplicationId = application.Id;

            var collegeTransmission = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{programChoice.CollegeCode.PadRight(4)}",
                BusinessKey = programChoice.Id
            };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id, ApplicationStatusId = application.ApplicationStatusId },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = new List<Dto.ProgramChoice> { programChoice }
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { collegeTransmission });

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty()
                .And.ContainSingle(t => t.Type == Enums.CollegeTransmissionType.ProgramChoice);

            var ctChoice = results.FirstOrDefault(x => x.Type == Enums.CollegeTransmissionType.ProgramChoice);
            ctChoice.Should().NotBeNull();
            ctChoice.ContextId.Should().Be(programChoice.Id);
            ctChoice.CollegeId.Should().Be(programChoice.CollegeId.GetValueOrDefault());
            ctChoice.Sent.Should().NotBeNull().And.Be(collegeTransmission.LastLoadDateTime);
            ctChoice.ApplicationId.Should().Be(application.Id);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHandler_ShouldPass_When_ProgramChoices_With_Application_Paid_And_SupplementalFee_Paid()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            application.ApplicationStatusId = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active).Id;

            var programChoices = _models.GetProgramChoice().Generate(5).Select(_mapper.Map<Dto.ProgramChoice>).ToList();
            foreach (var choice in programChoices)
            {
                choice.CollegeCode = _lookups.Colleges.First(c => c.Id == choice.CollegeId).Code;
                choice.CampusCode = _lookups.Campuses.First(c => c.Id == choice.CampusId).Code;
                choice.ProgramCode = _faker.Random.AlphaNumeric(8).ToUpperInvariant();
                choice.SupplementalFeePaid = true;
            }

            var collegeTransmission1 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{_faker.PickRandom(programChoices).CollegeCode.PadRight(4)}",
                BusinessKey = _faker.PickRandom(programChoices).Id
            };

            var collegeTransmission2 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{_faker.PickRandom(programChoices).CollegeCode.PadRight(4)}",
                BusinessKey = _faker.PickRandom(programChoices).Id
            };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id, ApplicationStatusId = application.ApplicationStatusId });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id, ApplicationStatusId = application.ApplicationStatusId },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = programChoices
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { collegeTransmission1, collegeTransmission2 });

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().OnlyContain(x => !x.WaitingForPayment);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHandler_ShouldPass_When_ProgramChoices_With_Application_Paid_SupplementalFee_NotPaid()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            application.ApplicationStatusId = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active).Id;

            var programChoices = _models.GetProgramChoice().Generate(5).Select(_mapper.Map<Dto.ProgramChoice>).ToList();
            foreach (var choice in programChoices)
            {
                choice.CollegeCode = _lookups.Colleges.First(c => c.Id == choice.CollegeId).Code;
                choice.CampusCode = _lookups.Campuses.First(c => c.Id == choice.CampusId).Code;
                choice.ProgramCode = _faker.Random.AlphaNumeric(8).ToUpperInvariant();
                choice.SupplementalFeePaid = true;
            }

            var choiceWaitingForPayment = _faker.PickRandom(programChoices);
            choiceWaitingForPayment.SupplementalFeePaid = false;

            var collegeTransmission1 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{_faker.PickRandom(programChoices).CollegeCode.PadRight(4)}",
                BusinessKey = _faker.PickRandom(programChoices).Id
            };

            var collegeTransmission2 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{_faker.PickRandom(programChoices).CollegeCode.PadRight(4)}",
                BusinessKey = _faker.PickRandom(programChoices).Id
            };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id, ApplicationStatusId = application.ApplicationStatusId },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = programChoices
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { collegeTransmission1, collegeTransmission2 });

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty();
            results.Where(x => x.CollegeId == choiceWaitingForPayment.CollegeId).Should().OnlyContain(x => x.WaitingForPayment);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHandler_ShouldPass_When_ProgramChoices_With_Application_NotPaid_SupplementalFee_NotPaid()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            application.ApplicationStatusId = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.PendingPayment).Id;

            var programChoices = _models.GetProgramChoice().Generate(5).Select(_mapper.Map<Dto.ProgramChoice>).ToList();
            foreach (var choice in programChoices)
            {
                choice.CollegeCode = _lookups.Colleges.First(c => c.Id == choice.CollegeId).Code;
                choice.CampusCode = _lookups.Campuses.First(c => c.Id == choice.CampusId).Code;
                choice.ProgramCode = _faker.Random.AlphaNumeric(8).ToUpperInvariant();
                choice.SupplementalFeePaid = true;
            }

            var randomChoice = _faker.PickRandom(programChoices);
            randomChoice.SupplementalFeePaid = false;

            var choiceWaitingForPayment = _faker.PickRandom(programChoices);
            choiceWaitingForPayment.SupplementalFeePaid = false;

            var collegeTransmission1 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{_faker.PickRandom(programChoices).CollegeCode.PadRight(4)}",
                BusinessKey = _faker.PickRandom(programChoices).Id
            };

            var collegeTransmission2 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{_faker.PickRandom(programChoices).CollegeCode.PadRight(4)}",
                BusinessKey = _faker.PickRandom(programChoices).Id
            };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id, ApplicationStatusId = application.ApplicationStatusId });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id, ApplicationStatusId = application.ApplicationStatusId },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = programChoices
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { collegeTransmission1, collegeTransmission2 });

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().OnlyContain(x => x.WaitingForPayment);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHandler_ShouldPass_When_ProgramChoices_With_Application_NotPaid_SupplementalFee_Paid()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            application.ApplicationStatusId = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.PendingPayment).Id;

            var programChoices = _models.GetProgramChoice().Generate(5).Select(_mapper.Map<Dto.ProgramChoice>).ToList();
            foreach (var choice in programChoices)
            {
                choice.CollegeCode = _lookups.Colleges.First(c => c.Id == choice.CollegeId).Code;
                choice.CampusCode = _lookups.Campuses.First(c => c.Id == choice.CampusId).Code;
                choice.ProgramCode = _faker.Random.AlphaNumeric(8).ToUpperInvariant();
                choice.SupplementalFeePaid = true;
            }

            var choiceWaitingForPayment = _faker.PickRandom(programChoices);
            choiceWaitingForPayment.SupplementalFeePaid = false;

            var collegeTransmission1 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{_faker.PickRandom(programChoices).CollegeCode.PadRight(4)}",
                BusinessKey = _faker.PickRandom(programChoices).Id
            };

            var collegeTransmission2 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{_faker.PickRandom(programChoices).CollegeCode.PadRight(4)}",
                BusinessKey = _faker.PickRandom(programChoices).Id
            };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id, ApplicationStatusId = application.ApplicationStatusId });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id, ApplicationStatusId = application.ApplicationStatusId },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = programChoices
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { collegeTransmission1, collegeTransmission2 });

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().OnlyContain(x => x.WaitingForPayment);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHanlder_ShouldPass_When_Educations()
        {
            // Arrange
            var application = _models.GetApplication().Generate();

            var education = _models.GetEducation().Generate(1).Select(_mapper.Map<Dto.Education>).First();
            education.ApplicantId = application.ApplicantId;
            education.InstituteName = "TestInstitute";
            education.AttendedFrom = "2017-05";
            education.AttendedTo = "2019-07";
            education.CurrentlyAttending = false;

            var programChoice = _models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).First();
            programChoice.ApplicationId = application.Id;
            programChoice.CollegeCode = _lookups.Colleges.First(c => c.Id == programChoice.CollegeId).Code;

            var college = _lookups.Colleges.First(c => c.Id == programChoice.CollegeId);

            var collegeTransmission = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.Education,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{college.Code.PadRight(4)}",
                BusinessKey = education.Id
            };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = new List<Dto.ProgramChoice> { programChoice }
                    }
                },
                Educations = new List<Dto.Education> { education },
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { collegeTransmission });

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty()
                .And.ContainSingle(t => t.Type == Enums.CollegeTransmissionType.Education);

            var ctEducation = results.FirstOrDefault(x => x.Type == Enums.CollegeTransmissionType.Education);
            ctEducation.Should().NotBeNull();
            ctEducation.ContextId.Should().Be(education.Id);
            ctEducation.CollegeId.Should().Be(college.Id);
            ctEducation.Sent.Should().NotBeNull().And.Be(collegeTransmission.LastLoadDateTime);
            ctEducation.ApplicationId.Should().Be(application.Id);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHanlder_ShouldPass_When_Offers()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var offer = _models.GetOffer().Generate(1).Select(_mapper.Map<Dto.Offer>).First();
            offer.ApplicationId = application.Id;
            offer.ProgramCode = _faker.Random.AlphaNumeric(8).ToUpperInvariant();
            offer.StartDate = _faker.Date.Past(1).AsUtc().ToStringOrDefault("yyMM");
            offer.ConfirmedDate = _faker.Date.Past().AsUtc();
            offer.OfferStatusId = _lookups.OfferStatuses.First(x => x.Code == Constants.Offers.Status.Accepted).Id;
            var programChoice = _models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).First();
            programChoice.CollegeId = _lookups.Colleges.First(c => c.Id == offer.CollegeId).Id;
            programChoice.CollegeCode = _lookups.Colleges.First(c => c.Id == offer.CollegeId).Code;
            programChoice.ApplicationId = application.Id;

            var college = _lookups.Colleges.First(c => c.Id == offer.CollegeId);
            var campus = _lookups.Campuses.First(c => c.Id == offer.CampusId);
            var entryLevel = _lookups.EntryLevels.First(x => x.Id == offer.EntryLevelId);
            var studyMethod = _lookups.StudyMethods.First(m => m.Id == offer.OfferStudyMethodId);

            var collegeTransmission = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                TransactionType = Constants.OfferTransmissionCodes.Accepted,
                Data = $"{college.Code.PadRight(4)}---------{offer.ProgramCode.PadRight(8)}{campus.Code.PadRight(4)}-{entryLevel.Code.PadRight(2)}{offer.StartDate.PadRight(4)}{studyMethod.Code.PadRight(1)}----{offer.ConfirmedDate.ToStringOrDefault(Constants.DateFormat.OfferTransmission)}"
            };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id },
                        Offers = new List<Dto.Offer> { offer },
                        ProgramChoices = new List<Dto.ProgramChoice> { programChoice }
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { collegeTransmission });

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);
            results.Should().NotBeNullOrEmpty();

            // Assert
            results.Should().NotBeNullOrEmpty()
                .And.ContainSingle(t => t.Type == Enums.CollegeTransmissionType.Offer);

            var ctOffer = results.FirstOrDefault(x => x.Type == Enums.CollegeTransmissionType.Offer);
            ctOffer.Should().NotBeNull();
            ctOffer.ContextId.Should().Be(offer.Id);
            ctOffer.CollegeId.Should().Be(college.Id);
            ctOffer.Sent.Should().NotBeNull().And.Be(collegeTransmission.LastLoadDateTime);
            ctOffer.ApplicationId.Should().Be(application.Id);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHanlder_ShouldPass_When_Offers_StatusChanges()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var offer = _models.GetOffer().Generate(1).Select(_mapper.Map<Dto.Offer>).First();
            offer.ApplicationId = application.Id;
            offer.ProgramCode = _faker.Random.AlphaNumeric(8).ToUpperInvariant();
            offer.StartDate = _faker.Date.Past(1).AsUtc().ToStringOrDefault("yyMM");
            offer.ConfirmedDate = _faker.Date.Past().AsUtc();
            offer.OfferStatusId = _lookups.OfferStatuses.First(x => x.Code == Constants.Offers.Status.Accepted).Id;
            var programChoice = _models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).First();
            programChoice.CollegeId = _lookups.Colleges.First(c => c.Id == offer.CollegeId).Id;
            programChoice.CollegeCode = _lookups.Colleges.First(c => c.Id == offer.CollegeId).Code;
            programChoice.ApplicationId = application.Id;

            var college = _lookups.Colleges.First(c => c.Id == offer.CollegeId);
            var campus = _lookups.Campuses.First(c => c.Id == offer.CampusId);
            var entryLevel = _lookups.EntryLevels.First(x => x.Id == offer.EntryLevelId);
            var studyMethod = _lookups.StudyMethods.First(m => m.Id == offer.OfferStudyMethodId);

            var ctOfferAccepted = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Between(DateTime.Now.AddMonths(-2).AsUtc(), DateTime.Now.AddMonths(-3).AsUtc()).AsUtc(),
                TransactionType = Constants.OfferTransmissionCodes.Accepted,
                Data = $"{college.Code.PadRight(4)}---------{offer.ProgramCode.PadRight(8)}{campus.Code.PadRight(4)}-{entryLevel.Code.PadRight(2)}{offer.StartDate.PadRight(4)}{studyMethod.Code.PadRight(1)}----{offer.ConfirmedDate.ToStringOrDefault(Constants.DateFormat.OfferTransmission)}"
            };

            var ctOfferDeclined = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.ProgramChoice,
                LastLoadDateTime = _faker.Date.Between(DateTime.UtcNow, DateTime.Now.AddMonths(-1).AsUtc()),
                TransactionType = Constants.OfferTransmissionCodes.Declined,
                Data = $"{college.Code.PadRight(4)}---------{offer.ProgramCode.PadRight(8)}{campus.Code.PadRight(4)}-{entryLevel.Code.PadRight(2)}{offer.StartDate.PadRight(4)}{studyMethod.Code.PadRight(1)}----{_faker.Date.Past().AsUtc().ToStringOrDefault(Constants.DateFormat.OfferTransmission)}"
            };

            var ctList = new List<Dto.CollegeTransmission> { ctOfferAccepted, ctOfferDeclined };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id },
                        Offers = new List<Dto.Offer> { offer },
                        ProgramChoices = new List<Dto.ProgramChoice> { programChoice }
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(ctList);

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);
            results.Should().NotBeNullOrEmpty();

            // Assert
            results.Should().NotBeNullOrEmpty()
                .And.ContainSingle(t => t.Type == Enums.CollegeTransmissionType.Offer);

            var ctOffer = results.FirstOrDefault(x => x.Type == Enums.CollegeTransmissionType.Offer);
            ctOffer.Should().NotBeNull();
            ctOffer.ContextId.Should().Be(offer.Id);
            ctOffer.CollegeId.Should().Be(college.Id);
            ctOffer.Sent.Should().NotBeNull().And.Be(ctOfferAccepted.LastLoadDateTime);
            ctOffer.ApplicationId.Should().Be(application.Id);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionHandler_ShouldPass_When_SupportingDocs()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            application.ApplicantId = Guid.NewGuid();

            var documentTypes = await XunitInjectionCollection.LookupsCache.GetSupportingDocumentTypes(Constants.Localization.EnglishCanada);

            var supportingDocuments = new List<Dto.SupportingDocument>
                                        {
                                          new Dto.SupportingDocument
                                          {
                                              Id = Guid.NewGuid(),
                                              ApplicantId = application.ApplicantId,
                                              Availability = Domestic.Enums.SupportingDocumentAvailability.AvailableforDistribution,
                                              DocumentTypeId = _faker.PickRandom(documentTypes).Id
                                          },

                                          new Dto.SupportingDocument
                                          {
                                              Id = Guid.NewGuid(),
                                              ApplicantId = application.ApplicantId,
                                              Availability = Domestic.Enums.SupportingDocumentAvailability.AvailableforDistribution,
                                              DocumentTypeId = _faker.PickRandom(documentTypes).Id
                                          }
                                        };

            var programChoice = _models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).First();
            programChoice.CollegeCode = _lookups.Colleges.First(c => c.Id == programChoice.CollegeId).Code;
            programChoice.ApplicationId = application.Id;

            var collegeTransmission1 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.SupportingDocument,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{programChoice.CollegeCode.PadRight(4)}",
                BusinessKey = supportingDocuments[0].Id
            };

            var collegeTransmission2 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.SupportingDocument,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{programChoice.CollegeCode.PadRight(4)}",
                BusinessKey = supportingDocuments[1].Id
            };

            var collegeTransmissions = new List<Dto.CollegeTransmission> { collegeTransmission1, collegeTransmission2 };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id });
            domesticContextMock.Setup(x => x.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(new List<Dto.OntarioStudentCourseCredit>());
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id, ApplicantId = application.ApplicantId },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = new List<Dto.ProgramChoice> { programChoice }
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = supportingDocuments,
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(collegeTransmissions);

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().OnlyContain(x => x.ApplicationId == application.Id);

            results[0].ContextId.Should().Be(programChoice.Id);
            results[0].Type.Should().Be(Enums.CollegeTransmissionType.ProgramChoice);

            results[1].ContextId.Should().Be(supportingDocuments[0].Id);
            results[1].CollegeId.Should().Be((Guid)programChoice.CollegeId);
            results[1].Sent.Should().NotBeNull().And.Be(collegeTransmission1.LastLoadDateTime);
            results[1].RequiredToSend.Should().Be(_lookups.DocumentPrints.FirstOrDefault(x => x.DocumentTypeId == supportingDocuments[0].DocumentTypeId && x.CollegeId == programChoice.CollegeId).SendToColtrane);
            results[1].Type.Should().Be(Enums.CollegeTransmissionType.SupportingDocument);

            results[2].ContextId.Should().Be(supportingDocuments[1].Id);
            results[2].CollegeId.Should().Be((Guid)programChoice.CollegeId);
            results[2].Sent.Should().NotBeNull().And.Be(collegeTransmission2.LastLoadDateTime);
            results[2].RequiredToSend.Should().Be(_lookups.DocumentPrints.FirstOrDefault(x => x.DocumentTypeId == supportingDocuments[1].DocumentTypeId && x.CollegeId == programChoice.CollegeId).SendToColtrane);
            results[2].Type.Should().Be(Enums.CollegeTransmissionType.SupportingDocument);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHandler_ShouldPass_When_StandardizedTests()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            application.ApplicantId = Guid.NewGuid();

            var testTypes = await XunitInjectionCollection.LookupsCache.GetStandardizedTestTypes(Constants.Localization.EnglishCanada);

            var test1 = new Dto.Test
            {
                Id = Guid.NewGuid(),
                ApplicantId = application.ApplicantId,
                TestTypeId = _faker.PickRandom(testTypes).Id,
                DateTestTaken = _faker.Date.Past().AsUtc(),
                Details = new List<Dto.TestDetail>
                                            {
                                                new Dto.TestDetail { Id = Guid.NewGuid() },
                                                new Dto.TestDetail { Id = Guid.NewGuid() }
                                            }
            };

            var test2 = new Dto.Test
            {
                Id = Guid.NewGuid(),
                ApplicantId = application.ApplicantId,
                TestTypeId = _faker.PickRandom(testTypes).Id,
                DateTestTaken = _faker.Date.Past().AsUtc(),
                Details = new List<Dto.TestDetail>
                                            {
                                                new Dto.TestDetail { Id = Guid.NewGuid() },
                                                new Dto.TestDetail { Id = Guid.NewGuid() }
                                            }
            };

            var programChoice = _models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).First();
            programChoice.CollegeCode = _lookups.Colleges.First(c => c.Id == programChoice.CollegeId).Code;
            programChoice.ApplicationId = application.Id;

            var ctTest1Detail1 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.Test,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{programChoice.CollegeCode.PadRight(4)}",
                BusinessKey = test1.Details[0].Id
            };
            var ctTest1Detail2 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.Test,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{programChoice.CollegeCode.PadRight(4)}",
                BusinessKey = test1.Details[1].Id
            };
            var ctTest2Detail1 = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.Test,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{programChoice.CollegeCode.PadRight(4)}",
                BusinessKey = test2.Details[0].Id
            };
            var ctTestUnRelated = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.Test,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{programChoice.CollegeCode.PadRight(4)}"
            };
            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = new List<Dto.ProgramChoice> { programChoice }
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test> { test1, test2 }
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { ctTest1Detail1, ctTest1Detail2, ctTest2Detail1 });

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().OnlyContain(x => x.ApplicationId == application.Id);
            results[0].ContextId.Should().Be(programChoice.Id);
            results[0].Type.Should().Be(Enums.CollegeTransmissionType.ProgramChoice);

            results[1].ContextId.Should().Be(test1.Id);
            results[1].Type.Should().Be(Enums.CollegeTransmissionType.SupportingDocument);
            results[1].CollegeId.Should().Be((Guid)programChoice.CollegeId);
            results[1].Sent.Should().NotBeNull().And.Be(new List<Dto.CollegeTransmission> { ctTest1Detail1, ctTest1Detail2 }.Max(x => x.LastLoadDateTime));
            results[1].RequiredToSend.Should().BeTrue();
            results[1].Name.Should().Be($"{testTypes.FirstOrDefault(x => x.Id == test1.TestTypeId)?.Label} ({test1.DateTestTaken.ToStringOrDefault()})");

            results[2].ContextId.Should().Be(test2.Id);
            results[2].Type.Should().Be(Enums.CollegeTransmissionType.SupportingDocument);
            results[2].CollegeId.Should().Be((Guid)programChoice.CollegeId);
            results[2].Sent.Should().NotBeNull().And.Be(new List<Dto.CollegeTransmission> { ctTest2Detail1 }.Max(x => x.LastLoadDateTime));
            results[2].RequiredToSend.Should().BeTrue();
            results[2].Name.Should().Be($"{testTypes.FirstOrDefault(x => x.Id == test2.TestTypeId)?.Label} ({test2.DateTestTaken.ToStringOrDefault()})");
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHandler_ShouldPass_When_ProfileData()
        {
            // Arrange
            var application = _models.GetApplication().Generate();

            var contact = new Dto.Contact
            {
                Id = Guid.NewGuid()
            };

            application.ApplicantId = contact.Id;

            var programChoice = _models.GetProgramChoice().Generate(1).Select(_mapper.Map<Dto.ProgramChoice>).First();
            programChoice.CollegeCode = _lookups.Colleges.First(c => c.Id == programChoice.CollegeId).Code;
            programChoice.ApplicationId = application.Id;

            var collegeTransmission = new Dto.CollegeTransmission
            {
                Id = _faker.Random.Long(min: 0),
                ColtraneXcId = _faker.Random.Long(min: 0),
                ApplicationNumber = application.ApplicationNumber,
                TransactionCode = Constants.CollegeTransmissionCodes.Applicant,
                LastLoadDateTime = _faker.Date.Past().AsUtc(),
                Data = $"{programChoice.CollegeCode.PadRight(4)}",
                BusinessKey = contact.Id
            };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id, ApplicantId = contact.Id });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = contact,
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = new List<Dto.ProgramChoice> { programChoice }
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission> { collegeTransmission });

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().NotBeNullOrEmpty();
            results.Should().OnlyContain(x => x.ApplicationId == application.Id);

            results[0].ContextId.Should().Be(programChoice.Id);
            results[0].Type.Should().Be(Enums.CollegeTransmissionType.ProgramChoice);

            results[1].ContextId.Should().Be(contact.Id);
            results[1].CollegeId.Should().Be((Guid)programChoice.CollegeId);
            results[1].Sent.Should().NotBeNull().And.Be(collegeTransmission.LastLoadDateTime);
            results[1].Type.Should().Be(Enums.CollegeTransmissionType.ProfileData);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHanlder_ShouldPass_When_Grades()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var academicRecordId = Guid.NewGuid();
            var grades = new List<Dto.OntarioStudentCourseCredit>
                        {
                             new Dto.OntarioStudentCourseCredit
                            {
                                ApplicantId = application.ApplicantId,
                                Id = Guid.NewGuid()
                            },
                             new Dto.OntarioStudentCourseCredit
                             {
                                 ApplicantId = application.ApplicantId,
                                 Id = Guid.NewGuid()
                             }
                        };

            var programChoices = _models.GetProgramChoice().Generate(5).Select(_mapper.Map<Dto.ProgramChoice>).ToList();
            for (var i = 0; i < programChoices.Count; i++)
            {
                var choice = programChoices[i];
                choice.CollegeCode = _lookups.Colleges.First(c => c.Id == choice.CollegeId).Code;
                choice.ApplicationId = application.Id;
            }

            var collegeTransmissions = new Faker<Dto.CollegeTransmission>()
                                    .RuleFor(o => o.Id, f => f.Random.Long(min: 0))
                                    .RuleFor(o => o.ColtraneXcId, f => f.Random.Long(min: 0))
                                    .RuleFor(o => o.ApplicationNumber, application.ApplicationNumber)
                                    .RuleFor(o => o.TransactionCode, Constants.CollegeTransmissionCodes.Grade)
                                    .RuleFor(o => o.LastLoadDateTime, f => f.Date.Past().AsUtc())
                                    .RuleFor(o => o.Data, f => $"{f.PickRandom(programChoices).CollegeCode.PadRight(4)}")
                                    .RuleFor(o => o.BusinessKey, f => f.PickRandom(grades).Id)
                                    .Generate(10);

            var transcript = new Dto.Transcript
            {
                ContactId = application.ApplicantId,
                Id = Guid.NewGuid(),
                TranscriptType = DtoEnums.TranscriptType.OntarioHighSchoolTranscript
            };

            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id });
            domesticContextMock.Setup(x => x.GetOntarioStudentCourseCredits(It.IsAny<Dto.GetOntarioStudentCourseCreditOptions>())).ReturnsAsync(grades);
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord> { new Dto.AcademicRecord { ApplicantId = application.ApplicantId, Id = academicRecordId } },
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = programChoices
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });
            domesticContextMock.Setup(m => m.GetTranscripts(It.IsAny<Dto.GetTranscriptOptions>())).ReturnsAsync(new List<Dto.Transcript>
            {
                transcript
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(collegeTransmissions);

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            results.Should().NotBeNullOrEmpty();
            results.Should().OnlyContain(x => x.ApplicationId == application.Id);
            results.Where(x => x.Type == Enums.CollegeTransmissionType.SupportingDocument).Should().HaveSameCount(programChoices.Select(x => x.CollegeCode).Distinct());
            results.Where(x => x.Type == Enums.CollegeTransmissionType.SupportingDocument)
                   .OrderByDescending(y => y.Sent).FirstOrDefault().Sent.Should()
                   .Be(collegeTransmissions.OrderByDescending(x => x.LastLoadDateTime).FirstOrDefault().LastLoadDateTime);
            results.Where(x => x.Type == Enums.CollegeTransmissionType.SupportingDocument).FirstOrDefault().ContextId.Should()
                   .Be(academicRecordId);
        }

        [Fact]
        [UnitTest("Handlers")]
        public async Task GetCollegeTransmissionsHandler_ShouldPass_When_NoTransmissions()
        {
            // Arrange
            var application = _models.GetApplication().Generate();
            var request = new GetCollegeTransmissions
            {
                ApplicationId = application.Id,
                User = Mock.Of<IPrincipal>()
            };

            var domesticContextMock = new DomesticContextMock();
            domesticContextMock.Setup(x => x.GetApplication(It.IsAny<Guid>())).ReturnsAsync(new Dto.Application { Id = application.Id });
            domesticContextMock.Setup(x => x.GetApplicantSummary(It.IsAny<Dto.GetApplicantSummaryOptions>())).ReturnsAsync(new Dto.ApplicantSummary
            {
                Contact = new Dto.Contact(),
                AcademicRecords = new List<Dto.AcademicRecord>(),
                ApplicationSummaries = new List<Dto.ApplicationSummary>
                {
                    new Dto.ApplicationSummary
                    {
                        Application = new Dto.Application { Id = application.Id, ApplicationStatusId = application.ApplicationStatusId },
                        Offers = new List<Dto.Offer>(),
                        ProgramChoices = new List<Dto.ProgramChoice>()
                    }
                },
                Educations = new List<Dto.Education>(),
                SupportingDocuments = new List<Dto.SupportingDocument>(),
                Tests = new List<Dto.Test>()
            });

            var coltraneBdsProviderMock = new Mock<IColtraneBdsProvider>();
            coltraneBdsProviderMock.Setup(x => x.GetCollegeTransmissions(It.IsAny<string>(), It.IsAny<Dto.GetCollegeTransmissionHistoryOptions>())).ReturnsAsync(new List<Dto.CollegeTransmission>());

            var handler = new GetCollegeTransmissionsHandler(_logger, domesticContextMock.Object, coltraneBdsProviderMock.Object, _lookupsCache, _requestCache, _apiMapper, _userAuthorization, _translationsCache);

            // Act
            var results = await handler.Handle(request, CancellationToken.None);

            // Assert
            results.Should().BeNullOrEmpty();
        }
    }
}
