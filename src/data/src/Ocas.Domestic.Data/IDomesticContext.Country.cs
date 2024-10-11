using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Country> GetCountry(Guid countryId, Locale locale);
        Task<Country> GetCountry(string name, Locale locale);
        Task<IList<Country>> GetCountries(Locale locale);
    }
}
