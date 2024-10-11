using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Models
{
    public class Applicant : ApplicantBase
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string AccountNumber { get; set; }
        public string PreviousLastName { get; set; }
        public string PreferredName { get; set; }
        public string Email { get; set; }
        public Guid AccountStatusId { get; set; }
        public Guid? CountryOfBirthId { get; set; }
        public Guid? CountryOfCitizenshipId { get; set; }
        public Guid? GenderId { get; set; }
        public Guid? FirstGenerationId { get; set; }
        public Guid? FirstLanguageId { get; set; }
        public Guid PreferredLanguageId { get; set; }
        public Guid? SponsorAgencyId { get; set; }
        public Guid? StatusInCanadaId { get; set; }
        public Guid? StatusOfVisaId { get; set; }
        public Guid? TitleId { get; set; }
        public Guid? AcceptedPrivacyStatementId { get; set; }
        public bool? AgreedToCasl { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public bool? IsAboriginalPerson { get; set; }
        public IList<Guid> AboriginalStatuses { get; set; }
        public string OtherAboriginalStatus { get; set; }
        public ApplicantAddress MailingAddress { get; set; }
        public string DateOfArrival { get; set; }
        public bool? EnrolledInHighSchool { get; set; }
        public bool? GraduatedHighSchool { get; set; }
        public string GraduationHighSchoolDate { get; set; }
        public string OntarioEducationNumber { get; set; }
        public bool PaymentLocked { get; set; }
        public Guid? IntlEvaluatorId { get; set; }
        public string IntlReferenceNumber { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool LastLoginExceed { get; set; }
        public string Source { get; set; }

        [JsonIgnore]
        public string SubjectId { get; set; }
    }

    public class ApplicantBase
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
    }
}