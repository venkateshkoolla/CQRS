using System;

namespace Ocas.Domestic.Models
{
    public class AboriginalStatus : Model<Guid>
    {
        public string ColtraneCode { get; set; }
        public bool ShowInPortal { get; set; }
    }
}
