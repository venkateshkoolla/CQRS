using System.Collections.Generic;
using System.Globalization;

namespace Ocas.Domestic.Apply.Api.Client
{
    public static class ApplyApiConstants
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
