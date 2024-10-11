using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Models
{
    public class Country : Lookups.LookupItem
    {
        [JsonIgnore]
        public string Name { get; set; }
    }
}
