using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Services.Handlers
{
    public class GetCampusesHandler : IRequestHandler<GetCampuses, IList<Campus>>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;

        public GetCampusesHandler(
            ILogger<GetCampusesHandler> logger,
            ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<IList<Campus>> Handle(GetCampuses request, CancellationToken cancellationToken)
        {
            var campuses = await _lookupsCache.GetCampuses();

            return campuses.Where(x => x.CollegeId == request.CollegeId).ToList();
        }
    }
}