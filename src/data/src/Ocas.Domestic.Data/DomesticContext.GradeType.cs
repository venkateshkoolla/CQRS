using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<GradeType> GetGradeType(Guid gradeTypeId, Locale locale)
        {
            return CrmExtrasProvider.GetGradeType(gradeTypeId, locale);
        }

        public Task<IList<GradeType>> GetGradeTypes(Locale locale)
        {
            return CrmExtrasProvider.GetGradeTypes(locale);
        }
    }
}