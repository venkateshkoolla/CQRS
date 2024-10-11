using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorrelationId;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ocas.Domestic.Apply.Admin.Api.Middlewares
{
    public class OcasEnrichLogContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public OcasEnrichLogContextMiddleware(RequestDelegate next, ILogger<OcasEnrichLogContextMiddleware> logger, ICorrelationContextAccessor correlationContextAccessor)
        {
            _next = next;
            _logger = logger;
            _correlationContextAccessor = correlationContextAccessor;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["CorrelationId"] = _correlationContextAccessor.CorrelationContext.CorrelationId,
                ["IdentityName"] = httpContext?.User?.Identity?.Name,
                ["SubjectClaim"] = httpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "sub")?.Value,
                ["CustomerCode"] = httpContext?.User?.Claims?.FirstOrDefault(x => x.Type == "customer_code")?.Value
            }))
            {
                await _next(httpContext);
            }
        }
    }
}
