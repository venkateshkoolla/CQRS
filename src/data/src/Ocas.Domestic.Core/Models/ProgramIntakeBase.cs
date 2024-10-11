using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class ProgramIntakeBase : Auditable
    {
        public Guid ProgramId { get; set; }
        public Guid? AvailabilityId { get; set; }
        public string StartDate { get; set; }
        public string Name { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? EnrolmentProjection { get; set; }
        public int? EnrolmentMaximum { get; set; }
        public Guid? ExpiryActionId { get; set; }
        public Guid? ProgramIntakeStatusId { get; set; }
        public Guid? DefaultEntrySemesterId { get; set; }
        public bool? HasSemesterOverride { get; set; }
        public State State { get; set; }
        public Status Status { get; set; }
    }
}
