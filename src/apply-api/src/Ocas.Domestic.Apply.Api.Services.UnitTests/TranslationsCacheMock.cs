using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests
{
    public class TranslationsCacheMock : ITranslationsCache
    {
        private readonly TranslationsDictionary _translationsDictionary = new TranslationsDictionary(new Dictionary<string, string>());

        public Task<string> GetTranslationValue(string locale, string key, string project)
        {
            return Task.FromResult(key);
        }

        public Task<TranslationsDictionary> GetTranslations(string locale, string project)
        {
            return Task.FromResult(_translationsDictionary);
        }
    }
}
