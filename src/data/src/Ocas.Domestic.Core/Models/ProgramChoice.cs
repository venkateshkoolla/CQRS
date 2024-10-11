using System;

namespace Ocas.Domestic.Models
{
    public class ProgramChoice : ProgramChoiceBase
    {
        public Guid Id { get; set; }
        public bool? WithdrawalDueToClosure { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public Guid? OfferStatusId { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public string Pathway { get; set; }

        public string IntakeStartDate { get; set; }
        public Guid? ProgramId { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public Guid? CampusId { get; set; }
        public string CampusName { get; set; }
        public string CampusCode { get; set; }
        public Guid? CollegeId { get; set; }
        public string CollegeName { get; set; }
        public string CollegeCode { get; set; }
        public Guid? DeliveryId { get; set; }
        public bool? SupplementalFeePaid { get; set; }
    }

    public class ProgramChoiceBase : Auditable
    {
        public Guid ApplicationId { get; set; }
        public Guid ApplicantId { get; set; }
        public Guid ProgramIntakeId { get; set; }
        public Guid EntryLevelId { get; set; }
        public int? PreviousYearAttended { get; set; }
        public int? PreviousYearApplied { get; set; }
        public string Name { get; set; }
        public int SequenceNumber { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public Guid SourceId { get; set; }
    }
}
