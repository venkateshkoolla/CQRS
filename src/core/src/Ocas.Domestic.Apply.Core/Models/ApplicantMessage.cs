using System;

namespace Ocas.Domestic.Apply.Models
{
    public class ApplicantMessage
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Message { get; set; }
        public bool? Read { get; set; }
        public string Title { get; set; }
    }
}
