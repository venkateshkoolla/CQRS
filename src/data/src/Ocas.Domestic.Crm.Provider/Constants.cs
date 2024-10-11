namespace Ocas.Domestic.Crm.Provider
{
    internal static class Constants
    {
        #region Language Codes

        // This code is the language code used in CRM
        public static class LanguageCodes
        {
            public const int English = 1033;
            public const int French = 1036;
        }

        #endregion Language Codes

        #region Enumeration

        internal enum State
        {
            Inactive = 1,
            Active = 0
        }

        internal enum Status
        {
            Inactive = 2,
            Active = 1
        }

        #region Contact Category IDs

        internal enum ContactUserTypes
        {
            Applicant = 1,
            SecondConsent = 2,

            //this is not a office contact type in contact, this value is used by ESM
            MigratedApplicant = 3
        }

        #endregion Contact Category IDs

        #region Entity Constants

        #region Lookup Codes

        public static class LookupCodes
        {
            public static class InstituteType
            {
                public const string Highschool = "HS";
                public const string College = "C";
                public const string University = "U";
            }

            public static class NewApplicationCycleStatusNames
            {
                public const string Active = "A";
                public const string Draft = "D";
                public const string Previous = "P";
            }

            public static class OfferStatus
            {
                public const string Accepted = "2";
                public const string Declined = "3";
                public const string NoDecision = "0";
            }

            public static class OfferState
            {
                public const string Active = "A";
                public const string Revoked = "R";
                public const string Deleted = "D";
                public const string Suspended = "S";
            }
        }

        #endregion Lookup Codes

        public static class InstituteType
        {
            public const string Highschool = "High School";
            public const string College = "College";
            public const string University = "University";

            // following is for search filters
            public const int HighschoolSearch = 1;

            public const int CollegeSearch = 2;
            public const int UniversitySearch = 8;
            public const int CollegeAndUniversity = 4;
        }

        public static class ApplicationCycleStatus
        {
            public const string OpenForEdit = "1";
            public const string Approved = "2";
            public const string Published = "3";
        }

        public static class SchoolStatus
        {
            public const string Open = "O";
            public const string Closed = "C";
        }

        public static class Payment
        {
            public const string Cash = "Cash";
            public const string Cheque = "Cheque";
            public const string MoneyOrder = "Money Order";
            public const string OCASCredit = "OCAS Credit";
            public const string OnlineBanking = "Online Banking";
            public const string AmericanExpress = "American Express";
            public const string Mastercard = "Mastercard";
            public const string Visa = "Visa";
            public const string Interac = "INTERAC Online";
            public const string NA = "N/A";

            //Paying with voucher for $0 order
            public const string PaymentResponseCode = "0";

            public const string OrderConfirmationNumber = "0";
        }

        #endregion Entity Constants

        #region eTMS

        public static class ETMSTranscriptRequest
        {
            public const string SendTranscriptNowTermCode = "N";
            public const string Dash = "-";
            public const string Space = " ";
            public const string FileNameSplitter = "\\";
            public const string FileExtensionSplitter = ".";
            public const string FirstNameXPath = "Student/Person/Name/FirstName";
            public const string LastNameXPath = "Student/Person/Name/LastName";
            public const string BirthDateXPath = "Student/Person/Birth/BirthDate";
            public const string GenderXPath = "Student/Person/Gender/GenderCode";
            public const string HighSchoolNumberXPath = "Student/Person/SchoolAssignedPersonID";
            public const string OenXPath = "Student/Person/AgencyAssignedID";
            public const string HighSchoolBsidXPath = "Student/AcademicRecord/AcademicSession/School/ESIS";
            public const string OssdIssueDateXPath = "Student/AcademicRecord/AcademicSession/AcademicSessionDetail/SessionEndDate";
            public const string DocumentIdXPath = "TransmissionData/DocumentID";
            public const string RequestTrackingIdXPath = "TransmissionData/RequestTrackingID";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string BirthDate = "BirthDate";
            public const string Gender = "Gender";
            public const string HighSchoolNumber = "HighSchoolNumber";
            public const string Oen = "Oen";
            public const string HighSchoolBsid = "HighSchoolBsid";
            public const string OssdIssueDate = "OssdIssueDate";
            public const string BirthDateFormat = "yyyy-MM-dd";
            public const string XmlFileType = "XML";
            public const string PdfFileType = "PDF";
            public const string DatFileType = "DAT";
            public const string TemporaryUploadFileType = "upload";
            public const string TimeStampFormat = "yyyyMMddHHmmssffff";
            public const string ExportPdfFileName = "TranscriptRequests.pdf";
            public const string ExportCsvFileName = "TranscriptRequests.csv";

            //public const string ExportFileTitle = "Transcript Requests";
            public const string TranscriptSchemaFilePath = @"Content\XMLSchema\HighSchoolTranscript.xsd";

            public const string PathToBinFolder = "bin";
            public const string EmailSubject = "Hardcopy Transcript from {0} {1}";
            public const string ParameterSeprator = "&lt;&lt;&lt;";
            public const string PropertySeprator = "::";
            public const string AttributeNoValue = "Not Available";
            public const string ListAllSuffix = "LISTALL_";
            public const string MailDateVariable = "maildate";
            public const string TranscriptFileUploadEmailTemplate = "eTMSTranscriptFileUploadEmailTemplateEnglish";
            public const string EmailAddressSeparator = ";";
            public const string XmlEntity = "entity";
            public const string XmlAttribute = "attribute";
            public const string XmlDefaultAttribute = "default";
            public const string XmlGraduatedAttributeTrue = "Y";
            public const string XmlGraduatedAttributeFalse = "N";
        }

        #endregion eTMS

        #endregion Enumeration

    }
}
