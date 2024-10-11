using System;
using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Models
{
    public class ReferralPartner
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public bool AllowCba { get; set; }

        [JsonIgnore]
        public bool AllowCbaReferralCodeAsSource { get; set; }
    }
}
