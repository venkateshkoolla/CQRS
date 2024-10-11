using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class TranscriptRequestException : Model<Guid>
    {
        public Guid InstituteId { get; set; }
        public ExceptionLevel ExceptionLevel { get; set; }
    }
}
