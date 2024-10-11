using System;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class IntakeApplicant
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
