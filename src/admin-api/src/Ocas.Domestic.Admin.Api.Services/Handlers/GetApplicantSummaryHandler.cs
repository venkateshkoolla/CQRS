using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetApplicantSummaryHandler : IRequestHandler<GetApplicantSummary, ApplicantSummary>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;
        private readonly IAppSettingsExtras _appSettingsExtras;
        private readonly IUserAuthorization _userAuthorization;
        private readonly string _locale;

        public GetApplicantSummaryHandler(ILogger<GetApplicantSummaryHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, ILookupsCache lookupsCache, IAppSettingsExtras appSettingsExtras, IUserAuthorization userAuthorization, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _appSettingsExtras = appSettingsExtras ?? throw new ArgumentNullException(nameof(appSettingsExtras));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<ApplicantSummary> Handle(GetApplicantSummary request, CancellationToken cancellationToken)
        {
            await _userAuthorization.CanAccessApplicantAsync(request.User, request.ApplicantId);

            var userType = _userAuthorization.GetUserType(request.User);

            var dtoApplicantSummary = await _domesticContext.GetApplicantSummary(new Dto.GetApplicantSummaryOptions
            {
                ApplicantId = request.ApplicantId,
                Locale = _locale.ToLocaleEnum()
            });
            if (dtoApplicantSummary.Contact == null) throw new NotFoundException($"applicant {request.ApplicantId} not found.");
            IList<SupportingDocument> supportingDocs = new List<SupportingDocument>();
            if (dtoApplicantSummary.SupportingDocuments.Any())
            {
                var documentTypes = await _lookupsCache.GetSupportingDocumentTypes(_locale);
                var officials = await _lookupsCache.GetOfficials(_locale);
                var institutes = dtoApplicantSummary.SupportingDocuments.Any(d => d.InstituteId.HasValue) ? await _lookupsCache.GetInstitutes() : new List<LookupItem>() as IList<LookupItem>;
                supportingDocs = _apiMapper.MapSupportingDocuments(dtoApplicantSummary.SupportingDocuments, documentTypes, officials, institutes);
            }

            IList<SupportingDocument> transcripts = new List<SupportingDocument>();
            if (dtoApplicantSummary.Transcripts.Any())
            {
                var colleges = await _lookupsCache.GetColleges(_locale);
                var universities = await _lookupsCache.GetUniversities();
                var filteredTranscripts = dtoApplicantSummary.Transcripts.Where(x => x.TranscriptType != TranscriptType.OntarioHighSchoolTranscript).ToList();
                transcripts = _apiMapper.MapSupportingDocuments(filteredTranscripts, colleges, universities);
            }

            IList<SupportingDocument> standardizedTests = new List<SupportingDocument>();
            if (dtoApplicantSummary.Tests.Any())
            {
                standardizedTests = _apiMapper.MapSupportingDocuments(dtoApplicantSummary.Tests, await _lookupsCache.GetStandardizedTestTypes(_locale));
            }

            AcademicRecord academicRecord = null;
            IList<SupportingDocument> academicSupportingDocs = new List<SupportingDocument>();
            if (dtoApplicantSummary.AcademicRecords.Any())
            {
                var dtoAcademicRecord = dtoApplicantSummary.AcademicRecords.OrderByDescending(a => a.ModifiedOn).FirstOrDefault();
                var highSchools = await _lookupsCache.GetHighSchools(_locale);
                var highSchoolTranscripts = dtoApplicantSummary.Transcripts.Where(x => x.TranscriptType == TranscriptType.OntarioHighSchoolTranscript).ToList();
                academicRecord = _apiMapper.MapAcademicRecord(dtoAcademicRecord, highSchools.Where(h => highSchoolTranscripts.Any(t => t.PartnerId == h.Id)).ToList());

                academicSupportingDocs.Add(_apiMapper.MapSupportingDocument(dtoAcademicRecord));
            }

            var applicant = _apiMapper.MapApplicant(
               dtoApplicantSummary.Contact,
               await _lookupsCache.GetAboriginalStatuses(_locale),
               await _lookupsCache.GetTitles(_locale),
               await _lookupsCache.GetSources(_locale),
               await _lookupsCache.GetColleges(_locale),
               await _lookupsCache.GetReferralPartners());

            var educations = new List<Education>();
            foreach (var education in dtoApplicantSummary.Educations)
            {
                educations.Add(await _apiMapper.MapEducation(
                    education,
                    await _lookupsCache.GetInstituteTypes(_locale),
                    await _lookupsCache.GetColleges(_locale),
                    await _lookupsCache.GetHighSchools(_locale),
                    await _lookupsCache.GetUniversities(),
                    _domesticContext));
            }

            IList<OntarioStudentCourseCredit> ontarioStudentCourseCredits = new List<OntarioStudentCourseCredit>();
            if (dtoApplicantSummary.OntarioStudentCourseCredits.Any())
            {
                ontarioStudentCourseCredits = _apiMapper.MapOntarioStudentCourseCredits(dtoApplicantSummary.OntarioStudentCourseCredits);
            }

            var applicationSummaries = new List<ApplicationSummary>();
            foreach (var dtoApplicationSummary in dtoApplicantSummary.ApplicationSummaries)
            {
                var applicationSummary = _apiMapper.MapApplicationSummary(
                    dtoApplicationSummary,
                    dtoApplicantSummary.Contact,
                    await GetProgramIntakes(dtoApplicationSummary.ProgramChoices),
                    await _lookupsCache.GetApplicationStatuses(_locale),
                    await _lookupsCache.GetInstituteTypes(_locale),
                    await _lookupsCache.GetTranscriptTransmissions(_locale),
                    await _lookupsCache.GetOfferStates(_locale),
                    await _lookupsCache.GetProgramIntakeAvailabilities(_locale),
                    _appSettingsExtras);
                applicationSummaries.Add(applicationSummary);
            }

            ApplicantSummary applicantSummary = null;

            switch (userType)
            {
                case UserType.CollegeUser:
                    var colleges = await _lookupsCache.GetColleges(_locale);
                    var college = colleges.FirstOrDefault(x => x.Code == request.User.GetPartnerId());
                    var universities = await _lookupsCache.GetUniversities();

                    var filteredTranscripts = dtoApplicantSummary.Transcripts.Where(x => x.TranscriptType == TranscriptType.OntarioHighSchoolTranscript || x.PartnerId == college.Id);
                    var collegeUserTranscripts = _apiMapper.MapSupportingDocuments(filteredTranscripts?.ToList(), colleges, universities);

                    var docsCollegeUser = supportingDocs
                                              .Concat(collegeUserTranscripts)
                                              .Concat(standardizedTests)
                                              .Concat(academicSupportingDocs)
                                              .OrderBy(d => d.ReceivedDate)
                                              .ThenBy(d => d.Name)
                                              .ToList();

                    applicant.Source = string.Empty;
                    applicant.AccountStatusId = Guid.Empty;

                    applicantSummary = new ApplicantSummary
                    {
                        Applicant = applicant,
                        AcademicRecord = academicRecord,
                        Educations = educations,
                        OntarioStudentCourseCredits = ontarioStudentCourseCredits,
                        SupportingDocuments = docsCollegeUser,
                        ApplicationSummaries = ApplicationSummaryCollegeUser(college.Id, applicationSummaries)
                    };
                    break;
                case UserType.HighSchoolUser:
                case UserType.HighSchoolBoardUser:
                    var docsHighSchoolUser = supportingDocs
                                              .Concat(standardizedTests)
                                              .Concat(academicSupportingDocs)
                                              .OrderBy(d => d.ReceivedDate)
                                              .ThenBy(d => d.Name)
                                              .ToList();

                    applicant.Source = string.Empty;
                    applicant.AccountStatusId = Guid.Empty;

                    applicantSummary = new ApplicantSummary
                    {
                        Applicant = applicant,
                        AcademicRecord = academicRecord,
                        Educations = educations,
                        OntarioStudentCourseCredits = ontarioStudentCourseCredits,
                        SupportingDocuments = docsHighSchoolUser,
                        ApplicationSummaries = ApplicationSummaryHighSchoolUser(applicationSummaries)
                    };
                    break;
                case UserType.OcasUser:
                    var docsOcasUser = supportingDocs
                          .Concat(transcripts)
                          .Concat(standardizedTests)
                          .Concat(academicSupportingDocs)
                          .OrderBy(d => d.ReceivedDate)
                          .ThenBy(d => d.Name)
                          .ToList();

                    applicantSummary = new ApplicantSummary
                    {
                        Applicant = applicant,
                        AcademicRecord = academicRecord,
                        Educations = educations,
                        OntarioStudentCourseCredits = ontarioStudentCourseCredits,
                        SupportingDocuments = docsOcasUser,
                        ApplicationSummaries = applicationSummaries
                    };
                    break;
                default:
                    throw new ForbiddenException();
            }

            return applicantSummary;
        }

        private async Task<IList<Dto.ProgramIntake>> GetProgramIntakes(IList<Dto.ProgramChoice> programChoices)
        {
            var filterChoices = programChoices.Where(c => c.SequenceNumber <= Constants.ProgramChoices.MaxTotalChoices).ToList();
            IList<Dto.ProgramIntake> intakes = null;
            if (filterChoices.Any())
            {
                var intakeIds = filterChoices.Select(c => c.ProgramIntakeId).Distinct().ToList();
                intakes = await _domesticContext.GetProgramIntakes(new Dto.GetProgramIntakeOptions { Ids = intakeIds });
            }

            return intakes;
        }

        private List<ApplicationSummary> ApplicationSummaryCollegeUser(Guid collegeId, List<ApplicationSummary> applicationSummaries)
        {
            var filteredApplicationSummaries = new List<ApplicationSummary>();
            foreach (var applicationSummary in applicationSummaries)
            {
                if (!applicationSummary.ProgramChoices.Any(x => x.CollegeId == collegeId))
                {
                    continue;
                }

                applicationSummary.FinancialTransactions = null;
                applicationSummary.ShoppingCartDetails = null;
                applicationSummary.ProgramChoices = applicationSummary.ProgramChoices.Where(x => x.CollegeId == collegeId).ToList();
                applicationSummary.Offers = applicationSummary.Offers.Where(x => x.CollegeId == collegeId).ToList();
                applicationSummary.TranscriptRequests = applicationSummary.TranscriptRequests.Where(x => x.ToInstituteId == collegeId || x.ToInstituteId == null).ToList();

                filteredApplicationSummaries.Add(applicationSummary);
            }

            return filteredApplicationSummaries;
        }

        private List<ApplicationSummary> ApplicationSummaryHighSchoolUser(List<ApplicationSummary> applicationSummaries)
        {
            foreach (var applicationSummary in applicationSummaries)
            {
                applicationSummary.Offers = null;
                applicationSummary.TranscriptRequests = null;
                applicationSummary.FinancialTransactions = null;
                applicationSummary.ShoppingCartDetails = null;
            }

            return applicationSummaries;
        }
    }
}
