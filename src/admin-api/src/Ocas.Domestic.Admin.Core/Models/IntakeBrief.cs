using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Admin.Models
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class IntakeBrief
    {
        public Guid Id { get; set; }
        public Guid? ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramTitle { get; set; }
        public Guid? DeliveryId { get; set; }
        public Guid? CollegeId { get; set; }
        public string CollegeName { get; set; }
        public Guid? CampusId { get; set; }
        public string CampusName { get; set; }
        public string StartDate { get; set; }
        public Guid? IntakeStatusId { get; set; }
        public Guid? IntakeAvailabilityId { get; set; }
        public IList<Guid> EligibleEntryLevelIds { get; set; }

        [JsonIgnore]
        public string Delivery { get; set; }

        [JsonIgnore]
        public string IntakeStatus { get; set; }

        [JsonIgnore]
        public string IntakeAvailability { get; set; }
    }
}
