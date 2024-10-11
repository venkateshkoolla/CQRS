using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetOntarioStudentCourseCreditOptions
    {
        public GetOntarioStudentCourseCreditOptions()
        {
            State = State.Active;
        }

        public Guid? ApplicantId { get; set; }
        public Guid? Id { get; set; }
        public State State { get; set; }
        public Guid? TranscriptId { get; set; }
    }
}
