using System;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<TranscriptRequestLog> GetTranscriptRequestLog(Guid id)
        {
            return CrmExtrasProvider.GetTranscriptRequestLog(id);
        }

        public async Task<TranscriptRequestLog> CreateTranscriptRequestLog(TranscriptRequestLogBase transcriptRequestLogBase)
        {
            var crmEntity = CrmMapper.MapTranscriptRequestLogBase(transcriptRequestLogBase);
            var id = await CrmProvider.CreateEntity(crmEntity);

            return await GetTranscriptRequestLog(id);
        }

        public async Task<TranscriptRequestLog> UpdateTranscriptRequestLog(TranscriptRequestLog transcriptRequestLog)
        {
            var crmEntity = CrmProvider.ApplicantTranscriptRequestLog.First(x => x.Id == transcriptRequestLog.Id);

            CrmMapper.PatchTranscriptRequestLog(transcriptRequestLog, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetTranscriptRequestLog(crmEntity.Id);
        }

        public Task DeleteTranscriptRequestLog(TranscriptRequestLog transcriptRequestLog)
        {
            return DeleteTranscriptRequestLog(transcriptRequestLog.Id);
        }

        public Task DeleteTranscriptRequestLog(Guid id)
        {
            return CrmProvider.DeactivateEntity(Crm.Entities.ocaslr_peterequestlog.EntityLogicalName, id);
        }
    }
}
