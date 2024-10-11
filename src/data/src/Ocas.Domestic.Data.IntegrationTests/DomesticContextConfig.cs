using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Ocas.AppSettings.Client;
using Ocas.Domestic.Data.TestFramework;
using Xunit;

namespace Ocas.Domestic.Data.IntegrationTests
{
    internal class DomesticContextConfig : IDomesticContextConfig, IAsyncLifetime
    {
        private static readonly SemaphoreSlim _appSettingsLock = new SemaphoreSlim(1, 1);
        private static IDictionary<string, JToken> _appSettings;

        public string CrmConnectionString { get; set;  }
        public string CrmWcfServiceUrl { get; set; }
        public string CrmExtrasConnectionString { get; set; }
        public int CommandTimeout { get; set; }

        public async Task InitializeAsync()
        {
            if (_appSettings is null)
            {
                await _appSettingsLock.WaitAsync();
                try
                {
                    if (_appSettings is null)
                        _appSettings = await new AppSettingsClient(new TestAppSettingsApiConfig()).GetAppSettings();
                }
                finally
                {
                    _appSettingsLock.Release();
                }
            }

            CrmConnectionString = _appSettings["ocas:crm:service:connection"].ToObject<string>();
            CrmWcfServiceUrl = _appSettings["ocas:crm:service:url"].ToObject<string>();
            CrmExtrasConnectionString = _appSettings["ocas:crm:extras"].ToObject<string>();
            CommandTimeout = TestConstants.Config.CommandTimeout;
        }

        Task IAsyncLifetime.DisposeAsync() => Task.CompletedTask;

        private class TestAppSettingsApiConfig : AppSettingsApiConfig
        {
            public string ApplicationName { get; } = "domesticapply-api";
            public string AppSettingsBaseUrl { get; } = "https://appsettingsapi-ci.dev.ocas.ca";
            public string IdSvrAppSettingsClientId { get; } = "asapi.clientcredentials";
            public string IdSvrAppSettingsClientSecret { get; } = "secret1";
            public string IdSvrAppSettingsScope { get; } = "as_api";
            public string IdSvrAuthority { get; } = "https://identity-ci.dev.ocas.ca/core";
#if DEBUG
            public string Environment { get; } = "dev";
#else
            public string Environment { get; } = "test";
#endif
        }
    }
}
