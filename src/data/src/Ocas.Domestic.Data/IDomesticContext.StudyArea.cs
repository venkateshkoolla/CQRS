using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<StudyArea> GetStudyArea(Guid studyAreaId, Locale locale);
        Task<IList<StudyArea>> GetStudyAreas(Locale locale);
    }
}
