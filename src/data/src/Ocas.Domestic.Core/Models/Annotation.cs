using System;

namespace Ocas.Domestic.Models
{
    public class Annotation
    {
        public Guid Id { get; set; }
        public string DocumentBody { get; set; }
        public string MimeType { get; set; }
        public DateTime CreatedOn { get; set; }
        public string FileName { get; set; }
    }
}
