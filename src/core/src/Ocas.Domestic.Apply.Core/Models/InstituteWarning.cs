using System;
using Ocas.Domestic.Apply.Enums;

namespace Ocas.Domestic.Apply.Models
{
    public class InstituteWarning
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid InstituteId { get; set; }
        public InstituteWarningType Type { get; set; }
    }
}
