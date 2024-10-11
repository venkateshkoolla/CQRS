using System;

namespace Ocas.Domestic.Models
{
    public class ProgramEntryLevelBase
    {
        public string Name { get; set; }
        public Guid ProgramId { get; set; }
        public Guid EntryLevelId { get; set; }
    }
}
