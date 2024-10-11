using System;

namespace Ocas.Domestic.Apply.Models
{
    public class TranscriptRequestBase
    {
        public Guid ApplicationId { get; set; }
        public Guid FromInstituteId { get; set; }
        public Guid? ToInstituteId { get; set; }
        public Guid TransmissionId { get; set; }
        public Guid? EducationId { get; set; }
    }
}
