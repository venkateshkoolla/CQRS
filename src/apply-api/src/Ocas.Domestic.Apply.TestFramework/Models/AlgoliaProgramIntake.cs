using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Apply.TestFramework.Models
{
    public class AlgoliaProgramIntake
    {
        public Guid ApplicationCycleId { get; set; }

        public Guid IntakeId { get; set; }

        public string CollegeCode { get; set; }

        public string AlternateCollegeName { get; set; }

        public Guid ProgramId { get; set; }

        public string ProgramCode { get; set; }

        public Guid ProgramDeliveryId { get; set; }

        public string IntakeStartDate { get; set; }

        public List<Guid> ProgramValidEntryLevelIds { get; set; }

        public Guid ProgramEntryLevelId { get; set; }

        public Guid? CampusId { get; set; }

        public Guid CollegeId { get; set; }
    }
}
