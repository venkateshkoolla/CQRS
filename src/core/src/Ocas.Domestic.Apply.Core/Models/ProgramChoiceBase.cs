using System;

namespace Ocas.Domestic.Apply.Models
{
    public class ProgramChoiceBase
    {
        public Guid ApplicationId { get; set; }
        public Guid ApplicantId { get; set; }
        public Guid IntakeId { get; set; }
        public Guid EntryLevelId { get; set; }
        public int? PreviousYearAttended { get; set; }
        public int? PreviousYearApplied { get; set; }
        public string EffectiveDate { get; set; }
    }
}
