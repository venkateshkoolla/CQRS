using System;

namespace Ocas.Domestic.Models
{
    public class CollegeApplicationCycle : Model<Guid>
    {
        public Guid CollegeId { get; set; }
        public Guid ApplicationCycleId { get; set; }
    }
}
