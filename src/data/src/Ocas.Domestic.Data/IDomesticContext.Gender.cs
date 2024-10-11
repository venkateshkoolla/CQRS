using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Gender> GetGender(Guid genderId, Locale locale);
        Task<IList<Gender>> GetGenders(Locale locale);
    }
}
