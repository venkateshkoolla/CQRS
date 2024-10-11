using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Models.Templates
{
    public class HighSchoolGradesViewModel
    {
        public string Literacy { get; set; }
        public string CommunityInvolvement { get; set; }
        public string HighestEducation { get; set; }
        public string DateCredentialAchieved { get; set; }
        public IList<string> SchoolsAttended { get; set; }
        public string HighSkillsMajor { get; set; }
        public string TotalCredits { get; set; }
        public IList<HighSchoolGradeViewModel> Grades { get; set; }

        public HighSchoolGradesLabels Labels { get; set; }

        public TranscriptFooterViewModel FooterViewModel { get; set; }

        public void LoadTranslations(TranslationsDictionary translationsDictionary)
        {
            Labels = new HighSchoolGradesLabels
            {
                OntarioHighSchoolInformation = translationsDictionary.Get("transcript.high_school.ontario_high_school_information"),
                AcademicData = translationsDictionary.Get("transcript.high_school.academic_data"),
                Literacy = translationsDictionary.Get("transcript.high_school.literacy_test"),
                CommunityInvolvement = translationsDictionary.Get("transcript.high_school.community_involvement"),
                HighestEducation = translationsDictionary.Get("transcript.high_school.highest_education"),
                DateCredentialAchieved = translationsDictionary.Get("transcript.high_school.date_credential_achieved"),
                SchoolsAttended = translationsDictionary.Get("transcript.high_school.schools_attended"),
                HighSkillsMajor = translationsDictionary.Get("transcript.high_school.specialist_high_skills_major"),
                OntarioHighSchoolGrades = translationsDictionary.Get("transcript.high_school.ontario_high_school_grades"),
                TotalCreditsToDate = translationsDictionary.Get("transcript.high_school.total_credits_to_date"),
                CourseCode = translationsDictionary.Get("transcript.high_school.course_code"),
                CompletedDate = translationsDictionary.Get("transcript.high_school.completion_date"),
                Mark = translationsDictionary.Get("transcript.high_school.mark"),
                MarkType = translationsDictionary.Get("transcript.high_school.mark_type"),
                Credit = translationsDictionary.Get("transcript.high_school.credit"),
                CourseStatus = translationsDictionary.Get("transcript.high_school.course_status"),
                DeliveryType = translationsDictionary.Get("transcript.high_school.delivery_type"),
                CourseType = translationsDictionary.Get("transcript.high_school.course_type"),
                Notes = translationsDictionary.Get("transcript.high_school.notes"),
                MidentCode = translationsDictionary.Get("transcript.high_school.mident_code"),
                NoGrades = translationsDictionary.Get("transcript.high_school.no_grades"),
                OntarioHighSchoolTranscript = translationsDictionary.Get("transcript.high_school.ontario_high_school_transcript")
            };
            FooterViewModel = new TranscriptFooterViewModel(translationsDictionary);
        }
    }

    public class HighSchoolGradeViewModel
    {
        public string CourseCode { get; set; }
        public string CompletedDate { get; set; }
        public string Mark { get; set; }
        public string MarkType { get; set; }
        public string Credit { get; set; }
        public string CourseStatus { get; set; }
        public string DeliveryType { get; set; }
        public string CourseType { get; set; }
        public string Notes { get; set; }
        public string MidentCode { get; set; }
    }

    public class HighSchoolGradesLabels
    {
        public string OntarioHighSchoolInformation { get; set; }
        public string AcademicData { get; set; }
        public string Literacy { get; set; }
        public string CommunityInvolvement { get; set; }
        public string HighestEducation { get; set; }
        public string DateCredentialAchieved { get; set; }
        public string SchoolsAttended { get; set; }
        public string HighSkillsMajor { get; set; }
        public string OntarioHighSchoolGrades { get; set; }
        public string OntarioHighSchoolTranscript { get; set; }
        public string TotalCreditsToDate { get; set; }
        public string CourseCode { get; set; }
        public string CompletedDate { get; set; }
        public string Mark { get; set; }
        public string MarkType { get; set; }
        public string Credit { get; set; }
        public string CourseStatus { get; set; }
        public string DeliveryType { get; set; }
        public string CourseType { get; set; }
        public string Notes { get; set; }
        public string MidentCode { get; set; }
        public string NoGrades { get; set; }
    }
}
