using System;

namespace Ocas.Domestic.Apply.Models
{
    public class Offer : Auditable
    {
        public Guid Id { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ApplicationStatusId { get; set; }
        public Guid ApplicationCycleId { get; set; }
        public string SoftExpiryDate { get; set; }
        public bool IsSoftExpired { get; set; }
        public string HardExpiryDate { get; set; }
        public bool IsHardExpired { get; set; }
        public string StartDate { get; set; }
        public int SequenceNumber { get; set; }
        public string TermIdentifier { get; set; }
        public bool IsLateAdmit { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? OfferLockReleaseDate { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public Guid OfferStateId { get; set; }
        public Guid OfferStatusId { get; set; }
        public Guid StudyMethodId { get; set; }
        public Guid OfferTypeId { get; set; }
        public Guid EntryLevelId { get; set; }
        public Guid IntakeId { get; set; }
        public Guid ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramName { get; set; }
        public string CampusName { get; set; }
        public Guid CollegeId { get; set; }
        public bool CanRespond { get; set; }
    }
}
