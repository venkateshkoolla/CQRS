using System;
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
    public class GetLatestPrivacyStatementHandler : IRequestHandler<GetLatestPrivacyStatement, PrivacyStatement>
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;
        private readonly ILogger _logger;

        public GetLatestPrivacyStatementHandler(
            ILogger<GetLatestPrivacyStatement> logger,
            ILookupsCache lookupsCache,
            RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public Task<PrivacyStatement> Handle(GetLatestPrivacyStatement request, CancellationToken cancellationToken)
        {
            return _lookupsCache.GetLatestPrivacyStatement(_locale);
        }
    }
}
