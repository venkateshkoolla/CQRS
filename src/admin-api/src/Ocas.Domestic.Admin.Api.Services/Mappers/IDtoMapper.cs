using System;
using System.Collections.Generic;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Apply.Services.Mappers;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public interface IDtoMapper : IDtoMapperBase
    {
        void PatchAcademicRecordBase(Dto.AcademicRecordBase dbDto, AcademicRecordBase model);
        void PatchApplicantUpdateInfo(Dto.Contact dbDto, ApplicantUpdateInfo model);
        void PatchOntarioStudentCourseCredit(Dto.OntarioStudentCourseCreditBase dbDto, OntarioStudentCourseCreditBase model, string modifiedBy);
        void PatchOntarioStudentCourseCreditBase(Dto.OntarioStudentCourseCreditBase dbDto, OntarioStudentCourseCreditBase model, string modifiedBy, Guid transcriptId);
        void PatchProgram(Dto.Program dbDto, Program model, string modifiedBy, IList<McuCode> mcuCodes, IList<Dto.ProgramSpecialCode> specialCodes, IList<LookupItem> promotions, IList<LookupItem> adultTrainings);
        void PatchProgramBase(Dto.ProgramBase dbDto, ProgramBase model, string modifiedBy, IList<McuCode> mcuCodes, IList<Dto.ProgramSpecialCode> specialCodes, IList<LookupItem> promotions, IList<LookupItem> adultTrainings);
        void PatchProgramIntakeBase(Dto.ProgramIntakeBase dbDto, ProgramIntake model, Dto.Program program, string modifiedBy);
    }
}