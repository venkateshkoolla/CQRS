using System.Threading.Tasks;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public interface IDtoMapper
    {
        Task PatchContact(Dto.Contact dbDto, Applicant model);
        Task PatchEducation(Dto.EducationBase dbDto, EducationBase model);
        void PatchProgramChoice(Dto.ProgramChoiceBase dbDto, ProgramChoiceBase model);
    }
}