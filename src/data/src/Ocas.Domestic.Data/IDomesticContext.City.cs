using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<City> GetCity(Guid cityId, Locale locale);

        Task<IList<City>> GetCities(Locale locale);
    }
}
