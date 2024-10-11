using System;
using Ocas.Domestic.Apply.Enums;

namespace Ocas.Domestic.Apply.Models
{
    public class OrderDetail
    {
        public Guid Id { get; set; }
        public ShoppingCartItemType Type { get; set; }
        public string ContextId { get; set; }
        public decimal? Amount { get; set; }
    }
}
