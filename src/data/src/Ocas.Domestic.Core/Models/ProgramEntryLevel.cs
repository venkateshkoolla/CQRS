using System;

namespace Ocas.Domestic.Models
{
    public class ProgramEntryLevel : ProgramEntryLevelBase
    {
        public Guid Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
