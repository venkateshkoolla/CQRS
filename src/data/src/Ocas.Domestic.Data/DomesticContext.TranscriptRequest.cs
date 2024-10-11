using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<TranscriptRequest> GetTranscriptRequest(Guid id)
        {
            return CrmExtrasProvider.GetTranscriptRequest(id);
        }

        public Task<IList<TranscriptRequest>> GetTranscriptRequests(GetTranscriptRequestOptions options)
        {
            return CrmExtrasProvider.GetTranscriptRequests(options);
        }

        public async Task<TranscriptRequest> CreateTranscriptRequest(TranscriptRequestBase transcriptRequestBase)
        {
            var crmEntity = CrmMapper.MapTranscriptRequestBase(transcriptRequestBase);
            var id = await CrmProvider.CreateEntity(crmEntity);

            return await GetTranscriptRequest(id);
        }

        public async Task<TranscriptRequest> UpdateTranscriptRequest(TranscriptRequest transcriptRequest)
        {
            var crmEntity = CrmProvider.TranscriptRequests.First(x => x.Id == transcriptRequest.Id);

            CrmMapper.PatchTranscriptRequest(transcriptRequest, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetTranscriptRequest(crmEntity.Id);
        }

        public Task DeleteTranscriptRequest(TranscriptRequest transcriptRequest)
        {
            return DeleteTranscriptRequest(transcriptRequest.Id);
        }

        // from A2C: https://dev.azure.com/ocas/OCAS%20Portfolio/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Providers%2FTranscriptRequest%2FTranscriptRequestProvider.cs&version=GBmaster&line=159&lineStyle=plain&lineEnd=166&lineStartColumn=9&lineEndColumn=10
        public Task DeleteTranscriptRequest(Guid id)
        {
            return CrmProvider.DeleteEntity(Crm.Entities.ocaslr_transcriptrequest.EntityLogicalName, id);
        }
    }
}
