using System;
using System.Collections.Generic;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class Order : Auditable
    {
        public Guid Id { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalTax { get; set; }
        public decimal FinalTotal { get; set; }
        public string OrderNumber { get; set; }
        public decimal FederalTax { get; set; }
        public decimal ProvincialTax { get; set; }
        public OrderPaymentStatus? OrderPaymentStatus { get; set; }
        public OrderProcessingStatus? OrderProcessingStatus { get; set; }
        public OrderPaymentType? OrderPaymentType { get; set; }
        public OrderTransactionType? TransactionType { get; set; }
        public string OrderConfirmationNumber { get; set; }
        public string PaymentResponseCode { get; set; }
        public string Note { get; set; }
        public decimal? AmountPaidToAccountBalance { get; set; }
        public decimal? AmountPaidFromAccountBalance { get; set; }
        public decimal? AmountPaidFromTransferBalance { get; set; }
        public decimal? AmountOverpaymentBalance { get; set; }
        public decimal? AmountPaidToOverpaymentBalance { get; set; }
        public decimal? AmountPaidFromOverpaymentBalance { get; set; }
        public decimal? AmountPaidAgainstNsfBalance { get; set; }
        public decimal? AmountPaidAgainstReturnedPaymentBalance { get; set; }
        public decimal? PaymentAmount { get; set; }
        public decimal? AmountPaidFromVoucher { get; set; }
        public decimal? TransactionAmount { get; set; }
        public string PaymentDetails { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public Guid? ApplicantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public string ApplicationNumber { get; set; }
        public Guid? ApplicationCycleId { get; set; }
        public Guid? ApplicationCycleStatusId { get; set; }
        public IList<OrderDetail> Details { get; set; }
        public Guid SourceId { get; set; }
    }
}
