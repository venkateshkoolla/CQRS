using System;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Models
{
    public class ContactBase : Auditable
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool? DoNotSendMM { get; set; }
        public string PreferredName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string SubjectId { get; set; }
        public DateTime BirthDate { get; set; }
        public ContactType ContactType { get; set; }
        public Guid AccountStatusId { get; set; }
        public Guid SourceId { get; set; }
        public Guid? AcceptedPrivacyStatementId { get; set; }
        public Guid PreferredLanguageId { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool LastLoginExceed { get; set; }
        public Guid? SourcePartnerId { get; set; }
    }
}
