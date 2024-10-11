using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Country> GetCountry(Guid countryId, Locale locale)
        {
            return CrmExtrasProvider.GetCountry(countryId, locale);
        }

        public Task<Country> GetCountry(string name, Locale locale)
        {
            return CrmExtrasProvider.GetCountry(name, locale);
        }

        public Task<IList<Country>> GetCountries(Locale locale)
        {
            return CrmExtrasProvider.GetCountries(locale);
        }
    }
}
