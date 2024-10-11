using System;

namespace Ocas.Domestic.Models
{
    public class University
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid SchoolStatusId { get; set; }
        public bool HasEtms { get; set; }
        public decimal? TranscriptFee { get; set; }
        public bool ShowInEducation { get; set; }
        public Address MailingAddress { get; set; }
    }
}
