using System;
using System.Collections.Generic;
using System.Linq;
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
    public class GetApplicationsHandler : IRequestHandler<GetApplications, IList<Application>>
    {
        private readonly ILogger<GetApplicationsHandler> _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly IDomesticContext _domesticContext;
        private readonly IApiMapper _apiMapper;

        public GetApplicationsHandler(ILogger<GetApplicationsHandler> logger, ILookupsCache lookupsCache, IDomesticContext domesticContext, IApiMapper apiMapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
        }

        public async Task<IList<Application>> Handle(GetApplications request, CancellationToken cancellationToken)
        {
            var applications = await _domesticContext.GetApplications(request.ApplicantId);
            return applications.Select(_apiMapper.MapApplication).ToList();
        }
    }
}
