using System;

namespace Ocas.Domestic.Apply.Models
{
    public class OfflinePaymentInfo
    {
        public Guid PaymentMethodId { get; set; }
        public string Notes { get; set; }
        public decimal Amount { get; set; }
    }
}
