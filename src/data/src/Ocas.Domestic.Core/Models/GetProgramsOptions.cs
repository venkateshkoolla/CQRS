using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetProgramsOptions
    {
        public GetProgramsOptions()
        {
            StateCode = State.Active;
        }

        public Guid? ApplicationCycleId { get; set; }
        public Guid? CollegeId { get; set; }
        public Guid? CampusId { get; set; }
        public Guid? DeliveryId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public State StateCode { get; set; }
    }
}