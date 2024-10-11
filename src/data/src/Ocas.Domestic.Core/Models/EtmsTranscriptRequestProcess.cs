using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class EtmsTranscriptRequestProcess : Auditable
    {
        public Guid Id { get; set; }
        public Guid EtmsTranscriptRequestid { get; set; }
        public DateTime? ProcessStartDate { get; set; }
        public Guid? TranscriptRequestStatusId { get; set; }
        public EtmsProcessType? EtmsProcessType { get; set; }
    }
}
