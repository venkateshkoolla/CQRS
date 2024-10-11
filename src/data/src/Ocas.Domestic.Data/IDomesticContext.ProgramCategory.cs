using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProgramCategory> GetProgramCategory(Guid programCategoryId, Locale locale);
        Task<IList<ProgramCategory>> GetProgramCategories(Locale locale);
    }
}
