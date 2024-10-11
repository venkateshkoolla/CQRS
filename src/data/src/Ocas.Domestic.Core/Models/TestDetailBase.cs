using System;

namespace Ocas.Domestic.Models
{
    public class TestDetailBase
    {
        public Guid TestId { get; set; }
        public string Description { get; set; }
        public int? Grade { get; set; }
        public int? Percentile { get; set; }
    }
}
