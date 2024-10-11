using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
    public class CreateTranscriptRequestsHandler : IRequestHandler<CreateTranscriptRequests, IList<TranscriptRequest>>
    {
        private readonly ILogger<CreateTranscriptRequestsHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;

        public CreateTranscriptRequestsHandler(
            ILogger<CreateTranscriptRequestsHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, IApiMapper apiMapper, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<IList<TranscriptRequest>> Handle(CreateTranscriptRequests request, CancellationToken cancellationToken)
        {
            var transcriptRequestsBase = request.TranscriptRequests;
            var application = await _domesticContext.GetApplication(transcriptRequestsBase.First().ApplicationId)
                ?? throw new NotFoundException("Application does not exist.");

            await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId);

            var applicant = await _domesticContext.GetContact(application.ApplicantId)
                ?? throw new NotFoundException("Applicant does not exist.");

            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            var transmissions = await _lookupsCache.GetTranscriptTransmissions(Constants.Localization.EnglishCanada);
            var requestStatuses = await _lookupsCache.GetTranscriptRequestStatuses(Constants.Localization.EnglishCanada);
            var instituteTypes = await _lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada);
            var applicantEdus = await _domesticContext.GetEducations(application.ApplicantId);

            var transcriptRequests = new List<TranscriptRequest>();
            Dto.TranscriptRequest dtoTranscriptRequest;
            await _domesticContext.BeginTransaction();
            try
            {
                foreach (var tr in transcriptRequestsBase)
                {
                    var educations = applicantEdus.Where(e => e.InstituteId == tr.FromInstituteId);

                    if (!educations.Any())
                        throw new NotFoundException($"Applicant does not have education for institute {tr.FromInstituteId}");

                    // pick the education with the latest grad date (or start date if currently attending)
                    var education = educations.OrderByDescending(e => e.AttendedTo is null ? 1 : 0).ThenByDescending(e => e.AttendedFrom).First();

                    var instituteType = instituteTypes.FirstOrDefault(t => t.Id == education.InstituteTypeId)
                        ?? throw new ValidationException("Education institute type not found.");

                    var transmission = transmissions.FirstOrDefault(t => t.Id == tr.TransmissionId)
                                        ?? throw new ValidationException($"Transmission {tr.TransmissionId} not found.");

                    if (transmission.Code != Constants.TranscriptTransmissions.SendTranscriptNow && transmission.InstituteTypeId != education.InstituteTypeId)
                        throw new ValidationException("TransmissionId must be match the Institute type of the transcript request.");

                    var requestStatusId = requestStatuses.FirstOrDefault(s => s.Code == Apply.Constants.TranscriptRequestStatuses.WaitingPayment)?.Id
                        ?? throw new ValidationException($"Request Status {Apply.Constants.TranscriptRequestStatuses.WaitingPayment} not found.");

                    var toInstituteCode = string.Empty;
                    if (tr.ToInstituteId.HasValue)
                    {
                        var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
                        var toCollege = colleges.FirstOrDefault(c => c.Id == tr.ToInstituteId)
                            ?? throw new ValidationException("ToInstituteId must be an exisiting college.");
                        toInstituteCode = toCollege.Code;
                    }

                    (var fromInstituteCode, var transcriptFee) = await ValidateInstitute(tr, instituteType.Code, transmission.Code, applicant, application.Id);

                    Dto.TranscriptRequestLog transcriptRequestLog = null;
                    var applicationStatus = applicationStatuses.First(s => s.Id == application.ApplicationStatusId);
                    if (applicationStatus.Code == Constants.ApplicationStatuses.Active && (transcriptFee == null || transcriptFee == 0))
                    {
                        transcriptRequestLog = await CreateTranscriptRequestLog(application.ApplicationNumber, fromInstituteCode);
                        requestStatusId = requestStatuses.FirstOrDefault(s => s.Code == Constants.TranscriptRequestStatuses.RequestInit)?.Id
                            ?? throw new ValidationException($"Request Status {Constants.TranscriptRequestStatuses.RequestInit} not found.");
                    }

                    dtoTranscriptRequest = await CreateTranscriptRequest(tr, request.User, application, education.Id, instituteType.Code, toInstituteCode, fromInstituteCode, requestStatusId, transcriptRequestLog);

                    if (transcriptRequestLog != null) await ProcessTranscriptRequestLog(transcriptRequestLog);

                    transcriptRequests.Add(_apiMapper.MapTranscriptRequest(dtoTranscriptRequest, instituteTypes, transmissions));
                }

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }

            return transcriptRequests;
        }

        private async Task<(string fromInstituteCode, decimal? transcriptFee)> ValidateInstitute(TranscriptRequestBase transcriptRequestBase, string instituteTypeCode, string transmissionCode, Dto.Contact applicant, Guid applicationId)
        {
            switch (instituteTypeCode)
            {
                case Constants.InstituteTypes.HighSchool:
                    {
                        var highSchool = await ValidateHighSchoolTranscriptRequest(transcriptRequestBase.ToInstituteId, transcriptRequestBase.FromInstituteId, transmissionCode, applicant);
                        return (highSchool.Code, highSchool.TranscriptFee);
                    }

                case Constants.InstituteTypes.College:
                    {
                        var college = await ValidateCollegeTranscriptRequest(transcriptRequestBase.ToInstituteId, transcriptRequestBase.FromInstituteId, applicationId);
                        return (college.Code, college.TranscriptFee);
                    }

                case Constants.InstituteTypes.University:
                    {
                        var university = await ValidateUniversityTranscriptRequest(transcriptRequestBase.ToInstituteId, transcriptRequestBase.FromInstituteId, applicationId);
                        return (university.Code, university.TranscriptFee);
                    }

                default:
                    throw new ValidationException($"Cannot create transcript request for {instituteTypeCode}");
            }
        }

        private async Task<College> ValidateCollegeTranscriptRequest(Guid? toInstituteId, Guid fromInstituteId, Guid applicationId)
        {
            if (toInstituteId.IsEmpty())
                throw new ValidationException("ToInstituteId must exist for college transcript request.");

            var applicationChoices = await _domesticContext.GetProgramChoices(new Dto.GetProgramChoicesOptions { ApplicationId = applicationId });
            if (!applicationChoices.Any(e => e.CollegeId == toInstituteId.Value))
                throw new NotFoundException($"Application does not have choice(s) for institute {toInstituteId.Value}");

            var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
            var college = colleges.FirstOrDefault(c => c.Id == fromInstituteId);

            if (!college.HasEtms)
                throw new ValidationException("College must support ETMS transcript request.");

            return college;
        }

        private async Task<HighSchool> ValidateHighSchoolTranscriptRequest(Guid? toInstituteId, Guid fromInstituteId, string transmissionCode, Dto.Contact applicant)
        {
            if (!toInstituteId.IsEmpty())
                throw new ValidationException("ToInstituteId must be empty for high school transcript request.");

            if (transmissionCode != Constants.TranscriptTransmissions.SendTranscriptNow)
                throw new ValidationException("TransmissionId must be 'Send Now' for high school transcript request.");

            var highSchools = await _lookupsCache.GetHighSchools(Constants.Localization.EnglishCanada);
            var highSchool = highSchools.FirstOrDefault(h => h.Id == fromInstituteId);

            if (highSchool is null)
            {
                var highSchoolEx = $"HighSchool not found in cache: {fromInstituteId}";
                _logger.LogCritical(highSchoolEx);
                throw new NotFoundException(highSchoolEx);
            }

            if (!highSchool.HasEtms)
                throw new ValidationException("High School must support ETMS transcript request.");

            if (applicant.HighSchoolEnrolled == true)
                throw new ValidationException("Cannot request transcripts from ETMS High School when applicant is currently attending");

            return highSchool;
        }

        private async Task<University> ValidateUniversityTranscriptRequest(Guid? toInstituteId, Guid fromInstituteId, Guid applicationId)
        {
            if (toInstituteId.IsEmpty())
                throw new ValidationException("ToInstituteId must exist for university transcript request.");

            var applicationChoices = await _domesticContext.GetProgramChoices(new Dto.GetProgramChoicesOptions { ApplicationId = applicationId });
            if (!applicationChoices.Any(e => e.CollegeId == toInstituteId.Value))
                throw new NotFoundException($"Application does not have choice(s) for institute {toInstituteId.Value}");

            var universities = await _lookupsCache.GetUniversities();
            var university = universities.FirstOrDefault(c => c.Id == fromInstituteId);

            if (!university.HasEtms)
                throw new ValidationException("University must support ETMS transcript request.");

            return university;
        }

        private Task<Dto.TranscriptRequestLog> CreateTranscriptRequestLog(string applicationNumber, string fromInstituteCode)
        {
            var dtoRequestLog = new Dto.TranscriptRequestLogBase
            {
                Name = $"{applicationNumber}-{fromInstituteCode}",
                ProcessStatus = ProcessStatus.NotProcessed
            };

            return _domesticContext.CreateTranscriptRequestLog(dtoRequestLog);
        }

        private Task<Dto.TranscriptRequest> CreateTranscriptRequest(TranscriptRequestBase transcriptRequestBase, IPrincipal user, Dto.Application application, Guid educationId, string instituteTypeCode, string toInstituteCode, string fromInstituteCode, Guid requestStatusId, Dto.TranscriptRequestLog transcriptRequestLog)
        {
            var dtoTranscriptRequestBase = new Dto.TranscriptRequestBase
            {
                ApplicantId = application.ApplicantId,
                ApplicationId = application.Id,
                EducationId = educationId,
                FromSchoolId = transcriptRequestBase.FromInstituteId,
                FromSchoolType = instituteTypeCode == Constants.InstituteTypes.College ? TranscriptSchoolType.College
                : instituteTypeCode == Constants.InstituteTypes.University ? TranscriptSchoolType.University
                : instituteTypeCode == Constants.InstituteTypes.HighSchool ? TranscriptSchoolType.HighSchool
                : (TranscriptSchoolType?)null,
                ModifiedBy = user.GetUpnOrEmail(),
                Name = !string.IsNullOrEmpty(toInstituteCode) ? $"{application.ApplicationNumber}-{fromInstituteCode}-{toInstituteCode}"
                : $"{application.ApplicationNumber}-{fromInstituteCode}",
                ToSchoolId = transcriptRequestBase.ToInstituteId,
                TranscriptRequestStatusId = requestStatusId,
                TranscriptTransmissionId = transcriptRequestBase.TransmissionId
            };

            if (transcriptRequestLog != null)
                dtoTranscriptRequestBase.PeteRequestLogId = transcriptRequestLog.Id;

            return _domesticContext.CreateTranscriptRequest(dtoTranscriptRequestBase);
        }

        private async Task ProcessTranscriptRequestLog(Dto.TranscriptRequestLog transcriptRequestLog)
        {
            transcriptRequestLog.ProcessStatus = ProcessStatus.Processed;
            await _domesticContext.UpdateTranscriptRequestLog(transcriptRequestLog);
        }
    }
}
