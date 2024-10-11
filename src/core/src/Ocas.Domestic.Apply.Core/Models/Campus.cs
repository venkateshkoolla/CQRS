using System;

namespace Ocas.Domestic.Apply.Models
{
    public class Campus
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid CollegeId { get; set; }
    }
}
