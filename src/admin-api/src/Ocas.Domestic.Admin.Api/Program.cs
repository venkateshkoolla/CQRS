using System;
using System.IO;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ocas.Domestic.Apply.Admin.Api.Configuration;
using Ocas.Domestic.Apply.Admin.Core.Settings;
using Serilog;

namespace Ocas.Domestic.Apply.Admin.Api
{
    public static class Program
    {
        public static Lazy<IConfiguration> Configuration { get; } = new Lazy<IConfiguration>(() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json", optional: true)
            .AddEnvironmentVariables()
            .Build());

        public static async Task<int> Main(string[] args)
        {
            SerilogBootstrapper.Initialize(Configuration.Value);

            try
            {
                Log.Information("Building web host");
                var host = CreateWebHostBuilder(args).Build();

                Log.Information("Fetching AppSettings");
                var appSettings = host.Services.GetService(typeof(IAppSettings)) as IAppSettings;
                await appSettings.InitializeAsync();

                Log.Information("Starting web host");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) => config.AddJsonFile("cachesettings.json", optional: false, reloadOnChange: true))
                .ConfigureServices(x => x.AddAutofac())
                .UseStartup<Startup>()
                .UseSerilog();
    }
}
