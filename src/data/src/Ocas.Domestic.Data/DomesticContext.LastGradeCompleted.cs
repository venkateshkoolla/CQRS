using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<LastGradeCompleted> GetLastGradeCompleted(Guid lastGradeCompletedId, Locale locale)
        {
            return CrmExtrasProvider.GetLastGradeCompleted(lastGradeCompletedId, locale);
        }

        public Task<IList<LastGradeCompleted>> GetLastGradeCompleteds(Locale locale)
        {
            return CrmExtrasProvider.GetLastGradeCompleteds(locale);
        }
    }
}
