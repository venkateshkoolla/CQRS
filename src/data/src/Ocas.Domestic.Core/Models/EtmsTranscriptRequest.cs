using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class EtmsTranscriptRequest : Auditable
    {
        public Guid Id { get; set; }
        public EtmsRequestType EtmsRequestType { get; set; }
        public string AccountNumber { get; set; }
        public string ApplicationNumber { get; set; }
        public string InstitutionName { get; set; }
        public string CampusName { get; set; }
        public DateTime? DateLastAttended { get; set; }
        public bool? Graduated { get; set; }
        public string LegalFirstNameInFinalYearOfHighSchool { get; set; }
        public string LegalSurnameInFinalYearOfHighSchool { get; set; }
        public string StudentNumber { get; set; }
        public string LanguageOfInstruction { get; set; }
        public int? LastGradeCompleted { get; set; }
        public string LevelOfStudy { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string FormerSurname { get; set; }
        public Guid? GenderId { get; set; }
        public string LegalFirstGivenName { get; set; }
        public string LegalLastFamilyName { get; set; }
        public string MiddleName { get; set; }
        public string OEN { get; set; }
        public string PhoneNumber { get; set; }
        public string ProgramName { get; set; }
        public string Title { get; set; }
    }
}
