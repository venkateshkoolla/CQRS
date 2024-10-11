using System;

namespace Ocas.Domestic.Models
{
    public class GetOrderOptions
    {
        public Guid? Id { get; set; }
        public string OrderNumber { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
