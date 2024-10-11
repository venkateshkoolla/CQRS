using System.Globalization;
using System.Security.Claims;

namespace Ocas.Domestic.Apply.Api.Services.UnitTests
{
    public static class TestConstants
    {
        public static class AppSettings
        {
            public const string LockStartTime = "22:00:00";
            public const string LockEndTime = "23:00:00";
        }

        public static class Identity
        {
            public const string OcasRole = "BO";
        }

        public static class Locale
        {
            public const string English = "en-CA";
            public static readonly CultureInfo EnglishCanada = CultureInfo.GetCultureInfo("en-CA");
            public static readonly CultureInfo FrenchCanada = CultureInfo.GetCultureInfo("fr-CA");
        }

        public static class TestUser
        {
            public static readonly ClaimsPrincipal ApplicantPrincipal = new ClaimsPrincipal(
                    new ClaimsIdentity(new[]
                    {
                        new Claim("upn", "alextest_20181217@mailinator.com"),
                        new Claim("email", "alextest_20181217@mailinator.com"),
                        new Claim("given_name", "Alex"),
                        new Claim("family_name", "Test"),
                        new Claim("birthdate", "1986-02-03")
                    }));
        }
    }
}
