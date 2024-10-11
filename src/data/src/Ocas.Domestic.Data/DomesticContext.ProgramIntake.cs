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
        public async Task<ProgramIntake> ActivateProgramIntake(Guid id)
        {
            await CrmProvider.ActivateEntity(ocaslr_program.EntityLogicalName, id);

            return await GetProgramIntake(id);
        }

        public async Task<ProgramIntake> CreateProgramIntake(ProgramIntakeBase model)
        {
            var crm = CrmMapper.MapProgramIntakeBase(model);
            var id = await CrmProvider.CreateEntity(crm);
            return await GetProgramIntake(id);
        }

        public Task DeleteProgramIntake(Guid id)
        {
            return CrmProvider.DeactivateEntity(ocaslr_programintake.EntityLogicalName, id);
        }

        public Task DeleteProgramIntake(ProgramIntake model)
        {
            return DeleteProgramIntake(model.Id);
        }

        public Task<ProgramIntake> GetProgramIntake(Guid id)
        {
            return CrmExtrasProvider.GetProgramIntake(id);
        }

        public Task<IList<ProgramIntake>> GetProgramIntakes(GetProgramIntakeOptions options)
        {
            return CrmExtrasProvider.GetProgramIntakes(options);
        }

        public Task<IList<ProgramIntake>> GetProgramIntakes(Guid programId)
        {
            return CrmExtrasProvider.GetProgramIntakes(programId);
        }

        public async Task<ProgramIntake> UpdateProgramIntake(ProgramIntake model)
        {
            var crm = CrmMapper.MapProgramIntake(model);
            await CrmProvider.UpdateEntity(crm);
            return await GetProgramIntake(model.Id);
        }
    }
}
