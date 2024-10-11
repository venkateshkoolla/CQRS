using System;
using System.Collections.Generic;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class CustomAudit : Auditable
    {
        public Guid Id { get; set; }
        public Guid? ApplicantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public string CustomEntityLabel { get; set; }
        public string CustomEntityLabelEn { get; set; }
        public Guid? OrderId { get; set; }
        public string CustomMessage { get; set; }
        public CustomAuditType CustomAuditType { get; set; }
        public IList<CustomAuditDetail> Details { get; set; }
    }
}
