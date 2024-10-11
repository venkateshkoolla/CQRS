using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply
{
    public interface ITranslationsCache
    {
        Task<string> GetTranslationValue(string locale, string key, string project = Constants.Translations.FallbackProject);
        Task<TranslationsDictionary> GetTranslations(string locale, string project = Constants.Translations.FallbackProject);
    }
}
