using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class Product : Model<Guid>
    {
        public ProductServiceType ProductServiceType { get; set; }
        public decimal Amount { get; set; }
    }
}
