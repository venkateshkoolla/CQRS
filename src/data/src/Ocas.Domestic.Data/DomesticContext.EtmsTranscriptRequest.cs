using System;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<EtmsTranscriptRequest> GetEtmsTranscriptRequest(Guid id)
        {
            return CrmExtrasProvider.GetEtmsTranscriptRequest(id);
        }

        public async Task<EtmsTranscriptRequest> UpdateEtmsTranscriptRequest(EtmsTranscriptRequest etmsTranscriptRequest)
        {
            var crmEntity = CrmProvider.eTMSTranscriptRequests.First(x => x.Id == etmsTranscriptRequest.Id);

            CrmMapper.PatchEtmsTranscriptRequest(etmsTranscriptRequest, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetEtmsTranscriptRequest(crmEntity.Id);
        }
    }
}
