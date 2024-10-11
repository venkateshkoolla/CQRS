using System;
using Ocas.Domestic.Apply.TestFramework;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public static class TestConstants
    {
        public static class Applicant
        {
            public static Guid ApplicantUpdateInfoId => new Guid("331F936D-DE29-E211-B6FB-00155D00646D");
        }

        public static class Application
        {
            public static Guid ApplicationIdWithOffers => new Guid("08C312DB-1842-4E08-8D5D-E3463B27EC7B");
            public static Guid ApplicationIdWithCollegeTransmissions => new Guid("0230957E-74AE-4180-9A6A-83209432F650");
        }

        public static class Identity
        {
            public static class Providers
            {
                public static class OcasAdfs
                {
                    //Ocas CCC role
                    public const string QaCccUsername = @"as\qa_ccc_support";
                    public const string QaCccEmail = "qa_ccc_support@ocas.ca";
                    public const string QaCccPassword = "Ocas2017!";

                    public const string QaBoUsername = "qatest@ocas.ca";
                    public const string QaBoPassword = "Ocas2015!";

                    //Ocas no access
                    public const string QaNoAccessUsername = @"as\tstest";
                    public const string QaNoAccessPassword = "Ocas2019!";
                }

                public static class PartnerIdp
                {
                    public const string PartnerTestUsername = "uat.sene.testuser";
                    public const string PartnerTestPassword = "Ocas2018!";

                    public const string SeneTestUsername = "uat.sene.testuser";
                    public const string SeneTestPassword = "Ocas2018!";

                    public const string ActonHsUsername = "uat.890332.admin";
                    public const string ActonHsPassword = "Ocas2019!";

                    public const string HaltonBoardUsername = "uat.66133.admin";
                    public const string HaltonBoardPassword = "Ocas2019!";
                }

                public static class OcasApplicants
                {
                    public const string ApplicantTestUsername = "testapplicant1@mailinator.com";
                    public const string ApplicantWithMesssages = "damith668@mailinator.com";
                    public const string ApplicantWithOfficialTests = "suefour@mailinator.com";
                    public const string ApplicantWithSupportingDocuments = "darrentest_20190515@test.ocas.ca";
                    public const string ApplicantWithAcceptedOffer = "santiago_bahringer6420190716@test.ocas.ca";

                    public const string AlternatePassword = "Welcome1";
                    public const string ValidPassword = IdentityConstants.ValidPassword;
                }
            }
        }

        public static class Intake
        {
            public static Guid IntakeIdWithApplicants => new Guid("9631320A-CDB2-40EE-9DA6-988E436DDE80");
        }
    }
}
