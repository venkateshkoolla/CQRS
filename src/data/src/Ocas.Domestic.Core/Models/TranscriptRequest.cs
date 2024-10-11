using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class TranscriptRequest : TranscriptRequestBase
    {
        public Guid Id { get; set; }
    }

    public class TranscriptRequestBase : Auditable
    {
        public Guid? ApplicantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? EducationId { get; set; }
        public Guid? EtmsTranscriptRequestId { get; set; }
        public Guid FromSchoolId { get; set; }
        public string FromSchoolName { get; set; }
        public TranscriptSchoolType? FromSchoolType { get; set; }
        public string Name { get; set; }
        public Guid? PeteRequestLogId { get; set; }
        public Guid? ToSchoolId { get; set; }
        public string ToSchoolName { get; set; }
        public Guid? TranscriptTransmissionId { get; set; }
        public Guid? TranscriptRequestStatusId { get; set; }
        public decimal? TranscriptFee { get; set; }
    }
}
