using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Current> GetCurrent(Guid currentId, Locale locale)
        {
            return CrmExtrasProvider.GetCurrent(currentId, locale);
        }

        public Task<IList<Current>> GetCurrents(Locale locale)
        {
            return CrmExtrasProvider.GetCurrents(locale);
        }
    }
}
