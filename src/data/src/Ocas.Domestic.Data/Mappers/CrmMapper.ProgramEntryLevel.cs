using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_program_entrylevels MapProgramEntrylevel(ProgramEntryLevel programEntryLevel)
        {
            var crm = MapProgramEntryLevelBase(programEntryLevel);
            crm.Id = programEntryLevel.Id;
            return crm;
        }

        public ocaslr_program_entrylevels MapProgramEntryLevelBase(ProgramEntryLevelBase programEntryLevelBase)
        {
            return new ocaslr_program_entrylevels
            {
                ocaslr_name = programEntryLevelBase.Name,
                ocaslr_programid = programEntryLevelBase.ProgramId.ToEntityReference(ocaslr_program.EntityLogicalName),
                ocaslr_entrylevelid = programEntryLevelBase.EntryLevelId.ToEntityReference(ocaslr_entrylevel.EntityLogicalName)
            };
        }
    }
}
