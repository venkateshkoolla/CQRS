using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<City> GetCity(Guid cityId, Locale locale)
        {
            return CrmExtrasProvider.GetCity(cityId, locale);
        }

        public Task<IList<City>> GetCities(Locale locale)
        {
            return CrmExtrasProvider.GetCities(locale);
        }
    }
}
