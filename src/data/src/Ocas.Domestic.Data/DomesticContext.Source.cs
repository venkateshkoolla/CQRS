using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Source> GetSource(Guid sourceId, Locale locale)
        {
            return CrmExtrasProvider.GetSource(sourceId, locale);
        }

        public Task<IList<Source>> GetSources(Locale locale)
        {
            return CrmExtrasProvider.GetSources(locale);
        }
    }
}
