using System;
using Ocas.Domestic.Apply.Admin.Enums;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class GetApplicantBriefOptions : GetPageableOptions
    {
        public GetApplicantBriefOptions()
        {
            Page = 1;
            PageSize = 100;
            SortBy = ApplicantBriefSortBy.AccountNumber;
            SortDirection = Enums.SortDirection.Ascending;
        }

        public string AccountNumber { get; set; }
        public Guid? ApplicationCycleId { get; set; }
        public string ApplicationNumber { get; set; }
        public Guid? ApplicationStatusId { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Mident { get; set; }
        public string OntarioEducationNumber { get; set; }
        public bool? PaymentLocked { get; set; }
        public string PhoneNumber { get; set; }
        public string PreviousLastName { get; set; }
        public ApplicantBriefSortBy SortBy { get; set; }
    }
}
