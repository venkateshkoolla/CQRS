using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Models
{
    public class ProgramChoice : ProgramChoiceBase, IAuditable
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public Guid? OfferStatusId { get; set; }
        public string IntakeStartDate { get; set; }
        public Guid? ProgramId { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public Guid? CampusId { get; set; }
        public string CampusName { get; set; }
        public Guid? CollegeId { get; set; }
        public string CollegeName { get; set; }
        public Guid? DeliveryId { get; set; }
        public string SupplementalFeeDescription { get; set; }
        public IList<Guid> EligibleEntryLevelIds { get; set; }
        public Guid SourceId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
