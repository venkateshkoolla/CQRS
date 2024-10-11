using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public async Task<ProgramChoice> CreateProgramChoice(ProgramChoiceBase programChoiceBase)
        {
            if (programChoiceBase.EffectiveDate.HasValue && programChoiceBase.EffectiveDate.Value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentException($"EffectiveDate must be DateTimeKind.Utc but received: {programChoiceBase.EffectiveDate.Value.Kind}", nameof(programChoiceBase));
            }

            var entity = CrmMapper.MapProgramChoice(programChoiceBase);
            var id = await CrmProvider.CreateEntity(entity);

            return await GetProgramChoice(id);
        }

        public Task DeleteProgramChoice(Guid id)
        {
            return CrmProvider.DeactivateEntity(Crm.Entities.ocaslr_programchoice.EntityLogicalName, id);
        }

        public async Task DeleteProgramChoice(ProgramChoice programChoice)
        {
            await DeleteProgramChoice(programChoice.Id);
        }

        public Task<ProgramChoice> GetProgramChoice(Guid id)
        {
            return CrmExtrasProvider.GetProgramChoice(id);
        }

        public Task<IList<ProgramChoice>> GetProgramChoices(GetProgramChoicesOptions options)
        {
            return CrmExtrasProvider.GetProgramChoices(options);
        }

        public Task<bool> HasProgramChoices(Guid applicationId)
        {
            return CrmExtrasProvider.HasProgramChoices(applicationId);
        }

        public async Task<ProgramChoice> UpdateProgramChoice(ProgramChoice programChoice)
        {
            var entity = CrmProvider.ProgramChoice.Single(x => x.Id == programChoice.Id);

            CrmMapper.PatchProgramChoice(programChoice, entity);
            await CrmProvider.UpdateEntity(entity);

            return await GetProgramChoice(entity.Id);
        }
    }
}
