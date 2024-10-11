using Microsoft.Extensions.Configuration;
using Serilog;

namespace Ocas.Domestic.Apply.Admin.Api.Configuration
{
    public static class SerilogBootstrapper
    {
        public static void Initialize(IConfiguration configuration)
        {
            var logConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithProperty("Environment", configuration["ocas:environment"])
                .Enrich.WithProperty("ApplicationName", configuration["ocas:applicationName"]);

            Log.Logger = logConfig.CreateLogger();
        }
    }
}
