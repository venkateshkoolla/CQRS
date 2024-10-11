using System;

namespace Ocas.Domestic.Models
{
    public class CustomAuditDetail
    {
        public Guid Id { get; set; }
        public Guid CustomAuditId { get; set; }
        public string DisplayName { get; set; }
        public string OriginalValue { get; set; }
        public string NewValue { get; set; }
        public string SchemaName { get; set; }
    }
}
