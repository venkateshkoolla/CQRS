using Microsoft.AspNetCore.Builder;
using Ocas.Domestic.Apply.Api.Middlewares;

namespace Ocas.Domestic.Apply.Api
{
    public static class OcasEnrichLogContextExtensions
    {
        public static IApplicationBuilder UseOcasEnrichLogContext(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OcasEnrichLogContextMiddleware>();
        }
    }
}
