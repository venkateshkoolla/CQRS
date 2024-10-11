using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetTranscriptRequestsHandler : IRequestHandler<GetTranscriptRequests, IList<TranscriptRequest>>
    {
        private readonly ILogger<GetTranscriptRequestsHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;

        public GetTranscriptRequestsHandler(ILogger<GetTranscriptRequestsHandler> logger, IDomesticContext domesticContext, IUserAuthorization userAuthorization, IApiMapper apiMapper, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache;
        }

        public async Task<IList<TranscriptRequest>> Handle(GetTranscriptRequests request, CancellationToken cancellationToken)
        {
            var getTranscriptRequestOptions = new Dto.GetTranscriptRequestOptions();
            if (request.ApplicationId.HasValue)
            {
                var application = await _domesticContext.GetApplication(request.ApplicationId.Value)
                                  ?? throw new NotFoundException("Application not found");
                await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId);

                getTranscriptRequestOptions.ApplicationId = request.ApplicationId.Value;
            }
            else
            {
                await _userAuthorization.CanAccessApplicantAsync(request.User, request.ApplicantId.Value);

                getTranscriptRequestOptions.ApplicantId = request.ApplicantId.Value;
            }

            var dtoTranscriptRequests = await _domesticContext.GetTranscriptRequests(getTranscriptRequestOptions);
            var transcriptRequests = new List<TranscriptRequest>();
            foreach (var dtoTranscriptRequest in dtoTranscriptRequests)
            {
                transcriptRequests.Add(_apiMapper.MapTranscriptRequest(
                    dtoTranscriptRequest,
                    await _lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetTranscriptTransmissions(Constants.Localization.EnglishCanada)));
            }

            return transcriptRequests;
        }
    }
}
