using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public void PatchEtmsTranscriptRequestProcess(EtmsTranscriptRequestProcess model, Ocaslr_etmstranscriptrequestprocess entity)
        {
            entity.Ocaslr_TranscriptRequestStatusId = model.TranscriptRequestStatusId.ToEntityReference(ocaslr_transcriptrequeststatus.EntityLogicalName);
            entity.Ocaslr_LastModifiedBy = model.ModifiedBy;
        }
    }
}
