using System;

namespace Ocas.Domestic.Models
{
    public class AcceptedPrivacyStatement : Model<Guid>
    {
        public Guid ContactId { get; set; }
        public Guid PrivacyStatementId { get; set; }
        public DateTime AcceptedDate { get; set; }
    }
}
