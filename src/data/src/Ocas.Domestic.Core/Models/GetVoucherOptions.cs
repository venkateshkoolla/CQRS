using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class GetVoucherOptions
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public Guid? ApplicationId { get; set; }
        public State? State { get; set; } = Enums.State.Active;
    }
}
