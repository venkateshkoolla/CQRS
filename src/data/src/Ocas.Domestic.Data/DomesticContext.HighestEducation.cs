using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<HighestEducation> GetHighestEducation(Guid highestEducationId, Locale locale)
        {
            return CrmExtrasProvider.GetHighestEducation(highestEducationId, locale);
        }

        public Task<IList<HighestEducation>> GetHighestEducations(Locale locale)
        {
            return CrmExtrasProvider.GetHighestEducations(locale);
        }
    }
}