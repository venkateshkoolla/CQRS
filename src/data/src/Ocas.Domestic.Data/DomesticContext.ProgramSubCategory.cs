using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ProgramSubCategory> GetProgramSubCategory(Guid programSubCategoryId, Locale locale)
        {
            return CrmExtrasProvider.GetProgramSubCategory(programSubCategoryId, locale);
        }

        public Task<IList<ProgramSubCategory>> GetProgramSubCategories(Locale locale)
        {
            return CrmExtrasProvider.GetProgramSubCategories(locale);
        }

        public Task<IList<ProgramSubCategory>> GetProgramSubCategories(Guid programCategoryId, Locale locale)
        {
            return CrmExtrasProvider.GetProgramSubCategories(programCategoryId, locale);
        }
    }
}
