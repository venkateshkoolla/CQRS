using Microsoft.AspNetCore.Builder;
using Ocas.Domestic.Apply.Api.Middlewares;

namespace Ocas.Domestic.Apply.Api
{
    public static class OcasEnrichSerilogHttpContextExtensions
    {
        public static IApplicationBuilder UseOcasEnrichSerilogHttpContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OcasEnrichSerilogHttpContextMiddleware>();
        }
    }
}
