using System;
using System.Collections.Generic;
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
    public class GetCitiesHandler : IRequestHandler<GetCities, IList<City>>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;

        public GetCitiesHandler(
            ILogger<GetCitiesHandler> logger,
            ILookupsCache lookupsCache,
            RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<IList<City>> Handle(GetCities request, CancellationToken cancellationToken)
        {
            var allCities = await _lookupsCache.GetCities(_locale);
            return !request.ProvinceId.HasValue ? allCities : allCities.Where(x => x.ProvinceId == request.ProvinceId.Value).ToList();
        }
    }
}
