using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Title> GetTitle(Guid titleId, Locale locale)
        {
            return CrmExtrasProvider.GetTitle(titleId, locale);
        }

        public Task<IList<Title>> GetTitles(Locale locale)
        {
            return CrmExtrasProvider.GetTitles(locale);
        }
    }
}
