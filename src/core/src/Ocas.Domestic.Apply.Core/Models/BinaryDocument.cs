using System;

namespace Ocas.Domestic.Apply.Models
{
    public class BinaryDocument
    {
        public string Name { get; set; }
        public string MimeType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public byte[] Data { get; set; }
    }
}
