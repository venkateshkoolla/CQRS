using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CacheManager.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Ocas.Domestic.Apply.Admin.Core.Settings;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services
{
    public class TranslationsCache : ITranslationsCache
    {
        private readonly ICacheManager<object> _cacheManager;
        private readonly IAppSettings _appSettings;
        private readonly ILogger _logger;

        public TranslationsCache(ICacheManager<object> cacheManager, IAppSettings appSettings, ILogger<TranslationsCache> logger)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetTranslationValue(string locale, string key, string project)
        {
            var translations = await GetTranslations(locale, project);
            return translations.Get(key);
        }

        public async Task<TranslationsDictionary> GetTranslations(string locale, string project)
        {
            var translationsKey = $"Translations_{project}_{locale}";
            var translations = _cacheManager.Get<TranslationsDictionary>(translationsKey);

            if (translations != null)
            {
                _logger.LogInformation($"{translationsKey} cache return", translations);
                return translations;
            }

            // Call translations url
            var translationsUrl = new Uri(string.Format(_appSettings.GetAppSetting("ocas:translationsUrl"), project, locale));

            using (var client = new HttpClient())
            using (var response = await client.GetAsync(translationsUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var dic = JsonConvert.DeserializeObject<IDictionary<string, string>>(content);
                    translations = new TranslationsDictionary(dic);
                }
                else
                {
                    throw new Common.Exceptions.UnhandledException($"{response.StatusCode} from ${translationsUrl}");
                }
            }

            var cacheExpiry = _appSettings.GetAppSettingOrDefault<double>("ocas:cacheExpiryMinutes", 1440);
            var item = new CacheItem<object>(translationsKey, translations, ExpirationMode.Absolute, TimeSpan.FromMinutes(cacheExpiry));
            _cacheManager.Put(item);

            _logger.LogInformation("Translations Url return added to cache", item);

            return translations;
        }
    }
}
