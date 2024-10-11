using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.TestFramework.Models
{
    public class ValidEntryLevel
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }
    }
}
