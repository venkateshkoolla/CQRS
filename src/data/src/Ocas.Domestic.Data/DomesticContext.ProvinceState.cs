using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ProvinceState> GetProvinceState(Guid provinceStateId, Locale locale)
        {
            return CrmExtrasProvider.GetProvinceState(provinceStateId, locale);
        }

        public Task<IList<ProvinceState>> GetProvinceStates(Locale locale)
        {
            return CrmExtrasProvider.GetProvinceStates(locale);
        }
    }
}
