using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<EtmsTranscriptRequestProcess> GetEtmsTranscriptRequestProcess(Guid id)
        {
            return CrmExtrasProvider.GetEtmsTranscriptRequestProcess(id);
        }

        public Task<IList<EtmsTranscriptRequestProcess>> GetEtmsTranscriptRequestProcesses(Guid etmsTranscriptRequestId)
        {
            return CrmExtrasProvider.GetEtmsTranscriptRequestProcesses(etmsTranscriptRequestId);
        }

        public async Task<EtmsTranscriptRequestProcess> UpdateEtmsTranscriptRequestProcess(EtmsTranscriptRequestProcess etmsTranscriptRequestProcess)
        {
            var crmEntity = CrmProvider.eTMSTranscriptRequestProcesses.First(x => x.Id == etmsTranscriptRequestProcess.Id);

            CrmMapper.PatchEtmsTranscriptRequestProcess(etmsTranscriptRequestProcess, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetEtmsTranscriptRequestProcess(crmEntity.Id);
        }
    }
}
