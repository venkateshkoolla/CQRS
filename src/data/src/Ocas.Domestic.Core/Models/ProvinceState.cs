using System;

namespace Ocas.Domestic.Models
{
    public class ProvinceState : Model<Guid>
    {
        public Guid CountryId { get; set; }
    }
}
