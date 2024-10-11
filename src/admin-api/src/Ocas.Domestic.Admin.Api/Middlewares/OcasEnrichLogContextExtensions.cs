using Microsoft.AspNetCore.Builder;
using Ocas.Domestic.Apply.Admin.Api.Middlewares;

namespace Ocas.Domestic.Apply.Admin.Api
{
    public static class OcasEnrichLogContextExtensions
    {
        public static IApplicationBuilder UseOcasEnrichLogContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OcasEnrichLogContextMiddleware>();
        }
    }
}
