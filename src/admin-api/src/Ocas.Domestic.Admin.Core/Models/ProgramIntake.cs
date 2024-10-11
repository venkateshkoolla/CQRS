using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ocas.Domestic.Apply.Models;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class ProgramIntake : IAuditable
    {
        public Guid? Id { get; set; }
        public string StartDate { get; set; }
        public Guid IntakeAvailabilityId { get; set; }
        public Guid IntakeStatusId { get; set; }
        public int? EnrolmentEstimate { get; set; }
        public int? EnrolmentMax { get; set; }
        public string ExpiryDate { get; set; }
        public Guid? IntakeExpiryActionId { get; set; }
        public Guid? DefaultEntryLevelId { get; set; }
        public IList<Guid> EntryLevelIds { get; set; }

        [JsonProperty(PropertyName = "_canDelete")]
        public bool CanDelete { get; set; }


        // In collegeadmin, replaced by ModifiedOn
        public string ModifiedDate { get; set; }

        [JsonIgnore]
        public DateTime ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
    }
}
