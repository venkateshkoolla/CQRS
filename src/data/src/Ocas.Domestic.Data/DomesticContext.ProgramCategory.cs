using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<ProgramCategory> GetProgramCategory(Guid programCategoryId, Locale locale)
        {
            return CrmExtrasProvider.GetProgramCategory(programCategoryId, locale);
        }

        public Task<IList<ProgramCategory>> GetProgramCategories(Locale locale)
        {
            return CrmExtrasProvider.GetProgramCategories(locale);
        }
    }
}
