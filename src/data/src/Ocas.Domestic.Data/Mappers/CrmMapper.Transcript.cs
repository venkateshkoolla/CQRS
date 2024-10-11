using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_transcript MapTranscriptBase(TranscriptBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));
            var entity = new ocaslr_transcript();
            PatchTranscriptBase(model, entity);

            return entity;
        }

        public void PatchTranscript(Transcript model, ocaslr_transcript entity)
        {
            PatchTranscriptBase(model, entity);
        }

        private void PatchTranscriptBase(TranscriptBase model, ocaslr_transcript entity)
        {
            entity.ocaslr_originalstudentid = model.OriginalStudentId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.ocaslr_transcripttypeEnum = (ocaslr_transcript_ocaslr_transcripttype)model.TranscriptType;
            entity.ocaslr_name = model.Name;
            entity.ocaslr_partnerid = model.PartnerId.ToEntityReference(ocaslr_partneridentity.EntityLogicalName);
            entity.ocaslr_contactid = model.ContactId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.Ocaslr_credentials = model.Credentials;
            entity.ocaslr_eTMSTranscriptId = model.EtmsTranscriptId.ToEntityReference(Ocaslr_etmstranscriptrequest.EntityLogicalName);
            entity.ocaslr_SupportingDocumentId = model.SupportingDocumentId.ToEntityReference(ocaslr_supportingdocument.EntityLogicalName);
            entity.ocaslr_TranscriptSourceId = model.TranscriptSourceId.ToEntityReference(ocaslr_transcriptsource.EntityLogicalName);
            entity.ocaslr_modifiedbyuser = model.ModifiedBy;
        }
    }
}
