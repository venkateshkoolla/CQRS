using System;

namespace Ocas.Domestic.Apply.Models
{
    public class TranscriptRequest : TranscriptRequestBase, IAuditable
    {
        public Guid Id { get; set; }
        public Guid? FromInstituteTypeId { get; set; }
        public string FromInstituteName { get; set; }
        public string ToInstituteName { get; set; }
        public Guid? RequestStatusId { get; set; }
        public decimal? Amount { get; set; }
        public Guid? EtmsTranscriptRequestId { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
