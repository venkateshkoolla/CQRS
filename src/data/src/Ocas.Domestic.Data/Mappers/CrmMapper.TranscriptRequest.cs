using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_transcriptrequest MapTranscriptRequestBase(TranscriptRequestBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new ocaslr_transcriptrequest();
            PatchTranscriptRequestBase(model, entity);

            return entity;
        }

        public void PatchTranscriptRequest(TranscriptRequest model, ocaslr_transcriptrequest entity)
        {
            PatchTranscriptRequestBase(model, entity);
        }

        private void PatchTranscriptRequestBase(TranscriptRequestBase model, ocaslr_transcriptrequest entity)
        {
            entity.ocaslr_applicantid = model.ApplicantId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.ocaslr_applicationid = model.ApplicationId.ToEntityReference(ocaslr_application.EntityLogicalName);
            entity.ocaslr_educationid = model.EducationId.ToEntityReference(ocaslr_education.EntityLogicalName);
            entity.Ocaslr_eTMSTranscriptRequestId = model.EtmsTranscriptRequestId.ToEntityReference(Ocaslr_etmstranscriptrequest.EntityLogicalName);
            entity.ocaslr_fromschoolid = model.FromSchoolId.ToEntityReference(Account.EntityLogicalName);
            entity.ocaslr_fromschooltype = model.FromSchoolType is null ? null : ((int)model.FromSchoolType).ToOptionSet<ocaslr_transcriptrequest_ocaslr_fromschooltype>();
            entity.ocaslr_name = model.Name;
            entity.ocaslr_peterequestlogid = model.PeteRequestLogId.ToEntityReference(ocaslr_peterequestlog.EntityLogicalName);
            entity.ocaslr_toschoolid = model.ToSchoolId.ToEntityReference(Account.EntityLogicalName);
            entity.ocaslr_transcripttransmissionid = model.TranscriptTransmissionId.ToEntityReference(ocaslr_transcripttransmission.EntityLogicalName);
            entity.ocaslr_transcriptrequeststatusid = model.TranscriptRequestStatusId.ToEntityReference(ocaslr_transcriptrequeststatus.EntityLogicalName);
            entity.ocaslr_modifiedbyuser = model.ModifiedBy;
        }
    }
}
