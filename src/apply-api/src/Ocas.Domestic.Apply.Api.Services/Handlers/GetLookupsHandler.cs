using System;
using System.Globalization;
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
    public class GetLookupsHandler : IRequestHandler<GetLookups, AllLookups>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;

        public GetLookupsHandler(ILogger<GetLookupsHandler> logger, ILookupsCache lookupsCache, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<AllLookups> Handle(GetLookups request, CancellationToken cancellationToken)
        {
            var keys = request.Filter?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var result = await _lookupsCache.GetAllLookups(_locale, keys);

            if (result.AboriginalStatuses != null)
            {
                result.AboriginalStatuses = result.AboriginalStatuses.Where(x => x.ShowInPortal).ToList();
            }

            if (result.ApplicationCycles != null)
            {
                result.ApplicationCycles = result.ApplicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active).ToList();
            }

            if (result.Titles != null)
            {
                result.Titles = result.Titles.Where(x => x.Code != Constants.Titles.Unknown).ToList();
            }

            if (result.TranscriptTransmissions != null)
            {
                var applicationCycles = await _lookupsCache.GetApplicationCycles();
                var activeApplicationCycles = applicationCycles.Where(a => a.Status == Constants.ApplicationCycleStatuses.Active);

                result.TranscriptTransmissions = result.TranscriptTransmissions
                        .Where(t => t.ApplicationCycleId == null || activeApplicationCycles.Any(a => a.Id == t.ApplicationCycleId.Value))
                        .OrderBy(t => t.EligibleUntil).ToList();
            }

            return result;
        }
    }
}
