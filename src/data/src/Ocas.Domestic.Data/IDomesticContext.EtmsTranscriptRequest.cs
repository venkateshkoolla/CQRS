using System;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<EtmsTranscriptRequest> GetEtmsTranscriptRequest(Guid id);
        Task<EtmsTranscriptRequest> UpdateEtmsTranscriptRequest(EtmsTranscriptRequest etmsTranscriptRequest);
    }
}
