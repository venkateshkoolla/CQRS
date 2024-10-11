using System;

namespace Ocas.Domestic.Apply.Models
{
    public class Education : EducationBase, IAuditable
    {
        public Guid Id { get; set; }
        public bool CanDelete { get; set; }
        public bool HasTranscripts { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
