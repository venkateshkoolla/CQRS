using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Data.TestFramework
{
    public static class TestConstants
    {
        public static class AboriginalStatuses
        {
            public const string Other = "99";
        }

        public static class AcademicRecord
        {
            public static Guid Id => new Guid("FDEB89F9-F639-4D16-97B1-0003FA4F4027");
            public static Guid ApplicantId => new Guid("EB822F17-1973-4726-B09C-E197607B543C");
        }

        public static class OntarioStudentCourseCredit
        {
            public static Guid Id => new Guid("AAC5EC86-A138-4473-B4D7-0000278DC983");
            public static Guid ApplicantId => new Guid("CFBF37CE-E00F-4F60-879F-993D2D938506");
        }

        public static class Annotation
        {
            public static Guid Id => new Guid("BF7DB44F-0471-42EC-A1A0-000FFFDD25DA");
        }

        public static class ApplicantMessage
        {
            public static Guid ApplicantId => new Guid("99D4DC59-C6DB-4680-B71D-58E0BE910DC8");
        }

        public static class ApplicantSummary
        {
            public static Guid ApplicantId => new Guid("955D9411-280F-4087-8CEF-4AEDD18A1994");
        }

        public static class Application
        {
            public static Guid ApplicantId => new Guid("08C8C496-1A47-4BF0-B4CC-582EE075ACAB");
        }

        public static class ApplicationCycleIds
        {
            public static Guid Y2017 => new Guid("fb111006-a418-e411-80cd-00155d327045");
        }

        public static class ApplicationCycleStatuses
        {
            public const string Active = "A";
        }

        public static class ApplicationStatuses
        {
            public const string NewApply = "2";
        }

        public static class CollegeIds
        {
            public static Guid ALGO => new Guid("209dae78-b7c8-45cf-bfac-a3a001cb9d33");
        }

        public static class Config
        {
            public const int CommandTimeout = 120;
        }

        public static class Contact
        {
            public const string OntarioEducationNumberDefault = "000000000";

            public static class AccountStatuses
            {
                public const string Active = "1";
            }
        }

        public static class Countries
        {
            public const string Canada = "800";
            public const string UnitedStates = "667";
        }

        public static class Credentials
        {
            public const string Other = "96";
        }

        public static class CustomAudits
        {
            public static Guid Id => new Guid("0F432848-CDBD-E911-8133-00155D156D47");
            public static Guid ApplicantId => new Guid("BE46F581-FC60-4839-A2C0-6F2C99C39BDE");
            public static Guid ApplicationId => new Guid("EFA157C2-65C8-4BD8-923A-6CEFC139A831");
        }

        public static class DocumentDistributions
        {
            public static Guid ApplicationId => new Guid("A6D4D2CD-8E33-4F55-8D4A-C0347896818E");
        }

        public static class CustomAuditDetails
        {
            public static Guid Id => new Guid("AFDEBDFD-89AE-E911-8133-00155D156D47");
        }

        public static class DateFormat
        {
            public const string YearMonthDay = "yyyy-MM-dd";
            public const string YearMonthOnly = "yyyy-MM";
        }

        public static class Education
        {
            public const string HighSchool = "HS";

            public const string DefaultOntarioEducationNumber = "000000000";
            public static Guid TestApplicant3Intl => new Guid("567A6882-74E6-410D-A038-C54E23FF21D3");
        }

        public static class EtmsTranscriptRequest
        {
            public static Guid TestEtmsTranscriptRequest => new Guid("93E920AF-48E6-49FA-867C-00043F69D783");
        }

        public static class EtmsTranscriptRequestProcess
        {
            public static Guid TestEtmsTranscriptRequestProcess => new Guid("BD216118-873F-E911-812C-00155D156D46");
        }

        public static class FinancialTransactions
        {
            public static Guid PaymentWithVoucher => new Guid("65C6A994-098E-E911-8131-00155D156D46");
            public static Guid PaymentWithMoneris => new Guid("36D6F02F-AB8E-E911-8131-00155D156D46");
            public static Guid Deposit => new Guid("FB205CD2-0757-E911-812D-00155D156D46");
            public static Guid ReturnedPayment => new Guid("8C04BDC4-F264-E811-810E-00155D156D47");
            public static Guid ReverseFullPayment => new Guid("7A4AFA82-F164-E811-810E-00155D156D47");
            public static Guid Refund => new Guid("59B1E8B1-9F1A-E911-812A-00155D156D47");
            public static Guid Transfer => new Guid("826F07AA-7F97-E911-8132-00155D156D46");
            public static Guid ReleaseOverpayment => new Guid("C20A7DE6-373D-E311-883A-00155D966496");
            public static Guid ReleaseFundonDeposit => new Guid("DABBAD6E-3A3D-E311-9624-00155D967DA0");
        }

        public static class HighSchoolUser
        {
            public static Guid ApplicantId => new Guid("EDB3F3B1-F6E8-4A23-8B62-E0C06F3D5027");
            public const string PartnerId = "929336";
            public const string BoardId = "28070";
        }

        public static class CollegeUser
        {
            public static Guid ApplicantId => new Guid("3FC86115-9E5F-4DC9-9B27-C4598AC968EC");
            public const string PartnerId = "SSFL";
        }

        public static class Institutes
        {
            public static Guid Id => new Guid("1947390E-389D-4EE5-AB55-4A639317F427");
        }

        public static class InstituteTypes
        {
            public const string HighSchool = "HS";
            public const string College = "C";
            public const string University = "U";
        }

        public static class OcasApplicants
        {
            public const string TestApplicant1Password = "Welcome1";
            public const string TestApplicant1Username = "testapplicant1@mailinator.com";
            public const string TestApplicant2Password = "Welcome1";
            public const string TestApplicant2Username = "testapplicant2@mailinator.com";
            public static Guid TestApplicant3Id => new Guid("065470EA-5431-45AD-B9BD-6B0C80F930ED");
        }

        public static class Offers
        {
            public static Guid ApplicantWithOffers => new Guid("83174590-DB38-462F-AA71-89136C91CD8E");
            public static Guid ApplicationWithOffers => new Guid("1DF82037-FE91-4DF8-9B0F-EA5062DB4898");
            public static Guid TestOffer => new Guid("D9AA35A0-F2CB-452B-9322-AB36E3752789");
        }

        public static class OfferAcceptances
        {
            public static Guid ApplicationWithOffer => new Guid("08C312DB-1842-4E08-8D5D-E3463B27EC7B");
        }

        public static class Orders
        {
            public static Guid TestOrder => new Guid("61C7B30E-ED5B-4957-8103-57F6E774DE63");

            public static class TestApplicant
            {
                public static Guid ApplicationId => new Guid("b1b355b8-6724-4ce7-aa9a-c079aa045f94");
                public static Guid ApplicantId => new Guid("2d74f40b-978f-4185-9621-d9cbfdeaa3f5");
                public static Guid SourceId => new Guid("45942575-dbff-e811-812a-00155d156d47");
                public static string ModifiedBy => "darrentest_20190515@test.ocas.ca";
            }
        }

        public static class OrderDetails
        {
            public static Guid TestOrderDetail => new Guid("B88DCB6F-E6EE-4D69-945B-8D76736C816E");
        }

        public static class PaymentMethods
        {
            public const string Cheque = "CH";
            public const string Mastercard = "M";
            public const string Na = "NA";
            public const string Visa = "V";
        }

        public static class Program
        {
            public static Guid IdWithApplications => new Guid("B7C90555-9D4B-4A3F-9C60-B8FD7DB66BBD");
        }

        public static class ProgramChoices
        {
            public static Guid ProgramIntake => new Guid("88462F4F-C6E4-4E09-8692-02452E79DEEF");
        }

        public static class ProgramIntake
        {
            public static List<Guid> Ids => new List<Guid> { new Guid("AC4CBDA1-9612-4B34-B15E-000E27C3F1AC"), new Guid("52A59392-8734-43A4-A500-000DF5D83B97") };
        }

        public static class Provinces
        {
            public const string Ontario = "ON";
        }

        public static class SchoolStatuses
        {
            public const string Closed = "C";
            public const string Open = "O";
        }

        public static class ShoppingCarts
        {
            public static Guid ApplicationWithShoppingCart => new Guid("1C2781A5-0F5D-4862-A8DE-0004F6952B78");
            public static Guid ApplicationWithSupplementalFee => new Guid("CCFD5401-4EF7-4F5D-8C2E-5BED6FC8C521");
            public static Guid ApplicantWithShoppingCartDetails => new Guid("d0aa9372-b42b-4c4f-bede-eed3353be206");
        }

        public static class Sources
        {
            public const string A2C2 = "A2C2";
        }

        public static class SupportingDocuments
        {
            public static Guid Id => new Guid("7DC42897-D797-E711-80F9-00155D156D1A");
            public static Guid ApplicantId => new Guid("C8902084-A406-453B-88C2-A2E024351D79");
        }

        public static class Tests
        {
            public static Guid ApplicantWithTestDetails => new Guid("331F936D-DE29-E211-B6FB-00155D00646D");
            public static Guid TestWithDetails => new Guid("80C462A1-D82C-E211-B6FB-00155D00646D");
        }

        public static class TestCollege
        {
            public const string Code = "HUMB";
        }

        public static class Transcripts
        {
            public static Guid Id => new Guid("CE73FB06-3A82-E711-80F9-00155D156D1A");
            public static Guid ContactId => new Guid("D8270146-18BE-4329-BC6D-F6DB69AF5438");
            public static Guid PartnerId => new Guid("71FAB410-F014-4562-BB4B-5032936695DE");
        }

        public static class Vouchers
        {
            public static Guid ApplicationWithVoucher => new Guid("76A3B763-FB2F-40CA-B1E9-965786EF457C");
        }

        public static class InternationalCreditAssessment
        {
            public static Guid Id => new Guid("3CACFE15-0B46-4D74-B0BE-FFF608F8F1EB");
            public static Guid ApplicantId => new Guid("FA3CE5B2-0A30-44B2-9ECA-54F398E0F085");
        }
    }
}
