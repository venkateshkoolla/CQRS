using System;

namespace Ocas.Domestic.Models
{
    public class GetCollegeTransmissionHistoryOptions
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public char? TransactionType { get; set; }
        public string TransactionCode { get; set; }
    }
}
