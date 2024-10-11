using System;
using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Models
{
    public class City
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public Guid ProvinceId { get; set; }

        [JsonIgnore]
        public string Name { get; set; }
    }
}
