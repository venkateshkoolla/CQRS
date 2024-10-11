using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class FinancialTransaction
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? TransactionDate { get; set; }
        public OrderPaymentType? PaymentType { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal? TransactionAmount { get; set; }
        public decimal? ReturnedPaymentPaid { get; set; }
        public decimal? ReturnedPaymentChargePaid { get; set; }
        public decimal? PayFromAccountBalance { get; set; }
        public decimal? PayToAccountBalance { get; set; }
        public decimal? AccountBalanceBeforeTransaction { get; set; }
        public decimal? AccountBalanceAfterTransaction { get; set; }
        public decimal? OverPaymentBalanceBeforeTransaction { get; set; }
        public decimal? OverPaymentBalanceAfterTransaction { get; set; }
        public decimal? NsfBalanceBeforeTransaction { get; set; }
        public decimal? NsfBalanceAfterTransaction { get; set; }
        public decimal? ReturnedPaymentBeforeTransaction { get; set; }
        public decimal? ReturnedPaymentAfterTransaction { get; set; }
        public decimal? ReturnedPaymentCharge { get; set; }
        public decimal? ReturnedPayment { get; set; }
        public decimal? PayFromApplicationBalance { get; set; }
        public decimal? PayToApplicationBalance { get; set; }
        public decimal? PayFromOverpayment { get; set; }
        public decimal? PayToOverpayment { get; set; }
        public decimal? PayFromTransfer { get; set; }
        public decimal? PayToTransfer { get; set; }
        public decimal? PaymentAmount { get; set; }
        public decimal? OrderAmount { get; set; }
        public decimal? OutstandingOrderAmount { get; set; }
        public string ChequeNumber { get; set; }
        public bool ReturnChargeApplied { get; set; }
        public string Note { get; set; }
        public string MoneyOrderNumber { get; set; }
        public Guid? ApplicantId { get; set; }
        public string ApplicantFirstName { get; set; }
        public string ApplicantLastName { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? ApplicationApplicantId { get; set; }
        public string ApplicationFirstName { get; set; }
        public string ApplicationLastName { get; set; }
        public Guid? OrderId { get; set; }
        public string OrderDetailId { get; set; }
        public Guid? RelatedFinancialTransactionId { get; set; }
        public string RelatedFinancialTransactionName { get; set; }
        public string RelatedOrderNumber { get; set; }
        public string OrderNumber { get; set; }
        public Guid? OrderApplicationId { get; set; }
        public string OrderApplicationNumber { get; set; }
        public string TransactionApplicationNumber { get; set; }
        public Receipt Receipt { get; set; }
    }
}
