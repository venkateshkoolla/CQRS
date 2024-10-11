using Microsoft.AspNetCore.Builder;
using Ocas.Domestic.Apply.Admin.Api.Middlewares;

namespace Ocas.Domestic.Apply.Admin.Api
{
    public static class OcasEnrichSerilogHttpContextExtensions
    {
        public static IApplicationBuilder UseOcasEnrichSerilogHttpContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OcasEnrichSerilogHttpContextMiddleware>();
        }
    }
}
