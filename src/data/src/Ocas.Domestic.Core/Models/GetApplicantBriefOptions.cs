using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetApplicantBriefOptions
    {
        public string AccountNumber { get; set; }
        public Guid? ApplicationCycleId { get; set; }
        public string ApplicationNumber { get; set; }
        public Guid? ApplicationStatusId { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Mident { get; set; }
        public string OntarioEducationNumber { get; set; }
        public bool? PaymentLocked { get; set; }
        public string PreviousLastName { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string PhoneNumber { get; set; }
        public ApplicantBriefSortBy SortBy { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
