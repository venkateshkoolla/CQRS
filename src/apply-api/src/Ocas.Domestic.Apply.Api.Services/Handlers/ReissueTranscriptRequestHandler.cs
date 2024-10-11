using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class ReissueTranscriptRequestHandler : IRequestHandler<ReissueTranscriptRequest, IList<TranscriptRequest>>
    {
        private readonly ILogger<ReissueTranscriptRequestHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;

        public ReissueTranscriptRequestHandler(
            ILogger<ReissueTranscriptRequestHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, IApiMapper apiMapper, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        // from A2C: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FControllers%2FTranscriptRequestController.cs&version=GBmaster&line=200&lineStyle=plain&lineEnd=331&lineStartColumn=1&lineEndColumn=1
        public async Task<IList<TranscriptRequest>> Handle(ReissueTranscriptRequest request, CancellationToken cancellationToken)
        {
            var transcriptRequest = await _domesticContext.GetTranscriptRequest(request.TranscriptRequestId)
                ?? throw new NotFoundException("Transcript request not found.");

            if (!transcriptRequest.ApplicantId.HasValue)
            {
                var applicantLogEx = $"Transcript request {transcriptRequest.Id} does not have an applicant.";
                _logger.LogCritical(applicantLogEx);
                throw new ValidationException(applicantLogEx);
            }

            await _userAuthorization.CanAccessApplicantAsync(request.User, transcriptRequest.ApplicantId.Value);

            if (!transcriptRequest.ApplicationId.HasValue)
            {
                var applicationLogEx = $"Transcript request {transcriptRequest.Id} does not have an application.";
                _logger.LogCritical(applicationLogEx);
                throw new ValidationException(applicationLogEx);
            }

            if (transcriptRequest.EtmsTranscriptRequestId.IsEmpty())
            {
                var etmsLogEx = $"Transcript request {transcriptRequest.Id} does not have an associated eTMS TR.";
                _logger.LogCritical(etmsLogEx);
                throw new ValidationException(etmsLogEx);
            }

            var requestStatuses = await _lookupsCache.GetTranscriptRequestStatuses(Constants.Localization.EnglishCanada);
            var reissueStatusId = requestStatuses.First(st => st.Code == Constants.TranscriptRequestStatuses.RequestReissue).Id;
            var notFoundStatusId = requestStatuses.First(st => st.Code == Constants.TranscriptRequestStatuses.TranscriptNotFound).Id;
            var noGradesStatusId = requestStatuses.First(st => st.Code == Constants.TranscriptRequestStatuses.NoGradesOnRecord).Id;

            if (transcriptRequest.FromSchoolType == TranscriptSchoolType.HighSchool && transcriptRequest.TranscriptRequestStatusId != notFoundStatusId)
            {
                throw new ValidationException($"Cannot re-issue a High School TR in status: {transcriptRequest.TranscriptRequestStatusId}");
            }
            else if (transcriptRequest.TranscriptRequestStatusId != notFoundStatusId && transcriptRequest.TranscriptRequestStatusId != noGradesStatusId)
            {
                throw new ValidationException($"Cannot re-issue a Post-Secondary TR in status: {transcriptRequest.TranscriptRequestStatusId}");
            }

            var etmsTR = await _domesticContext.GetEtmsTranscriptRequest(transcriptRequest.EtmsTranscriptRequestId.Value);

            // Get Current Process
            var processes = await _domesticContext.GetEtmsTranscriptRequestProcesses(etmsTR.Id);

            var latestProcessDate = processes.Max(p => p.ProcessStartDate);
            var latestProcess = processes.FirstOrDefault(pr => pr.EtmsProcessType == EtmsProcessType.TranscriptRequest && pr.ProcessStartDate == latestProcessDate);

            if (latestProcess is null)
            {
                var processLogEx = $"Could not find current EtmsTranscriptRequestProcess for etmsTR.Id: {etmsTR.Id}";
                _logger.LogCritical(processLogEx);
                throw new ValidationException(processLogEx);
            }

            var modifiedBy = request.User.GetUpnOrEmail();

            // First set the request back to original
            if (etmsTR.EtmsRequestType == EtmsRequestType.Reissue)
            {
                etmsTR.EtmsRequestType = EtmsRequestType.Original;

                etmsTR.ModifiedBy = modifiedBy;
                etmsTR = await _domesticContext.UpdateEtmsTranscriptRequest(etmsTR);
            }

            var latestApplicant = await _domesticContext.GetContact(transcriptRequest.ApplicantId.Value) ?? throw new NotFoundException($"Applicant does not exist: {transcriptRequest.ApplicantId}");
            var latestApplication = await _domesticContext.GetApplication(transcriptRequest.ApplicationId.Value) ?? throw new NotFoundException($"Application does not exist: {transcriptRequest.ApplicationId}");

            // update eTMS TR with latest Application Details
            etmsTR.AccountNumber = latestApplicant.AccountNumber;
            etmsTR.ApplicationNumber = latestApplication.ApplicationNumber;

            if (transcriptRequest.FromSchoolType == TranscriptSchoolType.HighSchool)
            {
                etmsTR.InstitutionName = transcriptRequest.FromSchoolName;
            }
            else
            {
                etmsTR.CampusName = transcriptRequest.FromSchoolName;
            }

            Dto.Education education = null;

            if (!transcriptRequest.EducationId.IsEmpty())
                education = await _domesticContext.GetEducation(transcriptRequest.EducationId.Value);

            if (education != null)
            {
                etmsTR.DateLastAttended = education.AttendedTo.IsDate() ? education.AttendedTo.ToDateTime() : (DateTime?)null;
                etmsTR.Graduated = education.Graduated;
                etmsTR.LegalFirstNameInFinalYearOfHighSchool = education.FirstNameOnRecord?.Trim();
                etmsTR.LegalSurnameInFinalYearOfHighSchool = education.LastNameOnRecord?.Trim();
                etmsTR.StudentNumber = education.StudentNumber?.Trim();
                etmsTR.LanguageOfInstruction = education.LanguageOfInstruction;
                etmsTR.ProgramName = education.Major;

                if (!education.LastGradeCompletedId.IsEmpty())
                {
                    var grades = await _lookupsCache.GetGrades(Constants.Localization.EnglishCanada);
                    var lastGradeCompleted = grades.First(x => x.Id == education.LastGradeCompletedId.Value);

                    var hasLastGradeCompleted = int.TryParse(lastGradeCompleted.Code, out var lastGradeCompletedValue);

                    etmsTR.LastGradeCompleted = hasLastGradeCompleted ? lastGradeCompletedValue : (int?)null;
                }

                if (!education.LevelOfStudiesId.IsEmpty())
                {
                    var studyLevels = await _lookupsCache.GetStudyLevels(Constants.Localization.EnglishCanada);
                    var studyLevel = studyLevels.First(x => x.Id == education.LevelOfStudiesId);

                    etmsTR.LevelOfStudy = studyLevel.Code;
                }
            }

            etmsTR.DateOfBirth = latestApplicant.BirthDate;
            etmsTR.Email = latestApplicant.Email;
            etmsTR.FormerSurname = latestApplicant.PreviousLastName?.Trim();
            if (!latestApplicant.GenderId.IsEmpty())
            {
                etmsTR.GenderId = latestApplicant.GenderId;
            }

            etmsTR.LegalFirstGivenName = latestApplicant.FirstName;
            etmsTR.LegalLastFamilyName = latestApplicant.LastName;
            etmsTR.MiddleName = latestApplicant.MiddleName?.Trim();

            etmsTR.OEN = latestApplicant.OntarioEducationNumber;
            etmsTR.PhoneNumber = latestApplicant.HomePhone;

            if (!latestApplicant.TitleId.IsEmpty())
            {
                var titles = await _lookupsCache.GetTitles(Constants.Localization.EnglishCanada);
                var title = titles.First(x => x.Id == latestApplicant.TitleId.Value);

                etmsTR.Title = title.Label;
            }

            // change eTMS TR to Reissue
            etmsTR.EtmsRequestType = EtmsRequestType.Reissue;

            // Update eTMS TR
            etmsTR.ModifiedBy = modifiedBy;
            await _domesticContext.UpdateEtmsTranscriptRequest(etmsTR);

            latestProcess.TranscriptRequestStatusId = reissueStatusId;

            // update eTMS TR process
            latestProcess.ModifiedBy = modifiedBy;
            await _domesticContext.UpdateEtmsTranscriptRequestProcess(latestProcess);

            // let plugins run, and return the updated TRs
            var dtoTranscriptRequests = await _domesticContext.GetTranscriptRequests(new Dto.GetTranscriptRequestOptions
            {
                ApplicationId = transcriptRequest.ApplicationId
            });

            dtoTranscriptRequests = dtoTranscriptRequests.Where(x => x.EtmsTranscriptRequestId == etmsTR.Id).ToList();

            var transcriptRequests = new List<TranscriptRequest>();
            foreach (var dtoTranscriptRequest in dtoTranscriptRequests)
            {
                transcriptRequests.Add(_apiMapper.MapTranscriptRequest(dtoTranscriptRequest, await _lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada), await _lookupsCache.GetTranscriptTransmissions(Constants.Localization.EnglishCanada)));
            }

            return transcriptRequests;
        }
    }
}
