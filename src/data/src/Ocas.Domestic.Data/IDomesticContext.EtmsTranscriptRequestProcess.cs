using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<EtmsTranscriptRequestProcess> GetEtmsTranscriptRequestProcess(Guid id);
        Task<IList<EtmsTranscriptRequestProcess>> GetEtmsTranscriptRequestProcesses(Guid etmsTranscriptRequestId);
        Task<EtmsTranscriptRequestProcess> UpdateEtmsTranscriptRequestProcess(EtmsTranscriptRequestProcess etmsTranscriptRequestProcess);
    }
}
