using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public async Task<Program> ActivateProgram(Guid programId)
        {
            await CrmProvider.ActivateEntity(ocaslr_program.EntityLogicalName, programId);

            return await GetProgram(programId);
        }

        public async Task<Program> CreateProgram(ProgramBase model)
        {
            await FixTitleCaseAsync(model);
            var crm = CrmMapper.MapProgramBase(model);
            var id = await CrmProvider.CreateEntity(crm);
            return await GetProgram(id);
        }

        public Task DeleteProgram(Program model)
        {
            return CrmProvider.DeactivateEntity(ocaslr_program.EntityLogicalName, model.Id);
        }

        public Task<Program> GetProgram(Guid programId)
        {
            return CrmExtrasProvider.GetProgram(programId);
        }

        public Task<IList<ProgramApplication>> GetProgramApplications(GetProgramApplicationsOptions options)
        {
            return CrmExtrasProvider.GetProgramApplications(options);
        }

        public Task<IList<Program>> GetPrograms(GetProgramsOptions options)
        {
            return CrmExtrasProvider.GetPrograms(options);
        }

        public async Task<Program> UpdateProgram(Program model)
        {
            await FixTitleCaseAsync(model);
            var crm = CrmMapper.MapProgram(model);
            await CrmProvider.UpdateEntity(crm);
            return await GetProgram(model.Id);
        }

        private async Task FixTitleCaseAsync(ProgramBase program)
        {
            var language = await CrmExtrasProvider.GetProgramLanguage(program.LanguageId, Locale.English);
            if (language.Code == Constants.ProgramLanguageCode.English)
            {
                program.Title = program.Title.AsTitleCase();
            }
        }
    }
}
