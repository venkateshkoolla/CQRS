using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Coltrane.Bds.Provider;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;
using DtoEnums = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetCollegeTransmissionsHandler : IRequestHandler<GetCollegeTransmissions, IList<CollegeTransmission>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IColtraneBdsProvider _coltraneBdsProvider;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;
        private readonly IApiMapper _apiMapper;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ITranslationsCache _translationsCache;

        public GetCollegeTransmissionsHandler(ILogger<GetCollegeTransmissionsHandler> logger, IDomesticContext domesticContext, IColtraneBdsProvider coltraneBdsProvider, ILookupsCache lookupsCache, RequestCache requestCache, IApiMapper apiMapper, IUserAuthorization userAuthorization, ITranslationsCache translationsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _coltraneBdsProvider = coltraneBdsProvider ?? throw new ArgumentNullException(nameof(coltraneBdsProvider));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _translationsCache = translationsCache ?? throw new ArgumentNullException(nameof(translationsCache));
        }

        public async Task<IList<CollegeTransmission>> Handle(GetCollegeTransmissions request, CancellationToken cancellationToken)
        {
            var application = await _domesticContext.GetApplication(request.ApplicationId)
                ?? throw new NotFoundException($"Application {request.ApplicationId} not found");

            await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId);

            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(_locale);
            var paidApplicationStatusId = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active).Id;

            var applicantSummary = await _domesticContext.GetApplicantSummary(
                new Dto.GetApplicantSummaryOptions
                {
                    ApplicantId = application.ApplicantId,
                    Locale = _locale.ToLocaleEnum(),
                    ApplicationId = application.Id,
                    IncludeFinancialTransactions = false,
                    IncludeShoppingCartDetails = false,
                    IncludeTranscriptRequests = false
                });
            var choices = applicantSummary.ApplicationSummaries[0].ProgramChoices;
            var offers = applicantSummary.ApplicationSummaries[0].Offers;
            var educations = applicantSummary.Educations;
            var dtoSupportingDocs = applicantSummary.SupportingDocuments;
            var dtoStandardizedTests = applicantSummary.Tests;
            var academicRecords = applicantSummary.AcademicRecords;

            var allColleges = await _lookupsCache.GetColleges(_locale);
            var colleges = allColleges.Where(x => choices.Any(y => y.CollegeId == x.Id)).ToList();

            var dtoCollegeTransmissions = await _coltraneBdsProvider.GetCollegeTransmissions(application.ApplicationNumber);

            var collegeTransmissions = new List<CollegeTransmission>();
            if (choices.Any()) collegeTransmissions.AddRange(ProgramChoiceTransmissions(dtoCollegeTransmissions, choices));
            if (educations.Any()) collegeTransmissions.AddRange(EducationTransmissions(application, dtoCollegeTransmissions, colleges, educations));
            if (offers.Any()) collegeTransmissions.AddRange(await OfferTransmissions(dtoCollegeTransmissions, colleges, offers));
            if (dtoSupportingDocs.Any()) collegeTransmissions.AddRange(await SupportingDocsTransmissions(application, dtoCollegeTransmissions, colleges, dtoSupportingDocs));
            if (dtoStandardizedTests.Any()) collegeTransmissions.AddRange(await StandardizedTestsTransmissions(application, dtoCollegeTransmissions, colleges, dtoStandardizedTests));
            collegeTransmissions.AddRange(ProfileDataTransmissions(application, dtoCollegeTransmissions, colleges));
            if (academicRecords.Any())
            {
                var academicRecord = academicRecords.First();
                var transcripts = await _domesticContext.GetTranscripts(new Dto.GetTranscriptOptions { ContactId = academicRecord.ApplicantId });
                var transcript = transcripts.FirstOrDefault(x => x.TranscriptType == DtoEnums.TranscriptType.OntarioHighSchoolTranscript);

                if (transcript != null)
                {
                    var dtoGrades = await _domesticContext.GetOntarioStudentCourseCredits(
                        new Dto.GetOntarioStudentCourseCreditOptions
                        {
                            TranscriptId = transcript.Id
                        });

                    if (dtoGrades.Any()) collegeTransmissions.AddRange(await GradesTransmissions(academicRecord.Id, application, dtoCollegeTransmissions, colleges, dtoGrades));
                }
                else
                {
                    _logger.LogCritical($"Ontario HS Transcript not found for applicant: {academicRecord.ApplicantId}");
                }
            }

            var isApplicationPaid = application.ApplicationStatusId == paidApplicationStatusId;

            if (!isApplicationPaid)
            {
                // if application is unpaid, then we will not send your application to any college
                collegeTransmissions.ForEach(x => x.WaitingForPayment = true);
            }
            else if (choices.Any(x => x.SupplementalFeePaid == false))
            {
                // if application is awaiting payment a supplemental fee, then we will not send your application to any college collecting that fee
                var unpaidCollegeIds = choices
                    .Where(x => x.SupplementalFeePaid == false)
                    .Select(x => x.CollegeId.GetValueOrDefault())
                    .Distinct();

                foreach (var unpaidCollegeId in unpaidCollegeIds)
                {
                    collegeTransmissions
                        .Where(x => x.CollegeId == unpaidCollegeId)
                        .ToList()
                        .ForEach(x => x.WaitingForPayment = true);
                }
            }

            return collegeTransmissions;
        }

        private IList<CollegeTransmission> ProgramChoiceTransmissions(IList<Dto.CollegeTransmission> dtoCollegeTransmissions, IList<Dto.ProgramChoice> choices)
        {
            var collegeTransmissions = dtoCollegeTransmissions
                 .Where(t => t.TransactionCode == Constants.CollegeTransmissionCodes.ProgramChoice)
                 .GroupBy(ct => new
                 {
                     CollegeCode = ct.Data.Substring(0, 4).Trim(),
                     ct.BusinessKey
                 })
                 .Select(g => new { g.Key.CollegeCode, g.Key.BusinessKey, LastLoadDateTime = g.Min(r => r.LastLoadDateTime) });

            var list = new List<CollegeTransmission>();

            foreach (var choice in choices.Where(c => c.SequenceNumber <= Constants.ProgramChoices.MaxTotalChoices))
            {
                var choiceMaxLastLoad = collegeTransmissions.FirstOrDefault(c =>
                        c.CollegeCode == choice.CollegeCode
                        && c.BusinessKey == choice.Id)
                    ?.LastLoadDateTime;
                list.Add(_apiMapper.MapCollegeTransmission(choiceMaxLastLoad, choice));
            }

            return list;
        }

        private IList<CollegeTransmission> EducationTransmissions(Dto.Application application, IList<Dto.CollegeTransmission> dtoCollegeTransmissions, IList<College> colleges, IList<Dto.Education> educations)
        {
            var collegeTransmissions = dtoCollegeTransmissions
                 .Where(t => t.TransactionCode == Constants.CollegeTransmissionCodes.Education)
                 .GroupBy(ct => new
                 {
                     CollegeCode = ct.Data.Substring(0, 4).Trim(),
                     ct.BusinessKey
                 })
                 .Select(g => new { g.Key.CollegeCode, g.Key.BusinessKey, LastLoadDateTime = g.Max(r => r.LastLoadDateTime) });

            var list = new List<CollegeTransmission>();

            foreach (var college in colleges)
            {
                foreach (var education in educations)
                {
                    var collegeTransmission = collegeTransmissions.FirstOrDefault(c =>
                                                          c.CollegeCode == college.Code
                                                          && c.BusinessKey == education.Id);

                    list.Add(_apiMapper.MapCollegeTransmission(application.Id, collegeTransmission?.LastLoadDateTime, education, college.Id));
                }
            }

            return list;
        }

        private async Task<IList<CollegeTransmission>> OfferTransmissions(IList<Dto.CollegeTransmission> dtoCollegeTransmissions, IList<College> colleges, IList<Dto.Offer> offers)
        {
            var collegeTransmissions = dtoCollegeTransmissions
                 .Where(t => t.TransactionCode == Constants.CollegeTransmissionCodes.ProgramChoice
                    && (t.TransactionType == Constants.OfferTransmissionCodes.Accepted || t.TransactionType == Constants.OfferTransmissionCodes.Declined))
                 .GroupBy(ct => new
                 {
                     CollegeCode = ct.Data.Substring(0, 4).Trim(),
                     ProgramCode = ct.Data.Substring(13, 8).Trim(),
                     CampusCode = ct.Data.Substring(21, 4).Trim(),
                     EntrySemester = ct.Data.Substring(26, 2).Trim(),
                     IntakeStartDate = ct.Data.Substring(28, 4).Trim(),
                     Delivery = ct.Data.Substring(32, 1).Trim(),
                     DateDecisionReceived = ct.Data.Substring(37, 8).Trim(),
                     ct.TransactionType
                 })
                 .Select(g => new { g.Key.CollegeCode, g.Key.ProgramCode, g.Key.CampusCode, g.Key.EntrySemester, g.Key.IntakeStartDate, g.Key.Delivery, g.Key.DateDecisionReceived, g.Key.TransactionType, LastLoadDateTime = g.Max(r => r.LastLoadDateTime) });

            var list = new List<CollegeTransmission>();

            var entryLevels = await _lookupsCache.GetEntryLevels(_locale);
            var campuses = await _lookupsCache.GetCampuses();
            var studyMethods = await _lookupsCache.GetStudyMethods(_locale);

            // filter offers to only colleges that the applicant is currently applying to (i.e. has program choices)
            foreach (var offer in offers.Where(x => colleges.Any(y => y.Id == x.CollegeId)))
            {
                var collegeTransmission = collegeTransmissions.FirstOrDefault(x =>
                                     x.CollegeCode == colleges.FirstOrDefault(y => y.Id == offer.CollegeId)?.Code
                                     && x.ProgramCode == offer.ProgramCode
                                     && x.CampusCode == campuses.FirstOrDefault(y => y.Id == offer.CampusId)?.Code
                                     && x.EntrySemester == entryLevels.FirstOrDefault(y => y.Id == offer.EntryLevelId)?.Code
                                     && x.IntakeStartDate == offer.StartDate
                                     && x.Delivery == studyMethods.FirstOrDefault(y => y.Id == offer.OfferStudyMethodId)?.Code
                                     && x.DateDecisionReceived == offer.ConfirmedDate.ToStringOrDefault(Constants.DateFormat.OfferTransmission));

                list.Add(_apiMapper.MapCollegeTransmission(collegeTransmission?.LastLoadDateTime, offer));
            }

            return list;
        }

        private async Task<IList<CollegeTransmission>> SupportingDocsTransmissions(Dto.Application application, IList<Dto.CollegeTransmission> dtoCollegeTransmissions, IList<College> colleges, IList<Dto.SupportingDocument> dtoSupportingDocs)
        {
            var collegeTransmissions = dtoCollegeTransmissions
                 .Where(t => t.TransactionCode == Constants.CollegeTransmissionCodes.SupportingDocument)
                 .GroupBy(ct => new
                 {
                     CollegeCode = ct.Data.Substring(0, 4).Trim(),
                     ct.BusinessKey
                 })
                 .Select(g => new { g.Key.CollegeCode, g.Key.BusinessKey, LastLoadDateTime = g.Max(r => r.LastLoadDateTime) });

            var list = new List<CollegeTransmission>();

            var documentPrints = await _lookupsCache.GetDocumentPrints();
            var documentTypes = await _lookupsCache.GetSupportingDocumentTypes(_locale);
            var officials = await _lookupsCache.GetOfficials(_locale);
            var institutes = dtoSupportingDocs.Any(d => d.InstituteId.HasValue) ? await _lookupsCache.GetInstitutes() : new List<LookupItem>();

            foreach (var college in colleges)
            {
                foreach (var doc in dtoSupportingDocs.Where(x => x.Availability == Domestic.Enums.SupportingDocumentAvailability.AvailableforDistribution))
                {
                    var collegeTransmission = collegeTransmissions.FirstOrDefault(c =>
                                                                       c.BusinessKey == doc.Id
                                                                       && c.CollegeCode == colleges.FirstOrDefault(x => x.Id == college.Id)?.Code);
                    var sendToColtrane = documentPrints.FirstOrDefault(x => x.DocumentTypeId == doc.DocumentTypeId && x.CollegeId == college.Id)?.SendToColtrane ?? false;
                    list.Add(_apiMapper.MapCollegeTransmission(application.Id, collegeTransmission?.LastLoadDateTime, doc, college.Id, sendToColtrane, documentTypes, officials, institutes));
                }
            }

            return list;
        }

        private async Task<IList<CollegeTransmission>> StandardizedTestsTransmissions(Dto.Application application, IList<Dto.CollegeTransmission> dtoCollegeTransmissions, IList<College> colleges, IList<Dto.Test> dtoStandardizedTests)
        {
            var collegeTransmissions = dtoCollegeTransmissions
                .Where(t => t.TransactionCode == Constants.CollegeTransmissionCodes.Test)
                .GroupBy(ct => new
                {
                    CollegeCode = ct.Data.Substring(0, 4).Trim(),
                    ct.BusinessKey
                })
                .Select(g => new { g.Key.CollegeCode, g.Key.BusinessKey, LastLoadDateTime = g.Max(r => r.LastLoadDateTime) });

            var list = new List<CollegeTransmission>();
            var testTypes = await _lookupsCache.GetStandardizedTestTypes(_locale);

            foreach (var college in colleges)
            {
                foreach (var test in dtoStandardizedTests)
                {
                    var lastLoadDateTime = collegeTransmissions.Any(x => test.Details.Any(y => y.Id == x.BusinessKey)) ? (DateTime?)collegeTransmissions.Where(x => test.Details.Any(y => y.Id == x.BusinessKey)).Max(x => x.LastLoadDateTime) : null;
                    list.Add(_apiMapper.MapCollegeTransmission(application.Id, lastLoadDateTime, test, college.Id, testTypes));
                }
            }

            return list;
        }

        private IList<CollegeTransmission> ProfileDataTransmissions(Dto.Application application, IList<Dto.CollegeTransmission> dtoCollegeTransmissions, IList<College> colleges)
        {
            var collegeTransmissions = dtoCollegeTransmissions
               .Where(t => t.TransactionCode == Constants.CollegeTransmissionCodes.Applicant)
               .GroupBy(ct => new
               {
                   CollegeCode = ct.Data.Substring(0, 4).Trim(),
                   ct.BusinessKey
               })
               .Select(g => new { g.Key.CollegeCode, g.Key.BusinessKey, LastLoadDateTime = g.Max(r => r.LastLoadDateTime) });

            var list = new List<CollegeTransmission>();
            foreach (var college in colleges)
            {
                var collegeTransmission = collegeTransmissions.FirstOrDefault(x => x.BusinessKey == application.ApplicantId && x.CollegeCode == colleges.FirstOrDefault(y => y.Id == college.Id)?.Code);
                list.Add(new CollegeTransmission
                {
                    ApplicationId = application.Id,
                    CollegeId = college.Id,
                    ContextId = application.ApplicantId,
                    Sent = collegeTransmission?.LastLoadDateTime,
                    Type = CollegeTransmissionType.ProfileData
                });
            }

            return list;
        }

        private async Task<IList<CollegeTransmission>> GradesTransmissions(Guid academicRecordId, Dto.Application application, IList<Dto.CollegeTransmission> dtoCollegeTransmissions, IList<College> colleges, IList<Dto.OntarioStudentCourseCredit> dtoGrades)
        {
            var collegeTransmissions = dtoCollegeTransmissions
               .Where(t => t.TransactionCode == Constants.CollegeTransmissionCodes.Grade)
               .GroupBy(ct => new
               {
                   CollegeCode = ct.Data.Substring(0, 4).Trim(),
                   ct.BusinessKey
               })
               .Select(g => new { g.Key.CollegeCode, g.Key.BusinessKey, LastLoadDateTime = g.Max(r => r.LastLoadDateTime) });

            var list = new List<CollegeTransmission>();
            var translationsDictionary = await _translationsCache.GetTranslations(_locale);
            var name = translationsDictionary?.Get("transcript.high_school.ontario_high_school_grades");

            foreach (var college in colleges)
            {
                var transmissions = collegeTransmissions.Where(x => dtoGrades.Any(y => y.Id == x.BusinessKey && x.CollegeCode == colleges.FirstOrDefault(c => c.Id == college.Id)?.Code));
                var transmission = transmissions.OrderByDescending(x => x.LastLoadDateTime).FirstOrDefault();

                list.Add(_apiMapper.MapCollegeTransmission(application.Id, transmission?.LastLoadDateTime, college.Id, academicRecordId, name));
            }

            return list;
        }
    }
}
