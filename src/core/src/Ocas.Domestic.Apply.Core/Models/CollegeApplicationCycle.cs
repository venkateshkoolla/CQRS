using System;

namespace Ocas.Domestic.Apply.Models
{
    public class CollegeApplicationCycle
    {
        public Guid Id { get; set; }
        public Guid MasterId { get; set; }
        public Guid CollegeId { get; set; }
        public string Year { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Guid StatusId { get; set; }
        public bool EnableIntakeOverride { get; set; }
    }
}
