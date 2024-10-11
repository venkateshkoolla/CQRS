using System;

namespace Ocas.Domestic.Apply.Models
{
    public class Application : ApplicationBase, IAuditable
    {
        public Guid Id { get; set; }
        public string ApplicationNumber { get; set; }
        public Guid ApplicationStatusId { get; set; }
        public string EffectiveDate { get; set; }
        public bool ProgramsComplete { get; set; }
        public bool TranscriptsComplete { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class ApplicationBase
    {
        public Guid ApplicantId { get; set; }
        public Guid ApplicationCycleId { get; set; }
    }
}
