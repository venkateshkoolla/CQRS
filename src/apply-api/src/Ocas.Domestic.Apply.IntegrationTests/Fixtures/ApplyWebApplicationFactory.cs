using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Ocas.Domestic.Apply.Api;
using Serilog;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class ApplyWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
#if DEBUG
                .UseEnvironment("Development")
#else
                .UseEnvironment("Tests")
#endif
                .UseSerilog();

            base.ConfigureWebHost(builder);
        }
    }
}
