using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class Contact : ContactBase
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; }
        public DateTime? LastUsedInCampaign { get; set; }
        public string PreviousLastName { get; set; }
        public Guid? FirstGenerationId { get; set; }
        public Guid? FirstLanguageId { get; set; }
        public Guid? GenderId { get; set; }
        public Guid? TitleId { get; set; }
        public CompletedSteps? CompletedSteps { get; set; }
        public Guid? PreferredCorrespondenceMethodId { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public Address MailingAddress { get; set; }
        public Guid? CountryOfBirthId { get; set; }
        public Guid? CountryOfCitizenshipId { get; set; }
        public Guid? SponsorAgencyId { get; set; }
        public Guid? StatusInCanadaId { get; set; }
        public Guid? StatusOfVisaId { get; set; }
        public DateTime? DateOfArrival { get; set; }
        public bool? IsAboriginalPerson { get; set; }
        public Guid? AboriginalStatusId { get; set; }
        public string OtherAboriginalStatus { get; set; }
        public bool? HighSchoolEnrolled { get; set; }
        public bool? HighSchoolGraduated { get; set; }
        public DateTime? HighSchoolGraduationDate { get; set; }
        public string OntarioEducationNumber { get; set; }
        public bool? OntarioEducationNumberLock { get; set; }
        public decimal? OverPayment { get; set; }
        public decimal? Balance { get; set; }
        public decimal? NsfBalance { get; set; }
        public decimal? ReturnedPayment { get; set; }
        public bool PaymentLocked { get; set; }
        public Guid? IntlEvaluatorId { get; set; }
        public string IntlReferenceNumber { get; set; }
    }
}
