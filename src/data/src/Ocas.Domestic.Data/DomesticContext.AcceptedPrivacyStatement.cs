using System;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public async Task<AcceptedPrivacyStatement> AddAcceptedPrivacyStatement(Contact contact, PrivacyStatement privacyStatement, DateTime acceptedDate)
        {
            if (acceptedDate.Kind != DateTimeKind.Utc) throw new ArgumentException($"acceptedDate must be DateTimeKind.Utc but received: {acceptedDate.Kind}", nameof(acceptedDate));

            var crmEntity = CrmMapper.MapAcceptedPrivacyStatement(contact, privacyStatement, acceptedDate);
            var id = await CrmProvider.CreateEntity(crmEntity);

            return await CrmExtrasProvider.GetAcceptedPrivacyStatement(id);
        }
    }
}
