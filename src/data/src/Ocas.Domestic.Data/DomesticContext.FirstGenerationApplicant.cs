using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<FirstGenerationApplicant> GetFirstGenerationApplicant(Guid firstGenerationApplicantId, Locale locale)
        {
            return CrmExtrasProvider.GetFirstGenerationApplicant(firstGenerationApplicantId, locale);
        }

        public Task<IList<FirstGenerationApplicant>> GetFirstGenerationApplicants(Locale locale)
        {
            return CrmExtrasProvider.GetFirstGenerationApplicants(locale);
        }
    }
}
