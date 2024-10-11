using System;

namespace Ocas.Domestic.Models
{
    public class Education : EducationBase
    {
        public Guid Id { get; set; }
        public bool HasTranscripts { get; set; }
        public bool HasPaidApplication { get; set; }
        public bool HasMoreThanOneEducation { get; set; }
    }
}
