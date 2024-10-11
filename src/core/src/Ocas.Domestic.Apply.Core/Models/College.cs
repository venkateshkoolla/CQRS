using System;
using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Models
{
    public class College
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool HasEtms { get; set; }
        public bool IsOpen { get; set; }
        public decimal? TranscriptFee { get; set; }
        public MailingAddress Address { get; set; }

        [JsonIgnore]
        public bool AllowCba { get; set; }

        [JsonIgnore]
        public bool AllowCbaMultiCollegeApply { get; set; }

        [JsonIgnore]
        public bool AllowCbaReferralCodeAsSource { get; set; }
    }
}
