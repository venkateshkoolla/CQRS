using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Ocas.AppSettings.Client;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Core.Settings;

namespace Ocas.Domestic.Apply.Admin.Api
{
    public class AppSettings : IAppSettings, AppSettingsApiConfig
    {
        private const string AppSettingsCacheKey = "AppSettings";

        private static readonly ICacheManager<IDictionary<string, JToken>> _cache =
            new BaseCacheManager<IDictionary<string, JToken>>(
                CacheManager.Core.ConfigurationBuilder.BuildConfiguration(settings =>
                    settings.WithSystemRuntimeCacheHandle()));

        private readonly AppSettingsClient _appSettingsClient;

        public async Task InitializeAsync()
        {
            var appSettings = await _appSettingsClient.GetAppSettings();

            _cache.AddOrUpdate(AppSettingsCacheKey, appSettings, _ => appSettings);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "ArgumentNullException states name of key missing")]
        public AppSettings(IConfiguration configuration)
        {
            // All App Settings Stored Locally within the appsettings.*.json files
            ApplicationName = configuration["ocas:appSettingsName"] ?? throw new ArgumentNullException("ocas:appSettingsName");
            Environment = configuration["ocas:environment"] ?? throw new ArgumentNullException("ocas:environment");
            AppSettingsBaseUrl = configuration["ocas:appSettingsBaseUrl"] ?? throw new ArgumentNullException("ocas:appSettingsBaseUrl");
            IdSvrAuthority = configuration["ocas:idsvr:authority"] ?? throw new ArgumentNullException("ocas:idsvr:authority");
            IdSvrAppSettingsClientId = configuration["ocas:idsvr:appSettings:clientId"] ?? throw new ArgumentNullException("ocas:idsvr:appSettings:clientId");
            IdSvrAppSettingsClientSecret = configuration["ocas:idsvr:appSettings:clientSecret"] ?? throw new ArgumentNullException("ocas:idsvr:appSettings:clientSecret");
            IdSvrAppSettingsScope = configuration["ocas:idsvr:appSettings:scope"] ?? throw new ArgumentNullException("ocas:idsvr:appSettings:scope");

            IdSvrRolesOcasUser = configuration.GetSection("ocas:idsvr:roles:ocas").GetChildren().Select(c => c.Value).ToList();
            if (IdSvrRolesOcasUser.Count < 1) throw new ArgumentNullException("ocas:idsvr:roles:ocas");

            IdSvrRolesPartnerCollegeUser = configuration.GetSection("ocas:idsvr:roles:partnerCollegeUser").GetChildren().Select(c => c.Value).ToList();
            if (IdSvrRolesPartnerCollegeUser.Count < 1) throw new ArgumentNullException("ocas:idsvr:roles:partnerCollegeUser");

            IdSvrRolesPartnerHighSchoolUser = configuration.GetSection("ocas:idsvr:roles:partnerHighSchoolUser").GetChildren().Select(c => c.Value).ToList();
            if (IdSvrRolesPartnerHighSchoolUser.Count < 1) throw new ArgumentNullException("ocas:idsvr:roles:partnerHighSchoolUser");

            IdSvrRolesPartnerHSBoardUser = configuration.GetSection("ocas:idsvr:roles:partnerHSBoardUser").GetChildren().Select(c => c.Value).ToList();
            if (IdSvrRolesPartnerHSBoardUser.Count < 1) throw new ArgumentNullException("ocas:idsvr:roles:partnerHSBoardUser");

            // For all remote appsettings
            _appSettingsClient = new AppSettingsClient(this);
        }

        public string ApplicationName { get; }
        public string Environment { get; }
        public string AppSettingsBaseUrl { get; }
        public string IdSvrAuthority { get; }
        public string IdSvrAppSettingsClientId { get; }
        public string IdSvrAppSettingsClientSecret { get; }
        public string IdSvrAppSettingsScope { get; }
        public IList<string> IdSvrRolesPartnerCollegeUser { get; }
        public IList<string> IdSvrRolesOcasUser { get; }
        public IList<string> IdSvrRolesPartnerHighSchoolUser { get; }
        public IList<string> IdSvrRolesPartnerHSBoardUser { get; }

        public string GetAppSetting(string key)
        {
            return GetAppSetting<string>(key);
        }

        public T GetAppSetting<T>(string key)
        {
            var appSettings = _cache.Get(AppSettingsCacheKey);

            return appSettings.TryGetValue(key, out var value) ? value.ToObject<T>() : throw new NotFoundException("E0031", $"Could not find app settings key '{key}'");
        }

        public string GetAppSettingOrDefault(string key, string defaultValue)
        {
            return GetAppSettingOrDefault<string>(key, defaultValue);
        }

        public T GetAppSettingOrDefault<T>(string key, T defaultValue)
        {
            var appSettings = _cache.Get(AppSettingsCacheKey);

            return appSettings.TryGetValue(key, out var value) ? value.ToObject<T>() : defaultValue;
        }
    }
}
