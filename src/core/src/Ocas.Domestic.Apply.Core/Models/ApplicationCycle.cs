using System;

namespace Ocas.Domestic.Apply.Models
{
    public class ApplicationCycle
    {
        public Guid Id { get; set; }
        public string Year { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Status { get; set; }
        public DateTime EqualConsiderationDate { get; set; }
    }
}
