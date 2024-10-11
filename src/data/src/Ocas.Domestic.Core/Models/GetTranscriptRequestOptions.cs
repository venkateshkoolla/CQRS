using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetTranscriptRequestOptions
    {
        public GetTranscriptRequestOptions()
        {
            State = State.Active;
            Status = Status.Active;
        }

        public Guid? ApplicantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public Guid? EducationId { get; set; }
        public State State { get; set; }
        public Status Status { get; set; }
    }
}
