using System;
using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class ProgramBrief
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public Guid? DeliveryId { get; set; }
        public Guid CollegeId { get; set; }
        public Guid CampusId { get; set; }

        [JsonIgnore]
        public string Delivery { get; set; }

        [JsonIgnore]
        public string College { get; set; }

        [JsonIgnore]
        public string Campus { get; set; }
    }
}
