using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<FirstGenerationApplicant> GetFirstGenerationApplicant(Guid firstGenerationApplicantId, Locale locale);
        Task<IList<FirstGenerationApplicant>> GetFirstGenerationApplicants(Locale locale);
    }
}
