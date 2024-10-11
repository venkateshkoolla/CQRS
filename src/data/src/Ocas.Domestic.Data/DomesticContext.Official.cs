using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Official> GetOfficial(Guid id, Locale locale)
        {
            return CrmExtrasProvider.GetOfficial(id, locale);
        }

        public Task<IList<Official>> GetOfficials(Locale locale)
        {
            return CrmExtrasProvider.GetOfficials(locale);
        }
    }
}
