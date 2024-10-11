using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<ProgramSubCategory>> GetProgramSubCategories(Locale locale);
        Task<IList<ProgramSubCategory>> GetProgramSubCategories(Guid programCategoryId, Locale locale);
        Task<ProgramSubCategory> GetProgramSubCategory(Guid id, Locale locale);
    }
}
