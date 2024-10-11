using System;
using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class OntarioStudentCourseCredit : OntarioStudentCourseCreditBase, IAuditable
    {
        public Guid Id { get; set; }
        public Guid TranscriptId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class OntarioStudentCourseCreditBase
    {
        public Guid ApplicantId { get; set; }
        public string CompletedDate { get; set; }
        public string CourseCode { get; set; }
        public string CourseMident { get; set; }
        public decimal Credit { get; set; }
        public string Grade { get; set; }
        public IList<string> Notes { get; set; }
        public Guid CourseDeliveryId { get; set; }
        public Guid CourseStatusId { get; set; }
        public Guid CourseTypeId { get; set; }
        public Guid GradeTypeId { get; set; }
        public string SupplierMident { get; set; }
    }
}