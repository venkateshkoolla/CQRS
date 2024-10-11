using System;

namespace Ocas.Domestic.Apply.Admin.Models
{
    public class ApplicantHistoryDetail
    {
        public Guid Id { get; set; }
        public Guid ApplicantHistoryId { get; set; }
        public string DisplayName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public string SchemaName { get; set; }
    }
}
