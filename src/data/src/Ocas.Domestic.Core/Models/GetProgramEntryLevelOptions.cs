using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetProgramEntryLevelOptions
    {
        public GetProgramEntryLevelOptions()
        {
            StateCode = State.Active;
            StatusCode = Status.Active;
        }

        public State StateCode { get; set; }
        public Status StatusCode { get; set; }
        public Guid? ProgramId { get; set; }
        public Guid? EntryLevelId { get; set; }
    }
}
