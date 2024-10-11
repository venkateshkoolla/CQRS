using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class Program : ProgramBase
    {
        public Guid Id { get; set; }
    }

    public class ProgramBase : IAuditable
    {
        // this is actually CollegeApplicationCycleId
        public Guid ApplicationCycleId { get; set; }
        public Guid CollegeId { get; set; }
        public Guid CampusId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public Guid DeliveryId { get; set; }
        public Guid ProgramTypeId { get; set; }
        public decimal Length { get; set; }
        public Guid LengthTypeId { get; set; }
        public string McuCode { get; set; }
        public Guid CredentialId { get; set; }
        public Guid DefaultEntryLevelId { get; set; }
        public IList<Guid> EntryLevelIds { get; set; }
        public Guid StudyAreaId { get; set; }
        public Guid HighlyCompetitiveId { get; set; }
        public Guid LanguageId { get; set; }
        public Guid LevelId { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? AdultTrainingId { get; set; }
        public string SpecialCode { get; set; }
        public int ApsNumber { get; set; }
        public Guid MinistryApprovalId { get; set; }
        public string Url { get; set; }
        public Guid ProgramCategory1Id { get; set; }
        public Guid ProgramSubCategory1Id { get; set; }
        public Guid? ProgramCategory2Id { get; set; }
        public Guid? ProgramSubCategory2Id { get; set; }
        public IList<ProgramIntake> Intakes { get; set; }

        // In collegeadmin, replaced by ModifiedOn
        public string ModifiedDate { get; set; }

        [JsonIgnore]
        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
