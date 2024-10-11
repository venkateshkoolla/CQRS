using System;

namespace Ocas.Domestic.Models
{
    public class DocumentPrint
    {
        public Guid Id { get; set; }
        public Guid DocumentTypeId { get; set; }
        public Guid CollegeId { get; set; }
        public bool SendToColtrane { get; set; }
    }
}
