namespace Ocas.Domestic.Apply.TestFramework
{
    public static class IdentityConstants
    {
        public const string AccountBaseUri = "https://account-ci.dev.ocas.ca/";
        public const string AuthBaseUri = "https://authenticate-ci.dev.ocas.ca/auth";
        public const string RedirectUri = "http://localhost:4200/authorized";

        public const string ValidPassword = "Welcome1!";

        public static class Apply
        {
            public const string Client = "a2c.apply";
            public const string ApplicantScope = "openid profile email apply_api";
            public const string OcasScope = "openid profile email roles apply_api";
        }

        public static class Admin
        {
            public const string Client = "a2c.admin";
            public const string OcasScope = "openid profile email roles applyadmin_api";
        }
    }
}
