using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public async Task<ProgramEntryLevel> CreateProgramEntryLevel(ProgramEntryLevelBase model)
        {
            var crm = CrmMapper.MapProgramEntryLevelBase(model);
            var id = await CrmProvider.CreateEntity(crm);
            return await GetProgramEntryLevel(id);
        }

        public Task DeleteProgramEntryLevel(Guid id)
        {
            return CrmProvider.DeleteEntity(ocaslr_program_entrylevels.EntityLogicalName, id);
        }

        public Task<ProgramEntryLevel> GetProgramEntryLevel(Guid id)
        {
            return CrmExtrasProvider.GetProgramEntryLevel(id);
        }

        public Task<IList<ProgramEntryLevel>> GetProgramEntryLevels(GetProgramEntryLevelOptions options)
        {
            return CrmExtrasProvider.GetProgramEntryLevels(options);
        }

        public async Task<ProgramEntryLevel> UpdateProgramEntryLevel(ProgramEntryLevel model)
        {
            var crm = CrmMapper.MapProgramEntrylevel(model);
            await CrmProvider.UpdateEntity(crm);
            return await GetProgramEntryLevel(model.Id);
        }
    }
}
