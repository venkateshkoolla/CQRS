using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_programintakeentrysemester MapProgramIntakeEntrySemester(ProgramIntakeEntrySemester programIntakeEntrySemester)
        {
            var crm = MapProgramIntakeEntrySemesterBase(programIntakeEntrySemester);
            crm.Id = programIntakeEntrySemester.Id;
            return crm;
        }

        public ocaslr_programintakeentrysemester MapProgramIntakeEntrySemesterBase(ProgramIntakeEntrySemesterBase programIntakeEntrySemesterBase)
        {
            return new ocaslr_programintakeentrysemester
            {
                ocaslr_name = programIntakeEntrySemesterBase.Name,
                ocaslr_programintake = programIntakeEntrySemesterBase.ProgramIntakeId.ToEntityReference(ocaslr_programintake.EntityLogicalName),
                ocaslr_entrysemester = programIntakeEntrySemesterBase.EntrySemesterId.ToEntityReference(ocaslr_entrylevel.EntityLogicalName)
            };
        }
    }
}
