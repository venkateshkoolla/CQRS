using System;

namespace Ocas.Domestic.Models
{
    public class DocumentDistribution
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SupportingDocumentId { get; set; }
        public Guid CollegeId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
