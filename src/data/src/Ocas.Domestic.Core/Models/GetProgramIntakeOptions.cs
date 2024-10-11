using System;
using System.Collections.Generic;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetProgramIntakeOptions
    {
        public GetProgramIntakeOptions()
        {
            StateCode = State.Active;
            StatusCode = Status.Active;
        }

        public State? StateCode { get; set; }
        public Status? StatusCode { get; set; }
        public IList<Guid> Ids { get; set; }
        public Guid? ApplicationCycleId { get; set; }
        public Guid? CollegeId { get; set; }
        public Guid? CampusId { get; set; }
        public string ProgramCode { get; set; }
        public string ProgramTitle { get; set; }
        public string FromDate { get; set; }
        public Guid? ProgramDeliveryId { get; set; }
        public Guid? ProgramId { get; set; }
    }
}
