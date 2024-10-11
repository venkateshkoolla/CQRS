using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public class DtoMapperBase : IDtoMapperBase
    {
        public void PatchProgramChoice(Dto.ProgramChoiceBase dbDto, ProgramChoiceBase model)
        {
            dbDto.ApplicationId = model.ApplicationId;
            dbDto.ApplicantId = model.ApplicantId;
            dbDto.EntryLevelId = model.EntryLevelId;
            dbDto.ProgramIntakeId = model.IntakeId;
            dbDto.PreviousYearApplied = model.PreviousYearApplied;
            dbDto.PreviousYearAttended = model.PreviousYearAttended;
        }
    }
}
