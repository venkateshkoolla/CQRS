using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetMcuCodeHandler : IRequestHandler<GetMcuCode, McuCode>
    {
        private readonly ILogger _logger;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;

        public GetMcuCodeHandler(ILogger<GetMcuCodeHandler> logger, ILookupsCache lookupsCache, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<McuCode> Handle(GetMcuCode request, CancellationToken cancellationToken)
        {
            var mcuCodes = await _lookupsCache.GetMcuCodes(_locale);

            return mcuCodes.FirstOrDefault(c => c.Code == request.McuCode)
                ?? throw new Common.Exceptions.NotFoundException($"McuCode {request.McuCode} not found");
        }
    }
}
