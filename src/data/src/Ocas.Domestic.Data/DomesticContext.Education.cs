using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Education> GetEducation(Guid educationId)
        {
            return CrmExtrasProvider.GetEducation(educationId);
        }

        public Task<IList<Education>> GetEducations(Guid applicantId)
        {
            return CrmExtrasProvider.GetEducations(applicantId);
        }

        public async Task<Education> CreateEducation(EducationBase educationBase)
        {
            var crmEntity = CrmMapper.MapEducationBase(educationBase);
            var id = await CrmProvider.CreateEntity(crmEntity);

            return await GetEducation(id);
        }

        public Task DeleteEducation(Guid educationId)
        {
            return CrmProvider.DeactivateEntity(Crm.Entities.ocaslr_education.EntityLogicalName, educationId);
        }

        public async Task<Education> UpdateEducation(Education education)
        {
            var crmEntity = CrmProvider.EducationRecords.Single(x => x.Id == education.Id);

            CrmMapper.PatchEducation(education, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetEducation(crmEntity.Id);
        }
    }
}
