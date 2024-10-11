using System;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class IntakeExport
    {
        public Guid IntakeId { get; set; }
        public string ApplicationCycle { get; set; }
        public string CollegeCode { get; set; }
        public string CampusCode { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramTitle { get; set; }
        public string ProgramDelivery { get; set; }
        public string StartDate { get; set; }
        public string ProgramIntakeAvailability { get; set; }
        public string ProgramIntakeStatus { get; set; }
        public string ExpiryDate { get; set; }
        public string Expiration { get; set; }
        public string HasSemesterOverride { get; set; }
        public string DefaultSemesterOverride { get; set; }
    }
}
