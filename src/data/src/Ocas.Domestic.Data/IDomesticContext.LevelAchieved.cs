using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<LevelAchieved> GetLevelAchieved(Guid levelAchievedId, Locale locale);
        Task<IList<LevelAchieved>> GetLevelAchieveds(Locale locale);
    }
}
