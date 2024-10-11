using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Ocas.Domestic.Apply.Enums;

namespace Ocas.Domestic.Apply.Models.Templates
{
    public class PostSecondaryTranscriptViewModel
    {
        // Transcript Information
        public string DateReceived { get; set; }
        public string ApplicationNumber { get; set; }

        // Student Information
        public string StudentPrefix { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentMiddleName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentFormerLastName { get; set; }
        public string StudentDob { get; set; }
        public string StudentGender { get; set; }
        public string StudentId { get; set; }
        public string StudentOen { get; set; }

        // Contact Info
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }

        // Notes/Special Instructions
        public string NotesSpecialInstructions { get; set; }

        public Labels Labels { get; set; } = new Labels();

        public TranscriptFooterViewModel FooterViewModel { get; set; }

        public IList<AcademicAward> AcademicAwards { get; set; } = new List<AcademicAward>();

        public IList<Semester> Semesters { get; set; } = new List<Semester>();

        public void LoadTranslations(TranslationsDictionary translationsDictionary, string institutionName)
        {
            Labels = new Labels
            {
                AcademicAwardDate = translationsDictionary.Get("transcript.post_secondary.academic_award_date"),
                AcademicAwardProgram = translationsDictionary.Get("transcript.post_secondary.academic_award_program"),
                AcademicAwardSectionHeader = translationsDictionary.Get("transcript.post_secondary.academic_award_section_header"),
                AcademicAwardTitle = translationsDictionary.Get("transcript.post_secondary.academic_award_title"),
                AcademicCreditsInGPA = translationsDictionary.Get("transcript.post_secondary.academic_credits_in_gpa"),
                AcademicGPA = translationsDictionary.Get("transcript.post_secondary.academic_gpa"),
                ApplicationNumber = translationsDictionary.Get("transcript.post_secondary.application_number"),
                BasisAcademicCreditByExam = translationsDictionary.Get("transcript.post_secondary.basis_academic_credit_by_exam"),
                BasisAcademicHighSchoolTransferCredit = translationsDictionary.Get("transcript.post_secondary.basis_academic_high_school_transfer_credit"),
                BasisAcademicRegular = translationsDictionary.Get("transcript.post_secondary.basis_academic_regular"),
                BasisforCredit = translationsDictionary.Get("transcript.post_secondary.basis_for_credit"),
                CollegeUniversityStudentID = translationsDictionary.Get("transcript.post_secondary.college_university_student_id"),
                ContactInformation = translationsDictionary.Get("transcript.post_secondary.contact_information"),
                ContactName = translationsDictionary.Get("transcript.post_secondary.contact_name"),
                ContactPhone = translationsDictionary.Get("transcript.post_secondary.contact_phone"),
                CourseGradeMark = translationsDictionary.Get("transcript.post_secondary.course_grade_mark"),
                CourseGradePoint = translationsDictionary.Get("transcript.post_secondary.course_grade_point"),
                CourseGradeQualifier = translationsDictionary.Get("transcript.post_secondary.course_grade_qualifier"),
                CourseNumber = translationsDictionary.Get("transcript.post_secondary.course_number"),
                CourseTitle = translationsDictionary.Get("transcript.post_secondary.course_title"),
                CreditValue = translationsDictionary.Get("transcript.post_secondary.credit_value"),
                CreditValueEarned = translationsDictionary.Get("transcript.post_secondary.credit_value_earned"),
                DateOfBirth = translationsDictionary.Get("transcript.post_secondary.date_of_birth"),
                DateReceived = translationsDictionary.Get("transcript.post_secondary.date_received"),
                DateSemesterStarted = translationsDictionary.Get("transcript.post_secondary.date_semester_started"),
                FirstName = translationsDictionary.Get("transcript.post_secondary.first_name"),
                FormerSurname = translationsDictionary.Get("transcript.post_secondary.former_surname"),
                Gender = translationsDictionary.Get("transcript.post_secondary.gender"),
                GenderFemale = translationsDictionary.Get("transcript.post_secondary.gender_female"),
                GenderMale = translationsDictionary.Get("transcript.post_secondary.gender_male"),
                GenderUnreported = translationsDictionary.Get("transcript.post_secondary.gender_unreported"),
                GradePointQualifier = translationsDictionary.Get("transcript.post_secondary.grade_point_qualifier"),
                NotesSpecialInstructions = translationsDictionary.Get("transcript.post_secondary.notes_special_instructions"),
                OEN = translationsDictionary.Get("transcript.post_secondary.oen"),
                OfficialTranscript = string.Format(translationsDictionary.Get("transcript.post_secondary.official_transcript"), institutionName),
                RangeMaximumforGPA = translationsDictionary.Get("transcript.post_secondary.range_maximum_for_gpa"),
                RangeMinimumforGPA = translationsDictionary.Get("transcript.post_secondary.range_minimum_for_gpa"),
                SecondNameAndInitial = translationsDictionary.Get("transcript.post_secondary.second_name_and_initial"),
                Semester = translationsDictionary.Get("transcript.post_secondary.semester"),
                SemesterSummary = translationsDictionary.Get("transcript.post_secondary.semester_summary"),
                StudentInformation = translationsDictionary.Get("transcript.post_secondary.student_information"),
                Surname = translationsDictionary.Get("transcript.post_secondary.surname"),
                TranscriptInformation = translationsDictionary.Get("transcript.post_secondary.transcript_information")
            };
            FooterViewModel = new TranscriptFooterViewModel(translationsDictionary);
        }

        public void LoadXml(XDocument doc, CultureInfo cultureInfo, PostSecondaryTranscriptVersion version)
        {
            switch (version)
            {
                case PostSecondaryTranscriptVersion.PESC:
                    LoadPescXml(doc, cultureInfo);
                    break;
                case PostSecondaryTranscriptVersion.X12:
                    LoadX12Xml(doc, cultureInfo);
                    break;
            }
        }

        private void LoadX12Xml(XDocument doc, CultureInfo cultureInfo)
        {
            // Transcript Information: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FTranscriptInformation.xsl&version=GBmaster
            DateReceived = TestAndFormatDate(doc.XPathSelectElement("//segment[@code='BGN']/element[@code='373']/value")?.Value, PostSecondaryTranscriptVersion.X12);

            // From A2C: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FStudentInformation3.xsl&version=GBmaster&line=4&lineStyle=plain&lineEnd=11&lineStartColumn=3&lineEndColumn=18
            var agencyIdentifiers = doc.XPathSelectElements("//segment[@code='REF']/element[@code='128']");
            var studentAgencyIds = new List<string>();
            foreach (var agencyIdentifier in agencyIdentifiers)
            {
                if (agencyIdentifier.XPathSelectElement("value")?.Value == "48")
                {
                    studentAgencyIds.Add(agencyIdentifier.XPathSelectElement("../element[@code='127']/value")?.Value);
                }
            }

            ApplicationNumber = studentAgencyIds.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            // Student Information: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FStudentInformation1.xsl&version=GBmaster
            StudentPrefix = doc.XPathSelectElements("//loop[@code='IN1']/segment[@code='IN2']/element[@code='1104' and ./value/text()='01']/../element[@code='93']").FirstOrDefault(x => !string.IsNullOrWhiteSpace(x?.Value))?.Value;
            StudentFirstName = doc.XPathSelectElements("//loop[@code='IN1']/segment[@code='IN2']/element[@code='1104' and ./value/text()='02']/../element[@code='93']").FirstOrDefault(x => !string.IsNullOrWhiteSpace(x?.Value))?.Value;
            StudentMiddleName = doc.XPathSelectElements("//loop[@code='IN1']/segment[@code='IN2']/element[@code='1104' and ./value/text()='03']/../element[@code='93']").FirstOrDefault(x => !string.IsNullOrWhiteSpace(x?.Value))?.Value;
            StudentLastName = doc.XPathSelectElements("//loop[@code='IN1']/segment[@code='IN2']/element[@code='1104' and ./value/text()='05']/../element[@code='93']").FirstOrDefault(x => !string.IsNullOrWhiteSpace(x?.Value))?.Value;
            StudentFormerLastName = doc.XPathSelectElements("//loop[@code='IN1']/segment[@code='IN2']/element[@code='1104' and ./value/text()='15']/../element[@code='93']").FirstOrDefault(x => !string.IsNullOrWhiteSpace(x?.Value))?.Value;
            StudentId = doc.XPathSelectElement("//segment[@code='REF']/element[@code='128' and ./value/text()='LR']/../element[@code='127']/value")?.Value;
            StudentDob = TestAndFormatDate(doc.XPathSelectElement("//segment[@code='DMG']/element[@code='1251']/value")?.Value, PostSecondaryTranscriptVersion.X12);

            var gender = doc.XPathSelectElement("//segment[@code='DMG']/element[@code='1068']/value")?.Value;
            switch (gender)
            {
                case "M":
                    StudentGender = Labels.GenderMale;
                    break;
                case "F":
                    StudentGender = Labels.GenderFemale;
                    break;
                case "U":
                    StudentGender = Labels.GenderUnreported;
                    break;
                default:
                    StudentGender = null;
                    break;
            }

            // Contact Information: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FContactInformation.xsl&version=GBmaster
            ContactName = doc.XPathSelectElement("//segment[@code='PER']/element[@code='93']/value")?.Value;
            ContactPhone = doc.XPathSelectElement("//segment[@code='PER']/element[@code='364']/value")?.Value;

            // Notes: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FTranscript.xsl&version=GBmaster&line=209&lineStyle=plain&lineEnd=210&lineStartColumn=1&lineEndColumn=1
            NotesSpecialInstructions = doc.XPathSelectElement("//segment[@code='NTE']/element[@code='352']/value")?.Value;

            // Semesters: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FTranscript.xsl&version=GBmaster&line=221&lineStyle=plain&lineEnd=366&lineStartColumn=9&lineEndColumn=24
            var sessions = doc.XPathSelectElements("//table[@section='detail']/loop[@code='LX']/loop[@code='SES']");
            foreach (var session in sessions)
            {
                var semester = new Semester
                {
                    Name = session.XPathSelectElement("segment[@code='SES']/element[@code='93']/value")?.Value,
                    StartDate = TestAndFormatDate(session.XPathSelectElement("segment[@code='SES']/element[@code='1251'][3]/value")?.Value, PostSecondaryTranscriptVersion.X12)
                };

                var academicSummaries = session.XPathSelectElements("loop[@code='SUM']");
                foreach (var academicSummary in academicSummaries)
                {
                    semester.AcademicSummaries.Add(new AcademicSummary
                    {
                        AcademicCreditsGpa = academicSummary.XPathSelectElement("segment[@code='SUM']/element[@code='380']/value")?.Value,
                        AcademicGpa = TestAndFormatDecimal(academicSummary.XPathSelectElement("segment[@code='SUM']/element[@code='1144']/value")?.Value, cultureInfo),
                        RangeMinGpa = TestAndFormatDecimal(academicSummary.XPathSelectElement("segment[@code='SUM']/element[@code='740']/value")?.Value, cultureInfo),
                        RangeMaxGpa = TestAndFormatDecimal(academicSummary.XPathSelectElement("segment[@code='SUM']/element[@code='741']/value")?.Value, cultureInfo)
                    });
                }

                var courses = session.XPathSelectElements("loop[@code='CRS']");
                foreach (var course in courses)
                {
                    semester.Courses.Add(new Course
                    {
                        CourseNumber = string.Join(
                            " ",
                            course.XPathSelectElement("segment[@code='CRS']/element[@code='93'][1]/value")?.Value,
                            course.XPathSelectElement("segment[@code='CRS']/element[@code='127']/value")?.Value)
                            .Trim(),
                        CourseTitle = course.XPathSelectElement("segment[@code='CRS']/element[@code='93'][2]/value")?.Value,
                        BasisForCredit = course.XPathSelectElement("segment[@code='CRS']/element[@code='1147']/value")?.Attribute(XName.Get("description"))?.Value,
                        CreditValue = course.XPathSelectElement("segment[@code='CRS']/element[@code='380'][1]/value")?.Value,
                        CreditValueEarned = course.XPathSelectElement("segment[@code='CRS']/element[@code='380'][2]/value")?.Value,
                        CourseGradeMark = course.XPathSelectElement("segment[@code='CRS']/element[@code='1258']/value")?.Value,
                        CourseGradeQualifier = course.XPathSelectElement("segment[@code='CRS']/element[@code='1148']/value")?.Value,
                        CourseGradePoint = course.XPathSelectElement("loop[@code='MKS']/segment/element[@code='1258']/value")?.Value,
                        GradePointQualifier = course.XPathSelectElement("loop[@code='MKS']/segment/element[@code='1148']/value")?.Value
                    });
                }

                Semesters.Add(semester);
            }
        }

        private void LoadPescXml(XDocument doc, CultureInfo cultureInfo)
        {
            // Transcript Information: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FPESCTranscriptInformation.xsl&version=GBmaster
            DateReceived = TestAndFormatDate(doc.XPathSelectElement("//TransmissionData/CreatedDateTime")?.Value, PostSecondaryTranscriptVersion.PESC);

            // Student Information: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FPESCStudentInformation.xsl&version=GBmaster
            StudentPrefix = doc.XPathSelectElement("//*[local-name()='Student']/*[local-name()='Person']/*[local-name()='Name']/*[local-name()='NamePrefix']")?.Value;
            StudentFirstName = doc.XPathSelectElement("//*[local-name()='Student']/*[local-name()='Person']/*[local-name()='Name']/*[local-name()='FirstName']")?.Value;
            StudentMiddleName = doc.XPathSelectElement("//*[local-name()='Student']/*[local-name()='Person']/*[local-name()='Name']/*[local-name()='MiddleName']")?.Value;
            StudentLastName = doc.XPathSelectElement("//*[local-name()='Student']/*[local-name()='Person']/*[local-name()='Name']/*[local-name()='LastName']")?.Value;
            StudentFormerLastName = doc.XPathSelectElement("//*[local-name()='Student']/*[local-name()='Person']/*[local-name()='AlternateName']/*[local-name()='LastName']")?.Value;
            StudentId = doc.XPathSelectElement("//*[local-name()='Student']/*[local-name()='Person']/*[local-name()='SchoolAssignedPersonID']")?.Value;

            var gender = doc.XPathSelectElement("//*[local-name()='Student']/*[local-name()='Person']/*[local-name()='Gender']/*[local-name()='GenderCode']")?.Value;
            switch (gender)
            {
                case "Male":
                    StudentGender = Labels.GenderMale;
                    break;
                case "Female":
                    StudentGender = Labels.GenderFemale;
                    break;
                case "Unreported":
                    StudentGender = Labels.GenderUnreported;
                    break;
                default:
                    StudentGender = null;
                    break;
            }

            StudentDob = TestAndFormatDate(doc.XPathSelectElement("//*[local-name()='Student']/*[local-name()='Person']/*[local-name()='Birth']/*[local-name()='BirthDate']")?.Value, PostSecondaryTranscriptVersion.PESC);

            // From A2C: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FPESCStudentInformation.xsl&version=GBmaster&line=44&lineStyle=plain&lineEnd=58&lineStartColumn=3&lineEndColumn=18
            // look for the application number in 3 different places
            var studentAgencyIds = new List<string>();
            var agencyAssignedId = doc.XPathSelectElement("//*[local-name()='Student']/*[local-name()='Person']/*[local-name()='AgencyAssignedID']")?.Value;
            var agencyIdentifiers = doc.XPathSelectElements("//*[local-name()='Student']/*[local-name()='Person']/*[local-name()='AgencyIdentifier']");

            studentAgencyIds.Add(agencyAssignedId);
            foreach (var agencyIdentifier in agencyIdentifiers)
            {
                if (agencyIdentifier.XPathSelectElement("AgencyName")?.Value == "OCAS Application Number")
                {
                    studentAgencyIds.Add(agencyIdentifier.XPathSelectElement("AgencyAssignedID")?.Value);
                }

                if (agencyIdentifier.XPathSelectElement("AgencyName")?.Value == "OUAC Application Number")
                {
                    studentAgencyIds.Add(agencyIdentifier.XPathSelectElement("AgencyAssignedID")?.Value);
                }

                // From A2C: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FPESCStudentInformation.xsl&version=GBmaster&line=60&lineStyle=plain&lineEnd=67&lineStartColumn=3&lineEndColumn=18
                // OEN is found in the AgencyIdentifiers as well
                if (agencyIdentifier.XPathSelectElement("AgencyCode")?.Value == "State")
                {
                    StudentOen = agencyIdentifier.XPathSelectElement("AgencyAssignedID")?.Value;
                }
            }

            ApplicationNumber = studentAgencyIds.FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));

            // Contact Information: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FPESCContactInformation.xsl&version=GBmaster
            var contactName1 = doc.XPathSelectElement("//*[local-name()='Source']/*[local-name()='Organization']/*[local-name()='Contacts']/*[local-name()='NoteMessage']")?.Value;
            var contactName2 = doc.XPathSelectElement("//*[local-name()='Source']/*[local-name()='Organization']/*[local-name()='Contacts']/*[local-name()='Phone']/*[local-name()='NoteMessage']")?.Value;
            ContactName = contactName1 + contactName2;

            var contactPhone1 = doc.XPathSelectElement("//*[local-name()='Source']/*[local-name()='Organization']/*[local-name()='Contacts']/*[local-name()='Phone']/*[local-name()='AreaCityCode']")?.Value;
            var contactPhone2 = doc.XPathSelectElement("//*[local-name()='Source']/*[local-name()='Organization']/*[local-name()='Contacts']/*[local-name()='Phone']/*[local-name()='PhoneNumber']")?.Value;
            ContactPhone = contactPhone1 + contactPhone2;

            // Academic Award within an Academic Record: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FPESCTranscript.xsl&version=GBmaster&line=198&lineStyle=plain&lineEnd=253&lineStartColumn=5&lineEndColumn=20
            var recordAwards = doc.XPathSelectElements("//*[local-name()='Student']/*[local-name()='AcademicRecord']/*[local-name()='AcademicAward']");
            foreach (var recordAward in recordAwards)
            {
                var awardDate = recordAward.XPathSelectElement("AcademicAwardDate")?.Value;

                if (!string.IsNullOrEmpty(awardDate))
                {
                    AcademicAwards.Add(new AcademicAward
                    {
                        Title = recordAward.XPathSelectElement("AcademicAwardTitle")?.Value,
                        Date = awardDate,
                        ProgramName = recordAward.XPathSelectElement("AcademicAwardProgram/AcademicProgramName")?.Value
                    });
                }
            }

            // Academic Award within an Academic Session: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FPESCTranscript.xsl&version=GBmaster&line=255&lineStyle=plain&lineEnd=299&lineStartColumn=5&lineEndColumn=20
            var sessionAwards = doc.XPathSelectElements("//*[local-name()='Student']/*[local-name()='AcademicRecord']/*[local-name()='AcademicSession']/*[local-name()='AcademicAward']");
            foreach (var sessionAward in sessionAwards)
            {
                var awardDate = sessionAward.XPathSelectElement("AcademicAwardDate")?.Value;

                if (!string.IsNullOrEmpty(awardDate))
                {
                    AcademicAwards.Add(new AcademicAward
                    {
                        Title = sessionAward.XPathSelectElement("AcademicAwardTitle")?.Value,
                        Date = awardDate,
                        ProgramName = sessionAward.XPathSelectElement("AcademicAwardProgram/AcademicProgramName")?.Value
                    });
                }
            }

            // Notes: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FPESCTranscript.xsl&version=GBmaster&line=334&lineStyle=plain&lineEnd=340&lineStartColumn=5&lineEndColumn=9
            var notes1 = doc.XPathSelectElement("/*[local-name()='CollegeTranscript']/*[local-name()='NoteMessage']")?.Value;
            var notes2 = doc.XPathSelectElement("/*[local-name()='HighSchoolTranscript']/*[local-name()='NoteMessage']")?.Value;
            NotesSpecialInstructions = notes1 + notes2;

            // Semesters: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FPESCTranscript.xsl&version=GBmaster&line=344&lineStyle=plain&lineEnd=502&lineStartColumn=5&lineEndColumn=11
            var sessions = doc.XPathSelectElements("//*[local-name()='Student']/*[local-name()='AcademicRecord']/*[local-name()='AcademicSession']");
            foreach (var session in sessions)
            {
                var semester = new Semester
                {
                    Name = session.XPathSelectElement("AcademicSessionDetail/SessionName")?.Value,
                    StartDate = session.XPathSelectElement("AcademicSessionDetail/SessionBeginDate")?.Value?.Substring(0, 10)
                };

                var academicSummaries = session.XPathSelectElements("AcademicSummary");
                foreach (var academicSummary in academicSummaries)
                {
                    semester.AcademicSummaries.Add(new AcademicSummary
                    {
                        AcademicCreditsGpa = academicSummary.XPathSelectElement("GPA/CreditHoursForGPA")?.Value ?? academicSummary.XPathSelectElement("GPA/CreditHoursforGPA")?.Value,
                        AcademicGpa = TestAndFormatDecimal(academicSummary.XPathSelectElement("GPA/GradePointAverage")?.Value, cultureInfo),
                        RangeMinGpa = TestAndFormatDecimal(academicSummary.XPathSelectElement("GPA/GPARangeMinimum")?.Value, cultureInfo),
                        RangeMaxGpa = TestAndFormatDecimal(academicSummary.XPathSelectElement("GPA/GPARangeMaximum")?.Value, cultureInfo)
                    });
                }

                var courses = session.XPathSelectElements("Course");
                foreach (var course in courses)
                {
                    var creditBasisDescription = string.Empty;
                    var creditBasis = course.XPathSelectElement("CourseCreditBasis")?.Value;
                    switch (creditBasis)
                    {
                        case "Regular":
                            creditBasisDescription = Labels.BasisAcademicRegular;
                            break;
                        case "CreditByExam":
                            creditBasisDescription = Labels.BasisAcademicCreditByExam;
                            break;
                        case "HighSchoolTransferCredit":
                            creditBasisDescription = Labels.BasisAcademicHighSchoolTransferCredit;
                            break;
                    }

                    semester.Courses.Add(new Course
                    {
                        CourseNumber = string.Join(
                            " ",
                            course.XPathSelectElement("CourseSubjectAbbreviation")?.Value,
                            course.XPathSelectElement("CourseNumber")?.Value)
                            .Trim(),
                        CourseTitle = course.XPathSelectElement("CourseTitle")?.Value,
                        BasisForCredit = creditBasisDescription,
                        CreditValue = course.XPathSelectElement("CourseCreditValue")?.Value,
                        CreditValueEarned = course.XPathSelectElement("CourseCreditEarned")?.Value,
                        CourseGradeMark = course.XPathSelectElement("CourseSupplementalAcademicGrade/CourseSupplementalGrade/CourseAcademicSupplementalGrade")?.Value,
                        CourseGradeQualifier = course.XPathSelectElement("CourseSupplementalAcademicGrade/CourseSupplementalGrade/CourseAcademicSupplementalGradeScaleCode")?.Value,
                        CourseGradePoint = course.XPathSelectElement("CourseAcademicGrade")?.Value,
                        GradePointQualifier = course.XPathSelectElement("CourseAcademicGradeScaleCode")?.Value
                    });
                }

                Semesters.Add(semester);
            }
        }

        private string TestAndFormatDecimal(string input, CultureInfo cultureInfo)
        {
            // override with CultureInfo.InvariantCulture because A2C did not localize numbers
            cultureInfo = CultureInfo.InvariantCulture;

            if (!string.IsNullOrWhiteSpace(input) && decimal.TryParse(input, out var inputDecimal))
            {
                return string.Format(cultureInfo, "{0:#0.00}", inputDecimal);
            }

            return null;
        }

        private string TestAndFormatDate(string input, PostSecondaryTranscriptVersion version)
        {
            switch (version)
            {
                case PostSecondaryTranscriptVersion.X12:
                    // From A2C, dates are a string of 8 numbers in X12: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FUtil.xsl&version=GBmaster&line=4&lineStyle=plain&lineEnd=16&lineStartColumn=1&lineEndColumn=16
                    if (!string.IsNullOrWhiteSpace(input) && input.Length >= 8)
                    {
                        return string.Join(
                            "-",
                            input?.Substring(0, 4),
                            input?.Substring(4, 2),
                            input?.Substring(6, 2));
                    }

                    break;

                case PostSecondaryTranscriptVersion.PESC:
                    // From A2C, take first 10 characters for dates in PESC: https://ocas.visualstudio.com/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FContent%2FXSLT%2FPESCUtil.xsl&version=GBmaster&line=29&lineStyle=plain&lineEnd=34&lineStartColumn=1&lineEndColumn=16
                    return input?.Substring(0, 10);
            }

            return null;
        }
    }

    public class AcademicAward
    {
        public string Title { get; set; }
        public string Date { get; set; }
        public string ProgramName { get; set; }
    }

    public class Semester
    {
        public string Name { get; set; }
        public string StartDate { get; set; }
        public IList<Course> Courses { get; set; } = new List<Course>();
        public IList<AcademicSummary> AcademicSummaries { get; set; } = new List<AcademicSummary>();
    }

    public class AcademicSummary
    {
        public string AcademicCreditsGpa { get; set; }
        public string AcademicGpa { get; set; }
        public string RangeMinGpa { get; set; }
        public string RangeMaxGpa { get; set; }
    }

    public class Course
    {
        public string Id { get; set; }
        public string CourseNumber { get; set; }
        public string CourseTitle { get; set; }
        public string BasisForCredit { get; set; }
        public string CreditValue { get; set; }
        public string CreditValueEarned { get; set; }
        public string CourseGradeMark { get; set; }
        public string CourseGradeQualifier { get; set; }
        public string CourseGradePoint { get; set; }
        public string GradePointQualifier { get; set; }
    }

    public class Labels
    {
        public string AcademicAwardDate { get; set; }
        public string AcademicAwardProgram { get; set; }
        public string AcademicAwardSectionHeader { get; set; }
        public string AcademicAwardTitle { get; set; }
        public string AcademicCreditsInGPA { get; set; }
        public string AcademicGPA { get; set; }
        public string ApplicationNumber { get; set; }
        public string BasisAcademicCreditByExam { get; set; }
        public string BasisAcademicHighSchoolTransferCredit { get; set; }
        public string BasisAcademicRegular { get; set; }
        public string BasisforCredit { get; set; }
        public string CollegeUniversityStudentID { get; set; }
        public string ContactInformation { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string CourseGradeMark { get; set; }
        public string CourseGradePoint { get; set; }
        public string CourseGradeQualifier { get; set; }
        public string CourseNumber { get; set; }
        public string CourseTitle { get; set; }
        public string CreditValue { get; set; }
        public string CreditValueEarned { get; set; }
        public string DateOfBirth { get; set; }
        public string DateReceived { get; set; }
        public string DateSemesterStarted { get; set; }
        public string FirstName { get; set; }
        public string FormerSurname { get; set; }
        public string Gender { get; set; }
        public string GenderFemale { get; set; }
        public string GenderMale { get; set; }
        public string GenderUnreported { get; set; }
        public string GradePointQualifier { get; set; }
        public string NotesSpecialInstructions { get; set; }
        public string OEN { get; set; }
        public string OfficialTranscript { get; set; }
        public string RangeMaximumforGPA { get; set; }
        public string RangeMinimumforGPA { get; set; }
        public string SecondNameAndInitial { get; set; }
        public string Semester { get; set; }
        public string SemesterSummary { get; set; }
        public string StudentInformation { get; set; }
        public string Surname { get; set; }
        public string TranscriptInformation { get; set; }
    }
}
