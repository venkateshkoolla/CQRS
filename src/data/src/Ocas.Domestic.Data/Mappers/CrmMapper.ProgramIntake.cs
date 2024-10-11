using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_programintake MapProgramIntake(ProgramIntake programIntake)
        {
            var crm = MapProgramIntakeBase(programIntake);
            crm.Id = programIntake.Id;
            return crm;
        }

        public ocaslr_programintake MapProgramIntakeBase(ProgramIntakeBase programIntake)
        {
            return new ocaslr_programintake
            {
                ocaslr_programid = programIntake.ProgramId.ToEntityReference(ocaslr_program.EntityLogicalName),
                ocaslr_availabilitytid = programIntake.AvailabilityId.ToEntityReference(ocaslr_programintakeavailability.EntityLogicalName),
                Ocaslr_startdate = programIntake.StartDate,
                ocaslr_name = programIntake.Name,
                ocaslr_expirydate = programIntake.ExpiryDate,
                ocaslr_enrolmentprojection = programIntake.EnrolmentProjection,
                ocaslr_enrolmentmaximum = programIntake.EnrolmentMaximum,
                ocaslr_expiryactionid = programIntake.ExpiryActionId.ToEntityReference(ocaslr_expiryaction.EntityLogicalName),
                ocaslr_programintakestatusid = programIntake.ProgramIntakeStatusId.ToEntityReference(ocaslr_programintakestatus.EntityLogicalName),
                ocaslr_defaultentrysemester = programIntake.DefaultEntrySemesterId.ToEntityReference(ocaslr_entrylevel.EntityLogicalName),
                ocaslr_overrideentrysemesters = programIntake.HasSemesterOverride
            };
        }
    }
}
