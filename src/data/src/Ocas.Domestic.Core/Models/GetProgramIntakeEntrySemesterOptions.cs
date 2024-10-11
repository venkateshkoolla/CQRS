using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetProgramIntakeEntrySemesterOptions
    {
        public GetProgramIntakeEntrySemesterOptions()
        {
            StateCode = State.Active;
            StatusCode = Status.Active;
        }

        public State StateCode { get; set; }
        public Status StatusCode { get; set; }
        public Guid? ProgramIntakeId { get; set; }
        public Guid? EntrySemesterId { get; set; }
    }
}
