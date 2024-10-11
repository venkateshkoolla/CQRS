using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class OrderDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public string Description { get; set; }
        public ShoppingCartItemType Type { get; set; }
        public Guid? ReferenceId { get; set; }
        public decimal? Amount { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public string ProductDescription { get; set; }
        public decimal? PricePerUnit { get; set; }
        public Guid? VoucherId { get; set; }
    }
}
