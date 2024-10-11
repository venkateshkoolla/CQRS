using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<InternationalCreditAssessmentStatus> GetInternationalCreditAssessmentStatus(Guid internationalCreditAssessmentStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetInternationalCreditAssessmentStatus(internationalCreditAssessmentStatusId, locale);
        }

        public Task<IList<InternationalCreditAssessmentStatus>> GetInternationalCreditAssessmentStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetInternationalCreditAssessmentStatuses(locale);
        }
    }
}
