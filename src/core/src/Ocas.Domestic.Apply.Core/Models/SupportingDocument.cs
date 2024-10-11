using System;
using Ocas.Domestic.Apply.Enums;

namespace Ocas.Domestic.Apply.Models
{
    public class SupportingDocument
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ReceivedDate { get; set; }
        public SupportingDocumentType Type { get; set; }
        public bool Processing { get; set; }
        public bool CanDownload { get; set; } = true;
    }
}
