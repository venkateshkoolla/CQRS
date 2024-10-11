using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Models
{
    public class TranslationsDictionary
    {
        private readonly IDictionary<string, string> _translations;

        public TranslationsDictionary(IDictionary<string, string> translations)
        {
            _translations = translations;
        }

        public string Get(string key)
        {
            _translations.TryGetValue(key, out var localizedValue);
            return localizedValue ?? key;
        }
    }
}
