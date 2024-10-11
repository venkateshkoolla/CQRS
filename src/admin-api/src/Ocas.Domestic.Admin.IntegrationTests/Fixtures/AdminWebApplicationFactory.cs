using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Ocas.Domestic.Apply.Admin.Api;
using Serilog;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class AdminWebApplicationFactory : WebApplicationFactory<Startup>
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
