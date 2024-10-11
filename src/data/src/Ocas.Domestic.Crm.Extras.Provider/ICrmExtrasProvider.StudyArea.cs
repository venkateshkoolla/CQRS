using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<StudyArea>> GetStudyAreas(Locale locale);
        Task<StudyArea> GetStudyArea(Guid id, Locale locale);
    }
}
