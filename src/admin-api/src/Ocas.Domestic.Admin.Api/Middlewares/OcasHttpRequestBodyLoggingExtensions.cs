using Microsoft.AspNetCore.Builder;
using Ocas.Domestic.Apply.Admin.Api.Middlewares;

namespace Ocas.Domestic.Apply.Admin.Api
{
    public static class OcasHttpRequestBodyLoggingExtensions
    {
        public static IApplicationBuilder UseOcasHttpRequestBodyLogging(this IApplicationBuilder builder, OcasHttpRequestBodyLoggingOptions options = null)
        {
            return builder.UseMiddleware<OcasHttpRequestBodyLoggingMiddleware>(options);
        }
    }
}
