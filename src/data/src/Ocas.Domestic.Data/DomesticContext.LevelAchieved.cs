using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<LevelAchieved> GetLevelAchieved(Guid levelAchievedId, Locale locale)
        {
            return CrmExtrasProvider.GetLevelAchieved(levelAchievedId, locale);
        }

        public Task<IList<LevelAchieved>> GetLevelAchieveds(Locale locale)
        {
            return CrmExtrasProvider.GetLevelAchieveds(locale);
        }
    }
}
