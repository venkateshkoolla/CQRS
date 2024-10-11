using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Gender> GetGender(Guid genderId, Locale locale)
        {
            return CrmExtrasProvider.GetGender(genderId, locale);
        }

        public Task<IList<Gender>> GetGenders(Locale locale)
        {
            return CrmExtrasProvider.GetGenders(locale);
        }
    }
}
