using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class UpsertAcademicRecordHandler : IRequestHandler<UpsertAcademicRecord, UpsertAcademicRecordResult>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly ILookupsCache _lookupsCache;
        private readonly IDtoMapper _dtoMapper;
        private readonly IApiMapper _apiMapper;
        private readonly string _locale;

        public UpsertAcademicRecordHandler(ILogger<UpsertAcademicRecordHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, ILookupsCache lookupsCache, IDtoMapper dtoMapper, IApiMapper apiMapper, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _dtoMapper = dtoMapper ?? throw new ArgumentNullException(nameof(dtoMapper));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<UpsertAcademicRecordResult> Handle(UpsertAcademicRecord request, CancellationToken cancellationToken)
        {
            if (!_userAuthorization.IsOcasTier2User(request.User) && !_userAuthorization.IsHighSchoolUser(request.User))
            {
                throw new ForbiddenException();
            }

            var academicRecords = await _domesticContext.GetAcademicRecords(request.ApplicantId);
            var academicRecord = academicRecords.OrderByDescending(a => a.ModifiedOn).FirstOrDefault();

            if (academicRecord != null)
            {
                if (academicRecords.Count() > 1)
                    _logger.LogInformation($"Applicant {request.ApplicantId} has more than one academic record. Latest record of {academicRecord.Id} going to be updated.");

                return await UpdateAcademicRecord(request, academicRecord);
            }

            return await CreateAcademicRecord(request);
        }

        private async Task<UpsertAcademicRecordResult> CreateAcademicRecord(UpsertAcademicRecord request)
        {
            var applicant = await _domesticContext.GetContact(request.ApplicantId);

            if (request.AcademicRecord.SchoolId.IsEmpty()) throw new ValidationException("'School Id' must not be empty when creating academic record");
            var school = (await _lookupsCache.GetHighSchools(_locale)).First(s => s.Id == request.AcademicRecord.SchoolId);

            var academicRecordBase = new Dto.AcademicRecordBase
            {
                ApplicantId = applicant.Id,
                Name = $"{applicant.AccountNumber}-{school.Mident}",
                Mident = school.Mident,
                ModifiedBy = request.User.GetUpnOrEmail()
            };
            _dtoMapper.PatchAcademicRecordBase(academicRecordBase, request.AcademicRecord);

            Dto.AcademicRecord newAcademicRecord;
            await _domesticContext.BeginTransaction();
            try
            {
                var dtoTranscript = new Dto.TranscriptBase
                {
                    ModifiedBy = request.User.GetUpnOrEmail(),
                    TranscriptType = TranscriptType.OntarioHighSchoolTranscript,
                    ContactId = applicant.Id,
                    PartnerId = request.AcademicRecord.SchoolId
                };
                await _domesticContext.CreateTranscript(dtoTranscript);

                newAcademicRecord = await _domesticContext.CreateAcademicRecord(academicRecordBase);
                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }

            return new UpsertAcademicRecordResult
            {
                AcademicRecord = _apiMapper.MapAcademicRecord(newAcademicRecord, new List<HighSchool> { school }),
                SupportingDocument = _apiMapper.MapSupportingDocument(newAcademicRecord)
            };
        }

        private async Task<UpsertAcademicRecordResult> UpdateAcademicRecord(UpsertAcademicRecord request, Dto.AcademicRecord academicRecord)
        {
            if (!request.AcademicRecord.SchoolId.IsEmpty()) throw new ValidationException("'School Id' must be empty when updating academic record");

            academicRecord.ModifiedBy = request.User.GetUpnOrEmail();
            _dtoMapper.PatchAcademicRecordBase(academicRecord, request.AcademicRecord);
            var result = await _domesticContext.UpdateAcademicRecord(academicRecord);

            var dtoTranscripts = await _domesticContext.GetTranscripts(new Dto.GetTranscriptOptions { ContactId = request.ApplicantId });
            var highSchools = new List<HighSchool>();

            var highSchoolTranscripts = dtoTranscripts.Where(x => x.TranscriptType == TranscriptType.OntarioHighSchoolTranscript).ToList();
            if (highSchoolTranscripts.Any())
                highSchools = (await _lookupsCache.GetHighSchools(_locale)).Where(h => highSchoolTranscripts.Any(t => t.PartnerId == h.Id)).ToList();

            return new UpsertAcademicRecordResult
            {
                AcademicRecord = _apiMapper.MapAcademicRecord(result, highSchools),
                SupportingDocument = null
            };
        }
    }
}
