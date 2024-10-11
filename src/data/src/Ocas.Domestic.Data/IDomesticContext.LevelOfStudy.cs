using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<LevelOfStudy> GetLevelOfStudy(Guid levelOfStudyId, Locale locale);
        Task<IList<LevelOfStudy>> GetLevelOfStudies(Locale locale);
    }
}
