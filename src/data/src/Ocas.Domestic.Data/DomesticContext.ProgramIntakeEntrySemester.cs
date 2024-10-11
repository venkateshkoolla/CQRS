using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext : IDomesticContext
    {
        public async Task<ProgramIntakeEntrySemester> CreateProgramIntakeEntrySemester(ProgramIntakeEntrySemesterBase model)
        {
            var crm = CrmMapper.MapProgramIntakeEntrySemesterBase(model);
            var id = await CrmProvider.CreateEntity(crm);
            return await GetProgramIntakeEntrySemester(id);
        }

        public Task DeleteProgramIntakeEntrySemester(Guid id)
        {
            return CrmProvider.DeleteEntity(ocaslr_programintakeentrysemester.EntityLogicalName, id);
        }

        public Task<ProgramIntakeEntrySemester> GetProgramIntakeEntrySemester(Guid id)
        {
            return _crmExtrasProvider.GetProgramIntakeEntrySemester(id);
        }

        public Task<IList<ProgramIntakeEntrySemester>> GetProgramIntakeEntrySemesters(GetProgramIntakeEntrySemesterOptions options)
        {
            return _crmExtrasProvider.GetProgramIntakeEntrySemesters(options);
        }

        public async Task<ProgramIntakeEntrySemester> UpdateProgramIntakeEntrySemester(ProgramIntakeEntrySemester model)
        {
            var crm = CrmMapper.MapProgramIntakeEntrySemester(model);
            await CrmProvider.UpdateEntity(crm);
            return await GetProgramIntakeEntrySemester(model.Id);
        }
    }
}
