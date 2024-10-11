using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;
using Contact = Ocas.Domestic.Crm.Entities.Contact;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_acceptedprivacystatement MapAcceptedPrivacyStatement(Models.Contact contact, PrivacyStatement privacyStatement, DateTime acceptedDate)
        {
            if (acceptedDate.Kind != DateTimeKind.Utc) throw new ArgumentException($"acceptedDate must be DateTimeKind.Utc but received: {acceptedDate.Kind}", nameof(acceptedDate));
            if (contact is null) throw new ArgumentNullException(nameof(contact));
            if (privacyStatement is null) throw new ArgumentNullException(nameof(privacyStatement));

            var entity = new ocaslr_acceptedprivacystatement
            {
                ocaslr_contactid = contact.Id.ToEntityReference(Contact.EntityLogicalName),
                ocaslr_privacystatementid = privacyStatement.Id.ToEntityReference(ocaslr_privacystatement.EntityLogicalName),
                ocaslr_accepteddate = acceptedDate,
                ocaslr_name = privacyStatement.Name
            };

            return entity;
        }
    }
}
