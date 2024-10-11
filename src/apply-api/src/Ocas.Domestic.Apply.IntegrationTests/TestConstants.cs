using Ocas.Domestic.Apply.TestFramework;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public static class TestConstants
    {
        public static class Algolia
        {
            public static class ProgramIntakes
            {
                public const string ApplicationId = "6C7TQDV5UT";
                public const string ApiKey = "aa5a1a16077789b2281441fbc926c891";
                public const string Index = "dev_programintakes_cbv6";
            }
        }

        public static class Etms
        {
            public const string GeorgianUser = @"partner\uat.geor.etms";
            public const string GeorgianPassword = "ocas2019!";
        }

        public static class Identity
        {
            public static class Providers
            {
                public static class OcasAdfs
                {
                    //Ocas CCC role
                    public const string QaCccUsername = @"as\qa_ccc_support";
                    public const string QaCccPassword = "Ocas2017!";

                    //Ocas no access
                    public const string QaNoAccessUsername = @"as\tstest";
                    public const string QaNoAccessPassword = "Ocas2019!";
                }

                public static class PartnerIdp
                {
                    public const string PartnerTestUsername = "uat.sene.testuser";
                    public const string PartnerTestPassword = "Ocas2018!";
                }

                public static class OcasApplicants
                {
                    public const string ApplicantTestUsername = "testapplicant1@mailinator.com";
                    public const string ApplicantTestUsernamePw = "Welcome1";

                    public const string ApplicantWithCollegeTransmissions = "5mi1mcc181mew767tr2a_daysontest@test.ocas.ca";
                    public const string ApplicantWithCollegeTransmissionsPw = "ocas1234";

                    public const string ApplicantWithMesssages = "damith668@mailinator.com";
                    public const string ApplicantWithMesssagesPw = "Welcome1";

                    public const string ApplicantWithSupportingDocuments = "darrentest_20190515@test.ocas.ca";
                    public const string ApplicantWithSupportingDocumentsPw = "Welcome1";

                    public const string ApplicantWithAcceptedOffer = "santiago_bahringer6420190716@test.ocas.ca";
                    public const string ApplicantWithAcceptedOfferPw = "Welcome1";

                    public const string ValidPassword = IdentityConstants.ValidPassword;
                }
            }
        }

        public static class Moneris
        {
            public const string MasterCard = "5454545454545454";
            public const string Visa = "4242424242424242";
            public const string HostedTokenizationKey = "htQZ4JKOJHH3QJU";

            public const decimal AmountForDeclinedResponse = 10.37M;
        }
    }
}
