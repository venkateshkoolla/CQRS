using System;

namespace Ocas.Domestic.Models
{
    public class ProgramApplication
    {
        public Guid ApplicationId { get; set; }
        public Guid IntakeId { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public string ApplicationNumber { get; set; }
        public Guid ApplicationStatusId { get; set; }
    }
}