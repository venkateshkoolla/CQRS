using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Messages;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.Handlers
{
    public class GetCollegesHandler : IRequestHandler<GetColleges, IList<College>>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;

        public GetCollegesHandler(
            ILogger<GetCollegesHandler> logger,
            ILookupsCache lookupsCache,
            RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public Task<IList<College>> Handle(GetColleges request, CancellationToken cancellationToken)
        {
            return _lookupsCache.GetColleges(_locale);
        }
    }
}
