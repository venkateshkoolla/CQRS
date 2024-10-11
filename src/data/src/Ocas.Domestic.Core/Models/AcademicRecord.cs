using System;

namespace Ocas.Domestic.Models
{
    public class AcademicRecord : AcademicRecordBase
    {
        public Guid Id { get; set; }
    }

    public class AcademicRecordBase : Auditable
    {
        public Guid ApplicantId { get; set; }
        public Guid? CommunityInvolvementId { get; set; }
        public DateTime? DateCredentialAchieved { get; set; }
        public Guid? HighestEducationId { get; set; }
        public Guid? HighSkillsMajorId { get; set; }
        public Guid? LiteracyTestId { get; set; }
        public string Mident { get; set; }
        public string Name { get; set; }
        public Guid? SchoolId { get; set; }
        public Guid? ShsmCompletionId { get; set; }
        public Guid? StudentId { get; set; }
    }
}
