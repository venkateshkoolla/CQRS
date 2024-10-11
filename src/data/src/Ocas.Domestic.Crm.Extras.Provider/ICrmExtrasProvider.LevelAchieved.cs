using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<LevelAchieved>> GetLevelAchieveds(Locale locale);
        Task<LevelAchieved> GetLevelAchieved(Guid id, Locale locale);
    }
}
