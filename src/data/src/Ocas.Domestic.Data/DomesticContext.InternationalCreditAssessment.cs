using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<InternationalCreditAssessment> GetInternationalCreditAssessment(Guid internationalCreditAssessmentId)
        {
            return CrmExtrasProvider.GetInternationalCreditAssessment(internationalCreditAssessmentId);
        }

        public Task<IList<InternationalCreditAssessment>> GetInternationalCreditAssessments(Guid applicantId)
        {
            return CrmExtrasProvider.GetInternationalCreditAssessments(applicantId);
        }

        public async Task<InternationalCreditAssessment> CreateInternationalCreditAssessment(InternationalCreditAssessmentBase internationalCreditAssessmentBase)
        {
            var crmEntity = CrmMapper.MapInternationalCreditAssessmentBase(internationalCreditAssessmentBase);
            var id = await CrmProvider.CreateEntity(crmEntity);

            return await GetInternationalCreditAssessment(id);
        }

        public async Task<InternationalCreditAssessment> UpdateInternationalCreditAssessment(InternationalCreditAssessment internationalCreditAssessment)
        {
            var crmEntity = CrmProvider.InternationalCreditAssessments.Single(x => x.Id == internationalCreditAssessment.Id);
            CrmMapper.PatchInternationalCreditAssessment(internationalCreditAssessment, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetInternationalCreditAssessment(crmEntity.Id);
        }

        public Task DeleteInternationalCreditAssessment(Guid id)
        {
            return CrmProvider.DeactivateEntity(Crm.Entities.ocaslr_internationalcreditassessment.EntityLogicalName, id);
        }
    }
}
