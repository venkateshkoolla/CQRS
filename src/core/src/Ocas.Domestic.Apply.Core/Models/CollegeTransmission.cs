using System;
using Ocas.Domestic.Apply.Enums;

namespace Ocas.Domestic.Apply.Models
{
    public class CollegeTransmission
    {
        public Guid ContextId { get; set; }
        public CollegeTransmissionType Type { get; set; }
        public string Name { get; set; }
        public Guid CollegeId { get; set; }
        public DateTime? Sent { get; set; }
        public bool WaitingForPayment { get; set; }
        public bool RequiredToSend { get; set; } = true;
        public Guid ApplicationId { get; set; }
    }
}
