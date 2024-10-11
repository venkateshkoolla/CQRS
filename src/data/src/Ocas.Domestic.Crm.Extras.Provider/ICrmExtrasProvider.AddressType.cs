using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<AddressType>> GetAddressTypes(Locale locale);
        Task<AddressType> GetAddressType(Guid id, Locale locale);
    }
}
