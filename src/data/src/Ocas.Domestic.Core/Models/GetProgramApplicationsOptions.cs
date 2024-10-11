using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetProgramApplicationsOptions
    {
        public GetProgramApplicationsOptions()
        {
            State = State.Active;
        }

        public Guid? ProgramId { get; set; }
        public Guid? IntakeId { get; set; }
        public Guid? ApplicationStatusId { get; set; }
        public State State { get; set; }
    }
}
