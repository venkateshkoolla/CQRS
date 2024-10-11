using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<McuCode> GetMcuCode(Guid mcuCodeId, Locale locale)
        {
            return CrmExtrasProvider.GetMcuCode(mcuCodeId, locale);
        }

        public Task<IList<McuCode>> GetMcuCodes(Locale locale)
        {
            return CrmExtrasProvider.GetMcuCodes(locale);
        }
    }
}
