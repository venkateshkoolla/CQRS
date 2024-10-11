using System;

namespace Ocas.Domestic.Models
{
    public class ReceiptBase
    {
        public string AvsResponseCode { get; set; }
        public string BankApprovalCode { get; set; }
        public string BankTransactionId { get; set; }
        public string Cardholder { get; set; }
        public decimal ChargeTotal { get; set; }
        public string CvdResponseCode { get; set; }
        public int? Eci { get; set; }
        public string Invoice { get; set; }
        public int? IsoCode { get; set; }
        public string IssConf { get; set; }
        public string IssName { get; set; }
        public string Message { get; set; }
        public string Name { get; set; }
        public Guid? PaymentMethodId { get; set; }
        public Guid? PaymentResultId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseOrderId { get; set; }
        public DateTime? TimeStamp { get; set; }
        public Guid? TransactionCurrencyId { get; set; }
        public string TransactionKey { get; set; }
        public string TransactionName { get; set; }
        public string GatewayTransactionId { get; set; }
        public string MaskedPan { get; set; }
    }
}