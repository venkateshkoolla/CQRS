using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetCollegeApplicationsOptions
    {
        public GetCollegeApplicationsOptions()
        {
            StateCode = State.Active;
        }

        public Guid? CollegeId { get; set; }
        public Guid? ApplicationCycleId { get; set; }
        public State StateCode { get; set; }
    }
}
