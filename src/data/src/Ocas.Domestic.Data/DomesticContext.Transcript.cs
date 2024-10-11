using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<Transcript> GetTranscript(Guid id)
        {
            return CrmExtrasProvider.GetTranscript(id);
        }

        public Task<IList<Transcript>> GetTranscripts(GetTranscriptOptions options)
        {
            return CrmExtrasProvider.GetTranscripts(options);
        }

        public async Task<Transcript> CreateTranscript(TranscriptBase transcriptBase)
        {
            var crmEntity = CrmMapper.MapTranscriptBase(transcriptBase);
            var id = await CrmProvider.CreateEntity(crmEntity);

            return await GetTranscript(id);
        }

        public Task DeleteTranscript(Guid id)
        {
            return CrmProvider.DeactivateEntity(Crm.Entities.ocaslr_transcript.EntityLogicalName, id);
        }

        public async Task<Transcript> UpdateTranscript(Transcript transcript)
        {
            var crmEntity = CrmProvider.Transcripts.First(x => x.Id == transcript.Id);

            CrmMapper.PatchTranscript(transcript, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetTranscript(crmEntity.Id);
        }
    }
}
