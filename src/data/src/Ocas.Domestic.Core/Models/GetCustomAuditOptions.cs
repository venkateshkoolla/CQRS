using System;

namespace Ocas.Domestic.Models
{
    public class GetCustomAuditOptions
    {
        public Guid? ApplicantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
