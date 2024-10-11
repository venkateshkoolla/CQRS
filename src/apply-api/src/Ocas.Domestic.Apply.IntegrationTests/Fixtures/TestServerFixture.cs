using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Api.Client;
using Ocas.Domestic.Apply.Core.Settings;
using Serilog;
using Serilog.Extensions.Logging;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class TestServerFixture : IDisposable
    {
        private static readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);
        private static bool _loggerInitialized;
        private ApplyWebApplicationFactory _factory;
        private bool _disposedValue;

        public async Task<HttpClient> CreateClient()
        {
            if (_factory != null) return _factory.CreateClient();

            // First time it's called it will initialize the TestServer (slower), but every time after it is fast to just get the Client
            _factory = new ApplyWebApplicationFactory();
            var client = _factory.CreateClient();
            var appSettings = _factory.Server.Host.Services.GetService(typeof(IAppSettings)) as IAppSettings;
            await appSettings.InitializeAsync();
            return client;
        }

        public async Task<ApplyApiClient> CreateApplyApiClient()
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
                return new ApplyApiClient(http, new SerilogLoggerProvider(Log.Logger).CreateLogger(nameof(ApplyApiClient)));
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
