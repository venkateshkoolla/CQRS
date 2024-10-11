using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<HighSkillsMajor> GetHighSkillsMajor(Guid highSkillsMajorId, Locale locale);
        Task<IList<HighSkillsMajor>> GetHighSkillsMajors(Locale locale);
    }
}