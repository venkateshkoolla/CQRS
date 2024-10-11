using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<StatusOfVisa> GetStatusOfVisa(Guid statusOfVisaId, Locale locale)
        {
            return CrmExtrasProvider.GetStatusOfVisa(statusOfVisaId, locale);
        }

        public Task<IList<StatusOfVisa>> GetStatusOfVisas(Locale locale)
        {
            return CrmExtrasProvider.GetStatusOfVisas(locale);
        }
    }
}
