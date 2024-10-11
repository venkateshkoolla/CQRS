using System;

namespace Ocas.Domestic.Apply.Models
{
    public class EducationBase
    {
        public bool? AcademicUpgrade { get; set; }
        public Guid ApplicantId { get; set; }
        public string AttendedFrom { get; set; }
        public string AttendedTo { get; set; }
        public Guid? CityId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CredentialId { get; set; }
        public bool? CurrentlyAttending { get; set; }
        public string FirstNameOnRecord { get; set; }
        public bool? Graduated { get; set; }
        public Guid? ProvinceId { get; set; }
        public Guid? InstituteTypeId { get; set; }
        public Guid? InstituteId { get; set; }
        public string InstituteName { get; set; }
        public Guid? LastGradeCompletedId { get; set; }
        public string LastNameOnRecord { get; set; }
        public Guid? LevelAchievedId { get; set; }
        public Guid? LevelOfStudiesId { get; set; }
        public string Major { get; set; }
        public string OntarioEducationNumber { get; set; }
        public string OtherCredential { get; set; }
        public string StudentNumber { get; set; }
        public decimal? TranscriptFee { get; set; }
        public MailingAddress Address { get; set; }
    }
}
