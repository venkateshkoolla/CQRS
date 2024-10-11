using System;

namespace Ocas.Domestic.Models
{
    public class Application : ApplicationBase
    {
        public Guid Id { get; set; }
        public string ApplicationNumber { get; set; }
        public bool? BasisForAdmissionLock { get; set; }
        public bool? CurrentLock { get; set; }
        public int ShoppingCartStatus { get; set; }
        public Guid? CurrentId { get; set; }
        public Guid? BasisForAdmissionId { get; set; }
        public decimal? Balance { get; set; }
        public int? CompletedSteps { get; set; }
    }

    public class ApplicationBase : Auditable
    {
        public Guid ApplicationStatusId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public Guid ApplicantId { get; set; }
        public Guid ApplicationCycleId { get; set; }
    }
}
