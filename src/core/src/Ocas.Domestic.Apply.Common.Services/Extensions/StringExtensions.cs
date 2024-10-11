using System.IO;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Services.Extensions
{
    public static class StringExtensions
    {
        public static Locale ToLocaleEnum(this string locale)
        {
            return locale == Constants.Localization.FrenchCanada
                ? Locale.French
                : Locale.English;
        }

        public static string SanitizeFileName(this string filename)
        {
            if (string.IsNullOrWhiteSpace(filename))
                return string.Empty;

            var invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (var c in invalid)
            {
                filename = filename.Replace(c.ToString(), string.Empty);
            }

            return filename;
        }
    }
}
