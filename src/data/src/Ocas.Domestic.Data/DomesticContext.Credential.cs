using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Credential> GetCredential(Guid credentialId, Locale locale)
        {
            return CrmExtrasProvider.GetCredential(credentialId, locale);
        }

        public Task<IList<Credential>> GetCredentials(Locale locale)
        {
            return CrmExtrasProvider.GetCredentials(locale);
        }
    }
}
