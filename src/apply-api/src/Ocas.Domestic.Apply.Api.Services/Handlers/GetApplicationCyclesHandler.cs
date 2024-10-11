using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetApplicationCyclesHandler : IRequestHandler<GetApplicationCycles, IList<ApplicationCycle>>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;

        public GetApplicationCyclesHandler(ILogger<GetApplicationCyclesHandler> logger, ILookupsCache lookupsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
        }

        public async Task<IList<ApplicationCycle>> Handle(GetApplicationCycles request, CancellationToken cancellationToken)
        {
            var appCycle = new List<ApplicationCycle>();
            foreach (var applicationCycle in await _lookupsCache.GetApplicationCycles())
            {
                if (applicationCycle.Status != Constants.ApplicationCycleStatuses.Active && applicationCycle.Status != Constants.ApplicationCycleStatuses.Previous) continue;

                appCycle.Add(applicationCycle);
            }

            return appCycle.OrderByDescending(x => x.Year).ToList();
        }
    }
}
