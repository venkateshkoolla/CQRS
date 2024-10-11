using System;

namespace Ocas.Domestic.Models
{
    public class City : Model<Guid>
    {
        public Guid ProvinceId { get; set; }
    }
}
