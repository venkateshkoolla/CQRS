using System;

namespace Ocas.Domestic.Models
{
    public class ApplicantMessage
    {
        public Guid Id { get; set; }
        public Guid ApplicantId { get; set; }
        public bool? HasRead { get; set; }
        public string LocalizedSubject { get; set; }
        public string LocalizedText { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
