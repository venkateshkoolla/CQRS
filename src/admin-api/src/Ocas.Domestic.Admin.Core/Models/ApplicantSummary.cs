using System.Collections.Generic;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class ApplicantSummary
    {
        public Applicant Applicant { get; set; }
        public AcademicRecord AcademicRecord { get; set; }
        public IList<Education> Educations { get; set; }
        public IList<OntarioStudentCourseCredit> OntarioStudentCourseCredits { get; set; }
        public IList<SupportingDocument> SupportingDocuments { get; set; }
        public IList<ApplicationSummary> ApplicationSummaries { get; set; }
    }
}
