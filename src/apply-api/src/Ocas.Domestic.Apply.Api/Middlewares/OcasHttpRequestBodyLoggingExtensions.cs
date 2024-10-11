using Microsoft.AspNetCore.Builder;
using Ocas.Domestic.Apply.Api.Middlewares;

namespace Ocas.Domestic.Apply.Api
{
    public static class OcasHttpRequestBodyLoggingExtensions
    {
        public static IApplicationBuilder UseOcasHttpRequestBodyLogging(this IApplicationBuilder builder, OcasHttpRequestBodyLoggingOptions options = null)
        {
            return builder.UseMiddleware<OcasHttpRequestBodyLoggingMiddleware>(options);
        }
    }
}
