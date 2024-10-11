using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<AdultTraining> GetAdultTraining(Guid adultTrainingId, Locale locale)
        {
            return CrmExtrasProvider.GetAdultTraining(adultTrainingId, locale);
        }

        public Task<IList<AdultTraining>> GetAdultTrainings(Locale locale)
        {
            return CrmExtrasProvider.GetAdultTrainings(locale);
        }
    }
}
