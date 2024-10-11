using System;

namespace Ocas.Domestic.Apply.Models
{
    public class DeclineAllOffersInfo
    {
        public Guid ApplicationId { get; set; }
        public bool? IncludeAccepted { get; set; }
    }
}
