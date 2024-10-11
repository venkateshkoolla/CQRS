namespace Ocas.Domestic
{
    public static class Constants
    {
        public static class AdultTraining
        {
            public const string No = "N";
        }

        public static class BasisForAdmission
        {
            /// <summary>
            /// Applicant Has Not Indicated
            /// </summary>
            public const string NotIndicated = "0";

            /// <summary>
            /// Applicant Is Or Will Be A Secondary School Graduate (Achieved An Ossd Under Os:Is) By The First Day Of College, I.E. The Start Date To Which They Are Applying.
            /// Ossd = Ontario Secondary School Diploma
            /// Os:Is = Ontario Schools: Intermediate and Senior (OS:IS)
            /// https://en.wikipedia.org/wiki/Ontario_Academic_Credit#Course_load
            /// </summary>
            public const string WillHaveOssdUnderOsIs = "1";

            /// <summary>
            /// The Applicant Will Not Have An Ossd By The First Day Of Classes.
            /// Ossd = Ontario Secondary School Diploma
            /// </summary>
            public const string WillNotHaveOssd = "2";

            /// <summary>
            /// Applicant Is Or Will Be A Secondary School Graduate (Achieved An Ossd Under Oss) By The First Day Of College,I.E. The Start Date To Which They Are Applying.
            /// Ossd = Ontario Secondary School Diploma
            /// </summary>
            public const string WillHaveOssd = "3";

            /// <summary>
            /// The Applicant Will Not Have An Ossc By The First Day Of Classes.
            /// Ossc = Ontario Secondary School Certificate
            /// </summary>
            public const string WillNotHaveOssc = "4";

            /// <summary>
            /// Applicant Is Or Will Be A Secondary School Graduate (Achieved An Ossc Under Oss) By The First Day Of College, I.E. The Start Date To Which They Are Applying.
            /// Ossc = Ontario Secondary School Certificate
            /// </summary>
            public const string WillHaveOssc = "5";
        }

        public static class Current
        {
            public const string No = "N";
            public const string Yes = "Y";
        }

        public static class Payment
        {
            public const string Na = "NA";
            public const string OrderConfirmationNumber = "0";
            public const string PaymentResponseCode = "0";
        }

        public static class AboriginalStatuses
        {
            public const string Other = "99";
        }

        public static class ApplicationCycleStatuses
        {
            public const string Active = "A";
            public const string Draft = "D";
            public const string Previous = "P";
        }

        public static class ApplicationStatuses
        {
            public const string Active = "1";
            public const string NewApply = "2";
            public const string Withdrawn = "4";
            public const string PendingPayment = "7";
        }

        public static class CanadianStatuses
        {
            public const string CanadianCitizen = "1";
            public const string StudyPermit = "4";
        }

        public static class CollegeTransmissionCodes
        {
            public const string Applicant = "AC";
            public const string ProgramChoice = "CC";
            public const string Grade = "GC";
            public const string SupportingDocument = "SC";
            public const string Test = "TC";
            public const string InternationalTranscriptEvalution = "IC";
            public const string Education = "EC";
            public const string NonFullTimeStudentActivity = "NC";
        }

        public static class CollegeTransmissionTransactionTypes
        {
            public const char Insert = 'I';
            public const char Update = 'U';
            public const char Delete = 'D';
        }

        public static class Contact
        {
            public const string OntarioEducationNumberDefault = "000000000";
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

        public static class DateFormat
        {
            public const string YearMonthDay = "yyyy-MM-dd";
            public const string YearMonthOnly = "yyyy-MM";
            public const string YearMonthNoDashOnly = "yyyyMM";
            public const string CcExpiry = "yyMM";
            public const string OsapDate = "yyyMMdd";
            public const string OfferTransmission = "yyyMMdd";
        }

        public static class Education
        {
            public const string DefaultOntarioEducationNumber = "000000000";
        }

        public static class InstituteTypes
        {
            public const string HighSchool = "HS";
            public const string College = "C";
            public const string University = "U";
        }

        public static class Offers
        {
            public static class Status
            {
                public const string Accepted = "2";
                public const string Declined = "3";
                public const string NoDecision = "0";
            }

            public static class State
            {
                public const string Active = "A";
                public const string Deleted = "D";
            }
        }

        public static class Provinces
        {
            public const string Ontario = "ON";
        }

        public static class SchoolStatuses
        {
            public const string Open = "O";
        }

        public static class SupportingDocumentTypes
        {
            public const string AcademicUpgradeDocuments = "17";
            public const string CertificatePostSecondary = "60";
            public const string CollegeTranscripts = "02";
            public const string Degree = "70";
            public const string DiplomaPostSecondary = "50";
            public const string EvaluationReport = "16";
            public const string InternationalTranscript = "47";
            public const string OutOfProvinceSecondarySchoolTranscript = "32";
            public const string UniversityTranscript = "03";
        }

        public static class TrancriptSources
        {
            public const string XLoad = "1";
            public const string SSLoad = "2";
            public const string AutoeTMS = "4";
            public const string ETMSPortal = "3";
            public const string FileExchange = "5";
            public const string OcasManual = "6";
            public const string Unknown = "0";
        }

        public static class TranscriptTransmissions
        {
            public const string AfterDegreeConferred = "R4";
            public const string SendTranscriptNow = "N";
        }

        public static class OfferTransmissionCodes
        {
            public const char Accepted = 'Y';
            public const char Declined = 'N';
        }

        public static class Currency
        {
            public const string Cad = "CAD";
        }

        public static class IntlCreditAssessments
        {
            public const string Completing = "1";
        }

        public static class PaymentResults
        {
            public const string Approved = "1";
        }

        public static class PostSecondaryTranscripts
        {
            public const string PescXmlNamespace = "urn:org:pesc:message:CollegeTranscript:v1.3.0";
        }

        public static class PreferredCorrespondenceMethods
        {
            public const string Email = "04";
        }

        public static class Product
        {
            public const string ApplicationFee = "APPLICATION FEE";
            public const string Voucher = "VOUCHER";
        }

        public static class ProgramIntakeAvailabilities
        {
            public const string Closed = "C";
            public const string Open = "O";
            public const string Waitlisted = "W";
        }

        public static class ProgramIntakeStatuses
        {
            public const string Active = "A";
            public const string Inactive = "I";
            public const string Cancelled = "N";
            public const string Suspended = "S";
        }

        public static class ProgramLanguageCode
        {
            public const string English = "English";
        }

        public static class Promotions
        {
            public const string Promotional = "02";
            public const string Standard = "01";
            public const string Unknown = "00";
        }

        public static class Sources
        {
            public const string A2C2 = "A2C2";
            public const string CBUI = "CBUI";
            public const string CBUIUNKNOWN = "CBUIUNKNOW";
        }

        public static class Titles
        {
            public const string Unknown = "0";
        }

        public static class TranscriptRequestStatuses
        {
            public const string NoGradesOnRecord = "ROUTE_EDI_147_27";
            public const string RequestInit = "REQUESTINIT";
            public const string RequestReissue = "REQUEST_REISSUE";
            public const string TranscriptNotFound = "ROUTE_EDI_147_10";
            public const string WaitingPayment = "WAITING_FOR_PAYMENT_ACTION";
            public const string WaitingPaymentApproval = "WAITFORPAYMAPPR";
        }
    }
}
