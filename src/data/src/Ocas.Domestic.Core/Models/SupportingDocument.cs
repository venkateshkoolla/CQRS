using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class SupportingDocument
    {
        public Guid Id { get; set; }
        public string BatchName { get; set; }
        public Guid? ApplicantId { get; set; }
        public Guid? OfficialSignatureId { get; set; }
        public DateTime? LandedOnDate { get; set; }
        public Guid? CredentialCodeId { get; set; }
        public string BoxNumber { get; set; }
        public Guid? CompleteId { get; set; }
        public string Level { get; set; }
        public SupportingDocumentAvailability Availability { get; set; }
        public string Name { get; set; }
        public Guid? OriginalId { get; set; }
        public Guid? TemporaryId { get; set; }
        public string Surname { get; set; }
        public Guid? OfficialId { get; set; }
        public DateTime? GraduationDate { get; set; }
        public int? CicClient { get; set; }
        public Guid? AgencyId { get; set; }
        public Guid? ProgramLevelId { get; set; }
        public Guid? DocumentTypeId { get; set; }
        public Guid? ScanUserId { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? ScanDate { get; set; }
        public Guid? LevelAchievedId { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? IssueDate { get; set; }
        public Guid? InstituteId { get; set; }
        public Guid? ProvinceId { get; set; }
        public string Class { get; set; }
        public DateTime? KeyDate { get; set; }
        public Guid? DocumentSourceId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
