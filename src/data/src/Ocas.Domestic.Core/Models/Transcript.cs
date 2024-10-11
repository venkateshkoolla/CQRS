using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class Transcript : TranscriptBase
    {
        public Guid Id { get; set; }
    }

    public class TranscriptBase : Auditable
    {
        public Guid? OriginalStudentId { get; set; }
        public TranscriptType TranscriptType { get; set; }
        public string Name { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? ContactId { get; set; }
        public string Credentials { get; set; }
        public Guid? EtmsTranscriptId { get; set; }
        public Guid? SupportingDocumentId { get; set; }
        public Guid? TranscriptSourceId { get; set; }
    }
}
