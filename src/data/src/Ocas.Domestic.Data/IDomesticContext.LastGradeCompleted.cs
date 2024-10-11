using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<LastGradeCompleted> GetLastGradeCompleted(Guid lastGradeCompletedId, Locale locale);

        Task<IList<LastGradeCompleted>> GetLastGradeCompleteds(Locale locale);
    }
}
