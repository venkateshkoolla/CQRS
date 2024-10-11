using System.IO;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Api.Services.Extensions
{
    public static class StringExtensions
    {
        public static Locale ToLocaleEnum(this string locale)
        {
            return locale == Constants.Localization.FrenchCanada
                ? Locale.French
                : Locale.English;
        }

        public static Core.Enums.PreferredLanguage ToPreferredLanguageEnum(this string locale)
        {
            return locale == Constants.Localization.FrenchCanada
                ? Core.Enums.PreferredLanguage.French
                : Core.Enums.PreferredLanguage.English;
        }
    }
}
