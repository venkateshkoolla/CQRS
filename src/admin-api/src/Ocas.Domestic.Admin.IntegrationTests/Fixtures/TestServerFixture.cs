using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Admin.Api.Client;
using Ocas.Domestic.Apply.Admin.Core.Settings;
using Serilog;
using Serilog.Extensions.Logging;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private static readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);
        private static bool _loggerInitialized;
        private AdminWebApplicationFactory _factory;
        private bool _disposedValue;

        public async Task<HttpClient> CreateClient()
        {
            if (_factory != null) return _factory.CreateClient();

            // First time it's called it will initialize the TestServer (slower), but every time after it is fast to just get the Client
            _factory = new AdminWebApplicationFactory();
            var client = _factory.CreateClient();
            var appSettings = _factory.Server.Host.Services.GetService(typeof(IAppSettings)) as IAppSettings;
            await appSettings.InitializeAsync();
            return client;
        }

        public async Task<AdminApiClient> CreateAdminApiClient()
        {
            await _syncLock.WaitAsync();
            try
            {
                if (!_loggerInitialized)
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.Console()
                        .WriteTo.File(".\\Logs\\test-log.txt", rollingInterval: RollingInterval.Day)
                        .CreateLogger();

                    _loggerInitialized = true;
                }

                var http = await CreateClient();
                http.Timeout = TimeSpan.FromMinutes(10);
                http.BaseAddress = new Uri("http://localhost/api/v1/");
                return new AdminApiClient(http, new SerilogLoggerProvider(Log.Logger).CreateLogger(nameof(AdminApiClient)));
            }
            finally
            {
                _syncLock.Release();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                _factory.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
            // TODO: set large fields to null.

            _disposedValue = true;
        }
    }
}
