using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<HighSkillsMajor> GetHighSkillsMajor(Guid highSkillsMajorId, Locale locale)
        {
            return CrmExtrasProvider.GetHighSkillsMajor(highSkillsMajorId, locale);
        }

        public Task<IList<HighSkillsMajor>> GetHighSkillsMajors(Locale locale)
        {
            return CrmExtrasProvider.GetHighSkillsMajors(locale);
        }
    }
}