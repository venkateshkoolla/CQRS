using System;

namespace Ocas.Domestic.Models
{
    public class InternationalCreditAssessment : InternationalCreditAssessmentBase
    {
        public Guid Id { get; set; }
    }

    public class InternationalCreditAssessmentBase : Auditable
    {
        public Guid ApplicantId { get; set; }
        public Guid InternationalCreditAssessmentStatusId { get; set; }
        public bool? HaveHighSchoolCourseEvaluation { get; set; }
        public string ReferenceNumber { get; set; }
        public Guid? CredentialEvaluationAgencyId { get; set; }
        public bool? HavePostSecondaryEvaluation { get; set; }
    }
}
