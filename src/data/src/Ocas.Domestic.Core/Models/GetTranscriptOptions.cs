using System;

namespace Ocas.Domestic.Models
{
    public class GetTranscriptOptions
    {
        public Guid? ContactId { get; set; }
        public Guid? PartnerId { get; set; }
        public string BoardMident { get; set; }
    }
}
