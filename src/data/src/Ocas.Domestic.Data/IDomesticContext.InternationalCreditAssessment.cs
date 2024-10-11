using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<InternationalCreditAssessment> GetInternationalCreditAssessment(Guid internationalCreditAssessmentId);
        Task<IList<InternationalCreditAssessment>> GetInternationalCreditAssessments(Guid applicantId);
        Task<InternationalCreditAssessment> CreateInternationalCreditAssessment(InternationalCreditAssessmentBase internationalCreditAssessmentBase);
        Task<InternationalCreditAssessment> UpdateInternationalCreditAssessment(InternationalCreditAssessment internationalCreditAssessment);
    }
}
