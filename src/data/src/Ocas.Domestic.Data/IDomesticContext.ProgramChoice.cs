using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProgramChoice> CreateProgramChoice(ProgramChoiceBase programChoiceBase);
        Task DeleteProgramChoice(Guid id);
        Task DeleteProgramChoice(ProgramChoice programChoice);
        Task<ProgramChoice> GetProgramChoice(Guid id);
        Task<IList<ProgramChoice>> GetProgramChoices(GetProgramChoicesOptions options);
        Task<bool> HasProgramChoices(Guid applicationId);
        Task<ProgramChoice> UpdateProgramChoice(ProgramChoice programChoice);
    }
}