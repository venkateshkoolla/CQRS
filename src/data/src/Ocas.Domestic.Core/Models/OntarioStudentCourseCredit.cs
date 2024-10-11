using System;

namespace Ocas.Domestic.Models
{
    public class OntarioStudentCourseCredit : OntarioStudentCourseCreditBase
    {
        public Guid Id { get; set; }
    }

    public class OntarioStudentCourseCreditBase : Auditable
    {
        public Guid ApplicantId { get; set; }
        public string CompletedDate { get; set; }
        public string CourseCode { get; set; }
        public string CourseMident { get; set; }
        public decimal? Credit { get; set; }
        public string Grade { get; set; }
        public string Notes { get; set; }
        public Guid? TranscriptId { get; set; }
        public Guid? CourseStatusId { get; set; }
        public Guid? CourseTypeId { get; set; }
        public Guid? GradeTypeId { get; set; }
        public Guid? CourseDeliveryId { get; set; }
    }
}
