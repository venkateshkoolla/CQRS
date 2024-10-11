using System.Globalization;
using System.Security.Claims;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests
{
    public static class TestConstants
    {
        public static class AppSettings
        {
            public const string LockStartTime = "22:00:00";
            public const string LockEndTime = "23:00:00";
        }

        public static class Locale
        {
            public static readonly CultureInfo EnglishCanada = CultureInfo.GetCultureInfo("en-CA");
            public static readonly CultureInfo FrenchCanada = CultureInfo.GetCultureInfo("fr-CA");
        }

        public static class TestUser
        {
            public static class College
            {
                public const string PartnerId = "SENE";

                public static readonly ClaimsPrincipal TestPrincipal = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new[]
                            {
                            new Claim("upn", "qatest@Partners.onco.local"),
                            new Claim("customer_code", Constants.IdentityServer.Partner.Customer),
                            new Claim("partner_id", PartnerId),
                            new Claim(ClaimTypes.Role, "UAT_PartnerCollegeUsers")
                            }));
            }

            public static class HsBoard
            {
                public static readonly ClaimsPrincipal TestPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("customer_code", Constants.IdentityServer.Partner.Customer),
                        new Claim(ClaimTypes.Role, "UAT_PartnerBoardUsers")
                    }));
            }

            public static class HsUser
            {
                public static readonly ClaimsPrincipal TestPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("customer_code", Constants.IdentityServer.Partner.Customer),
                        new Claim(ClaimTypes.Role, "UAT_PartnerSchoolUsers"),
                        new Claim("partner_id", "890332")
                    }));
            }

            public static class Ocas
            {
                public const string UpnOrEmail = "qatest@ocas.ca";

                public static readonly ClaimsPrincipal TestPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new[]
                        {
                            new Claim("upn", UpnOrEmail),
                            new Claim(ClaimTypes.Role, "CCC")
                        }));
            }
        }
    }
}
