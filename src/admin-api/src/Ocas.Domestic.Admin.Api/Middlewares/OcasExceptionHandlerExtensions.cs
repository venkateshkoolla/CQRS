using Microsoft.AspNetCore.Builder;
using Ocas.Domestic.Apply.Admin.Api.Middlewares;

namespace Ocas.Domestic.Apply.Admin.Api
{
    public static class OcasExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseOcasExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<OcasExceptionHandlerMiddleware>();
        }
    }
}
