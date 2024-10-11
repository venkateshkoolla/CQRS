using System.Collections.Generic;
using System.Globalization;

namespace Ocas.Domestic.Apply.Admin.Api.Client
{
    internal static class AdminApiConstants
    {
        public static class Localization
        {
            public static readonly CultureInfo EnglishCanada = CultureInfo.GetCultureInfo("en-CA");
            public static readonly CultureInfo FrenchCanada = CultureInfo.GetCultureInfo("fr-CA");

            public static readonly List<CultureInfo> SupportedLocalizations = new List<CultureInfo>
            {
                EnglishCanada,
                FrenchCanada
            };
        }
    }
}