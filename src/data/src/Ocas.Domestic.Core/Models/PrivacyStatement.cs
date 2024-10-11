using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class PrivacyStatement : Model<Guid>
    {
        public string Content { get; set; }
        public PrivacyStatementCategory Category { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Version { get; set; }
    }
}
