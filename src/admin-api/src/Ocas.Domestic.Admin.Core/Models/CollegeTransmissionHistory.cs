using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class CollegeTransmissionHistory
    {
        public long Id { get; set; }
        public Guid ContextId { get; set; }
        public Guid CollegeId { get; set; }
        public DateTime? Sent { get; set; }
        public string Data { get; set; }
        public string Description { get; set; }
        public char TransactionType { get; set; }
        public string TransactionCode { get; set; }
        public IList<CollegeTransmissionHistory> Details { get; set; }
    }
}
