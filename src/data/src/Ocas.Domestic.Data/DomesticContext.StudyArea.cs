using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<StudyArea> GetStudyArea(Guid studyAreaId, Locale locale)
        {
            return CrmExtrasProvider.GetStudyArea(studyAreaId, locale);
        }

        public Task<IList<StudyArea>> GetStudyAreas(Locale locale)
        {
            return CrmExtrasProvider.GetStudyAreas(locale);
        }
    }
}
