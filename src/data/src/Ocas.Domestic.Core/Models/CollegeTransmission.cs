using System;

namespace Ocas.Domestic.Models
{
    public class CollegeTransmission
    {
        public long Id { get; set; }
        public int TransactionCodeId { get; set; }
        public long ColtraneXcId { get; set; }
        public string CollegeCode { get; set; }
        public string AccountNumber { get; set; }
        public string ApplicationNumber { get; set; }
        public DateTime LastLoadDateTime { get; set; }
        public string TransactionCode { get; set; }
        public char TransactionType { get; set; }
        public string Data { get; set; }
        public Guid BusinessKey { get; set; }
        public string Description { get; set; }
    }
}
