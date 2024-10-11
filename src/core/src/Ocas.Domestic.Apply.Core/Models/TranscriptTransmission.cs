using System;

namespace Ocas.Domestic.Apply.Models
{
    public class TranscriptTransmission : Lookups.LookupItem
    {
        public Guid? ApplicationCycleId { get; set; }
        public Guid? InstituteTypeId { get; set; }
        public DateTime? EligibleUntil { get; set; }
    }
}
