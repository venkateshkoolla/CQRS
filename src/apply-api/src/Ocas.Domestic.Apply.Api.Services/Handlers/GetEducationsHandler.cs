using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Api.Services.Mappers;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetEducationsHandler : IRequestHandler<GetEducations, List<Education>>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;
        private readonly ILookupsCache _lookupsCache;

        public GetEducationsHandler(ILogger<GetEducationsHandler> logger, IDomesticContext domesticContext, IApiMapper apiMapper, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<List<Education>> Handle(GetEducations request, CancellationToken cancellationToken)
        {
            var dtoEducations = await _domesticContext.GetEducations(request.ApplicantId);

            var educations = new List<Education>();
            foreach (var education in dtoEducations)
            {
               educations.Add(await _apiMapper.MapEducation(
                    education,
                    await _lookupsCache.GetInstituteTypes(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetHighSchools(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetUniversities(),
                    _domesticContext));
            }

            return educations;
        }
    }
}
