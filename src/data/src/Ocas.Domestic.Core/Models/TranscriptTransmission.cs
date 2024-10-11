using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class TranscriptTransmission : Model<Guid>
    {
        public Guid? ApplicationCycleId { get; set; }
        public InstitutionType? InstitutionType { get; set; }
        public DateTime? TermDueDate { get; set; }
        public DateTime? TermEndDate { get; set; }
        public DateTime? TermStartDate { get; set; }
    }
}
