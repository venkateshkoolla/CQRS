using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Models
{
    public class ProgramIntake : ProgramIntakeBase
    {
        public Guid Id { get; set; }
        public Guid CollegeApplicationCycleId { get; set; }
        public Guid ApplicationCycleId { get; set; }
        public Guid? PromotionId { get; set; }
        public IList<Guid> EntryLevels { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramTitle { get; set; }
        public Guid ProgramDeliveryId { get; set; }
        public Guid CollegeId { get; set; }
        public Guid CampusId { get; set; }
    }
}
