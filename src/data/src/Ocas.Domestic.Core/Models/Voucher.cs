using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class Voucher
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ProductId { get; set; }
        public string Code { get; set; }
        public decimal? Value { get; set; }
        public VoucherState? VoucherState { get; set; }
        public Guid? ApplicantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? ShoppingCartId { get; set; }
        public Guid? SourceId { get; set; }

        public Guid? OrderDetailId
        {
            get => string.IsNullOrEmpty(OrderDetailIdString) ? (Guid?)null : new Guid(OrderDetailIdString);
            set => OrderDetailIdString = value?.ToString();
        }

        public Guid? ShoppingCartDetailId
        {
            get => string.IsNullOrEmpty(ShoppingCartDetailIdString) ? (Guid?)null : new Guid(ShoppingCartDetailIdString);
            set => ShoppingCartDetailIdString = value?.ToString();
        }

        // because these fields are stored as NVARCHAR(36) instead of UNIQUEIDENTIFIER in the database
        // https://stackoverflow.com/a/31607532
        private string OrderDetailIdString { get; set; }
        private string ShoppingCartDetailIdString { get; set; }
    }
}
