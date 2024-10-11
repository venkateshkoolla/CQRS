using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<LevelOfStudy> GetLevelOfStudy(Guid levelOfStudyId, Locale locale)
        {
            return CrmExtrasProvider.GetLevelOfStudy(levelOfStudyId, locale);
        }

        public Task<IList<LevelOfStudy>> GetLevelOfStudies(Locale locale)
        {
            return CrmExtrasProvider.GetLevelOfStudies(locale);
        }
    }
}
