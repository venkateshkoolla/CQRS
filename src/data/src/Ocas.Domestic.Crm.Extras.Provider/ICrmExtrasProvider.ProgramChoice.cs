using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<ProgramChoice>> GetProgramChoices(GetProgramChoicesOptions options);
        Task<ProgramChoice> GetProgramChoice(Guid id);
        Task<bool> HasProgramChoices(Guid applicationId);
    }
}
