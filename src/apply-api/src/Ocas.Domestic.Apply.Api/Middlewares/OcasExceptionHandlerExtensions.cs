using Microsoft.AspNetCore.Builder;
using Ocas.Domestic.Apply.Api.Middlewares;

namespace Ocas.Domestic.Apply.Api
{
    public static class OcasExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseOcasExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OcasExceptionHandlerMiddleware>();
        }
    }
}
