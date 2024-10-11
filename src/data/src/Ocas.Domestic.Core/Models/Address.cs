using System;

namespace Ocas.Domestic.Models
{
    public class Address
    {
        public Guid? CountryId { get; set; }
        public string Country { get; set; }
        public Guid? ProvinceStateId { get; set; }
        public string ProvinceState { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public bool? Verified { get; set; }
    }
}
