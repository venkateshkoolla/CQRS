using System;
using System.Collections.Generic;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class ApplicantHistory
    {
        public Guid Id { get; set; }
        public Guid? ApplicantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public IList<ApplicantHistoryDetail> Details { get; set; }
    }
}
