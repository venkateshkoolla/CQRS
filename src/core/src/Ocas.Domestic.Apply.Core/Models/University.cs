using System;
using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Models
{
    public class University
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool HasEtms { get; set; }
        public Guid SchoolStatusId { get; set; }
        public decimal? TranscriptFee { get; set; }
        public MailingAddress Address { get; set; }

        [JsonIgnore]
        public bool ShowInEducation { get; set; }
    }
}
