using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<LevelOfStudy>> GetLevelOfStudies(Locale locale);
        Task<LevelOfStudy> GetLevelOfStudy(Guid id, Locale locale);
    }
}
