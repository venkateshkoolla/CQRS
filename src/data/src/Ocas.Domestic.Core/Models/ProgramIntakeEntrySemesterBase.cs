using System;

namespace Ocas.Domestic.Models
{
    public class ProgramIntakeEntrySemesterBase
    {
        public string Name { get; set; }
        public Guid ProgramIntakeId { get; set; }
        public Guid EntrySemesterId { get; set; }
    }
}
