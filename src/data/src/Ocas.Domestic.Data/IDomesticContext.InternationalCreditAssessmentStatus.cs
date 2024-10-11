using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<InternationalCreditAssessmentStatus> GetInternationalCreditAssessmentStatus(Guid internationalCreditAssessmentStatusId, Locale locale);
        Task<IList<InternationalCreditAssessmentStatus>> GetInternationalCreditAssessmentStatuses(Locale locale);
    }
}
