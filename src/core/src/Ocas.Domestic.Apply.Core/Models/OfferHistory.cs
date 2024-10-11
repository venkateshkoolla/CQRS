using System;

namespace Ocas.Domestic.Apply.Models
{
    public class OfferHistory
    {
        public Guid Id { get; set; }
        public Guid? ApplicantId { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public string CollegeName { get; set; }
        public string ProgramTitle { get; set; }
        public string ProgramCode { get; set; }
        public Guid? ProgramId { get; set; }
        public string CollegeId { get; set; }
        public string CampusId { get; set; }
        public Guid? EntryLevelId { get; set; }
        public Guid? ProgramDeliveryId { get; set; }
        public string CampusName { get; set; }
        public string StartDate { get; set; }
        public string EntryLevelCode { get; set; }
        public string LocalizedProgramDelivery { get; set; }
        public Guid? OfferStatusId { get; set; }
        public string LocalizedOfferStatusDescription { get; set; }
        public Guid? ApplicationCycleId { get; set; }
        public Guid? ApplicationId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
