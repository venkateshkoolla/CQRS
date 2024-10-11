using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<AdultTraining> GetAdultTraining(Guid adultTrainingId, Locale locale);
        Task<IList<AdultTraining>> GetAdultTrainings(Locale locale);
    }
}
