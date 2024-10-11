using System;

namespace Ocas.Domestic.Models
{
    public class ApplicantBrief
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public string ApplicationNumber { get; set; }
        public Guid? ApplicationStatusId { get; set; }
        public DateTime BirthDate { get; set; }
        public Guid? CountryOfCitizenshipId { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string MobilePhone { get; set; }
        public string OntarioEducationNumber { get; set; }
        public bool PaymentLocked { get; set; }
        public string PreferredName { get; set; }
        public string PreviousLastName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
