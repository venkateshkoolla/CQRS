using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProvinceState> GetProvinceState(Guid provinceStateId, Locale locale);
        Task<IList<ProvinceState>> GetProvinceStates(Locale locale);
    }
}