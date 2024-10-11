using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<Title> GetTitle(Guid titleId, Locale locale);

        Task<IList<Title>> GetTitles(Locale locale);
    }
}
