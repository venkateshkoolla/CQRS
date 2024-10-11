using System.Collections.Generic;

namespace Ocas.Domestic.Models
{
    public class ApplicantSummary
    {
        public Contact Contact { get; set; }
        public IList<Education> Educations { get; set; }
        public IList<SupportingDocument> SupportingDocuments { get; set; }
        public IList<Transcript> Transcripts { get; set; }
        public IList<Test> Tests { get; set; }
        public IList<AcademicRecord> AcademicRecords { get; set; }
        public IList<OntarioStudentCourseCredit> OntarioStudentCourseCredits { get; set; }
        public IList<ApplicationSummary> ApplicationSummaries { get; set; }
    }
}
