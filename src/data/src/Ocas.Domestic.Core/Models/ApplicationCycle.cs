using System;

namespace Ocas.Domestic.Models
{
    public class ApplicationCycle : Model<Guid>
    {
        public string Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid StatusId { get; set; }
    }
}
