using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<AddressType> GetAddressType(Guid addressTypeId, Locale locale)
        {
            return CrmExtrasProvider.GetAddressType(addressTypeId, locale);
        }

        public Task<IList<AddressType>> GetAddressTypes(Locale locale)
        {
            return CrmExtrasProvider.GetAddressTypes(locale);
        }
    }
}
