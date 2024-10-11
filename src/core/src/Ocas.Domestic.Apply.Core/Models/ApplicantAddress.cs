using System;
using Newtonsoft.Json;

namespace Ocas.Domestic.Apply.Models
{
    public class ApplicantAddress
    {
        public Guid? CountryId { get; set; }

        [JsonIgnore]
        public string Country { get; set; }

        public Guid? ProvinceStateId { get; set; }

        [JsonIgnore]
        public string ProvinceState { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public bool Verified { get; set; }
    }
}
