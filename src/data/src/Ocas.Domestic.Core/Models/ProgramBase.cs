using System;

namespace Ocas.Domestic.Models
{
    public class ProgramBase : Auditable
    {
        public Guid ApplicationCycleId { get; set; }
        public Guid CollegeApplicationCycleId { get; set; }
        public Guid CollegeId { get; set; }
        public Guid CampusId { get; set; }
        public string LocalizedName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public Guid DeliveryId { get; set; }
        public Guid ProgramTypeId { get; set; }
        public decimal Length { get; set; }
        public Guid LengthTypeId { get; set; }
        public Guid McuCodeId { get; set; }
        public Guid CredentialId { get; set; }
        public Guid DefaultEntryLevelId { get; set; }
        public Guid StudyAreaId { get; set; }
        public Guid HighlyCompetitiveId { get; set; }
        public Guid LanguageId { get; set; }
        public Guid LevelId { get; set; }
        public Guid PromotionId { get; set; }
        public Guid AdultTrainingId { get; set; }
        public Guid? SpecialCodeId { get; set; }
        public int ApsNumber { get; set; }
        public Guid MinistryApprovalId { get; set; }
        public string Url { get; set; }
        public Guid ProgramCategory1Id { get; set; }
        public Guid ProgramSubCategory1Id { get; set; }
        public Guid? ProgramCategory2Id { get; set; }
        public Guid? ProgramSubCategory2Id { get; set; }
    }
}
