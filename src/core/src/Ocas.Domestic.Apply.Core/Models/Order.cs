using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public IList<OrderDetail> Details { get; set; }
    }
}
