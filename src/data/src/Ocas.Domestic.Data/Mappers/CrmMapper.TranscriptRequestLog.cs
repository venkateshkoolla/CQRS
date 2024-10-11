using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_peterequestlog MapTranscriptRequestLogBase(TranscriptRequestLogBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new ocaslr_peterequestlog();
            PatchTranscriptRequestLogBase(model, entity);

            return entity;
        }

        public void PatchTranscriptRequestLog(TranscriptRequestLog model, ocaslr_peterequestlog entity)
        {
            PatchTranscriptRequestLogBase(model, entity);
        }

        private void PatchTranscriptRequestLogBase(TranscriptRequestLogBase model, ocaslr_peterequestlog entity)
        {
            entity.ocaslr_name = model.Name;
            entity.ocaslr_orderid = model.OrderId.ToEntityReference(SalesOrder.EntityLogicalName);
            entity.Ocaslr_processedstatus = model.ProcessStatus.HasValue ? ((int)model.ProcessStatus).ToOptionSet<ocaslr_peterequestlog_Ocaslr_processedstatus>() : null;
            entity.ocaslr_requesttimestamp = model.RequestTimestamp;
            entity.Ocaslr_servicerequest = model.ServiceRequest;
            entity.Ocaslr_serviceresponse = model.ServiceResponse;
            entity.ocaslr_serviceresponsecode = model.ServiceResponseCode;
            entity.ocaslr_transcriptrequeststatusid = model.TranscriptRequestStatusId.ToEntityReference(ocaslr_transcriptrequeststatus.EntityLogicalName);
        }
    }
}
