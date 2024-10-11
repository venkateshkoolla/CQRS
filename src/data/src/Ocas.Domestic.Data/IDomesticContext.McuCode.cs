using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<McuCode> GetMcuCode(Guid mcuCodeId, Locale locale);
        Task<IList<McuCode>> GetMcuCodes(Locale locale);
    }
}
