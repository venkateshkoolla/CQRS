using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<OntarioStudentCourseCredit> GetOntarioStudentCourseCredit(Guid id)
        {
            return CrmExtrasProvider.GetOntarioStudentCourseCredit(id);
        }

        public Task<IList<OntarioStudentCourseCredit>> GetOntarioStudentCourseCredits(GetOntarioStudentCourseCreditOptions options)
        {
            return CrmExtrasProvider.GetOntarioStudentCourseCredits(options);
        }

        public async Task<OntarioStudentCourseCredit> CreateOntarioStudentCourseCredit(OntarioStudentCourseCreditBase ontarioStudentCourseCreditBase)
        {
            var crmEntity = CrmMapper.MapOntarioStudentCourseCreditBase(ontarioStudentCourseCreditBase);
            var id = await CrmProvider.CreateEntity(crmEntity);

            return await GetOntarioStudentCourseCredit(id);
        }

        public Task DeleteOntarioStudentCourseCredit(Guid ontarioStudentCourseCreditId)
        {
            return CrmProvider.DeactivateEntity(Crm.Entities.ocaslr_ontariostudentcoursecredit.EntityLogicalName, ontarioStudentCourseCreditId);
        }

        public async Task<OntarioStudentCourseCredit> UpdateOntarioStudentCourseCredit(OntarioStudentCourseCredit ontarioStudentCourseCredit)
        {
            var crmEntity = CrmProvider.OntarioStudentCourseCredit.FirstOrDefault(x => x.Id == ontarioStudentCourseCredit.Id)
                ?? throw new Exception($"ontarioStudentCourseCredit {ontarioStudentCourseCredit.Id} does not exist");

            CrmMapper.PatchOntarioStudentCourseCredit(ontarioStudentCourseCredit, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetOntarioStudentCourseCredit(crmEntity.Id);
        }
    }
}
