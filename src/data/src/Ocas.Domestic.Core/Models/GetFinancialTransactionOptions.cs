using System;

namespace Ocas.Domestic.Models
{
    public class GetFinancialTransactionOptions
    {
        public Guid? Id { get; set; }
        public Guid? ApplicantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? OrderId { get; set; }
        public string OrderNumber { get; set; }
        public Guid? ReceiptId { get; set; }
    }
}
