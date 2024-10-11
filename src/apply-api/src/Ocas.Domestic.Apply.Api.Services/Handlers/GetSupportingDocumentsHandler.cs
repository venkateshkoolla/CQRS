using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Api.Services.Caches;
using Ocas.Domestic.Apply.Api.Services.Extensions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetSupportingDocumentsHandler : IRequestHandler<GetSupportingDocuments, IList<SupportingDocument>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;

        public GetSupportingDocumentsHandler(ILogger<GetSupportingDocumentsHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, ILookupsCache lookupsCache, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<IList<SupportingDocument>> Handle(GetSupportingDocuments request, CancellationToken cancellationToken)
        {
            var dtoSupportingDocs = await _domesticContext.GetSupportingDocuments(request.ApplicantId);
            var dtoTranscripts = await _domesticContext.GetTranscripts(new Dto.GetTranscriptOptions { ContactId = request.ApplicantId });
            var dtoStandardizedTests = await _domesticContext.GetTests(new Dto.GetTestOptions { ApplicantId = request.ApplicantId, IsOfficial = true }, _locale.ToLocaleEnum());
            var dtoAcademicRecords = await _domesticContext.GetAcademicRecords(request.ApplicantId);

            IList<SupportingDocument> supportingDocs = new List<SupportingDocument>();
            if (dtoSupportingDocs.Any())
            {
                var documentTypes = await _lookupsCache.GetSupportingDocumentTypes(_locale);
                var officials = await _lookupsCache.GetOfficials(_locale);
                var institutes = dtoSupportingDocs.Any(d => d.InstituteId.HasValue) ? await _lookupsCache.GetInstitutes() : new List<LookupItem>() as IList<LookupItem>;
                supportingDocs = _apiMapper.MapSupportingDocuments(dtoSupportingDocs, documentTypes, officials, institutes);
            }

            IList<SupportingDocument> transcripts = new List<SupportingDocument>();
            if (dtoTranscripts.Any())
            {
                var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
                var universities = await _lookupsCache.GetUniversities();
                var filteredTranscripts = dtoTranscripts.Where(x => x.TranscriptType != TranscriptType.OntarioHighSchoolTranscript).ToList();
                transcripts = _apiMapper.MapSupportingDocuments(filteredTranscripts, colleges, universities);
            }

            IList<SupportingDocument> standardizedTests = new List<SupportingDocument>();
            if (dtoStandardizedTests.Any())
            {
                standardizedTests = _apiMapper.MapSupportingDocuments(dtoStandardizedTests, await _lookupsCache.GetStandardizedTestTypes(_locale));
            }

            IList<SupportingDocument> academicRecords = new List<SupportingDocument>();
            if (dtoAcademicRecords.Any())
            {
                academicRecords = _apiMapper.MapSupportingDocuments(dtoAcademicRecords);
            }

            return supportingDocs
                .Concat(transcripts)
                .Concat(standardizedTests)
                .Concat(academicRecords)
                .OrderBy(d => d.ReceivedDate)
                .ThenBy(d => d.Name)
                .ToList();
        }
    }
}
