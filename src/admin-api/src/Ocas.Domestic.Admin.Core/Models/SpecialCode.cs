using System;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class SpecialCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        // Actually CollegeApplicationCycleId but keeping name from collegeadmin-api
        public Guid ApplicationCycleId { get; set; }
    }
}
