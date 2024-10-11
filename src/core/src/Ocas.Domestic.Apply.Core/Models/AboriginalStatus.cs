using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Models
{
    public class AboriginalStatus : Lookups.LookupItem
    {
        [JsonIgnore]
        public string ColtraneCode { get; set; }

        [JsonIgnore]
        public bool ShowInPortal { get; set; }
    }
}
