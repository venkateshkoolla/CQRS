using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Extensions
{
    public static class StringExtensions
    {
        public static Locale ToLocaleEnum(this string locale)
        {
            return locale == Constants.Localization.FrenchCanada
                ? Locale.French
                : Locale.English;
        }
    }
}
