using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetOfferAcceptancesOptions
    {
        public GetOfferAcceptancesOptions()
        {
            StateCode = State.Active;
            StatusCode = Status.Active;
        }

        public State StateCode { get; set; }
        public Status StatusCode { get; set; }
        public Guid? Id { get; set; }
        public Guid? ApplicationId { get; set; }
    }
}
