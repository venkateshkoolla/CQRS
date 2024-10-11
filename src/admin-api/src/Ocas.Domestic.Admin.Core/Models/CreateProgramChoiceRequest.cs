using System;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class CreateProgramChoiceRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid ProgramId { get; set; }
        public string StartDate { get; set; }
        public Guid EntryLevelId { get; set; }
    }
}
