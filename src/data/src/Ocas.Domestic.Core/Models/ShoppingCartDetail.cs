using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class ShoppingCartDetail
    {
        public Guid Id { get; set; }
        public Guid ShoppingCartId { get; set; }
        public ShoppingCartItemType Type { get; set; }
        public string Description { get; set; }
        public Guid? ReferenceId { get; set; }
        public decimal? Amount { get; set; }
        public int Quantity { get; set; }
        public Guid? UomId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? ProgramChoiceId { get; set; }
        public Guid? VoucherId { get; set; }
        public Guid? TranscriptRequestId { get; set; }
        public decimal? ManualDiscountAmount { get; set; }
        public decimal? PricePerUnit { get; set; }
        public string ProductName { get; set; }
        public Guid? VoucherProductId { get; set; }
    }
}
