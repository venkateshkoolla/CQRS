using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public interface IDtoMapperBase
    {
        void PatchProgramChoice(Dto.ProgramChoiceBase dbDto, ProgramChoiceBase model);
    }
}
