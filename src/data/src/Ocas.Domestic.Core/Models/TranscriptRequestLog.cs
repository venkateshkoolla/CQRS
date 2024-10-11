using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class TranscriptRequestLog : TranscriptRequestLogBase
    {
        public Guid Id { get; set; }
    }

    public class TranscriptRequestLogBase : Auditable
    {
        public string Name { get; set; }
        public Guid? OrderId { get; set; }
        public ProcessStatus? ProcessStatus { get; set; }
        public DateTime? RequestTimestamp { get; set; }
        public string ServiceRequest { get; set; }
        public string ServiceResponse { get; set; }
        public string ServiceResponseCode { get; set; }
        public Guid? TranscriptRequestStatusId { get; set; }
    }
}
