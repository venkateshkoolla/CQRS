using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_programchoice MapProgramChoice(ProgramChoiceBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new ocaslr_programchoice();
            PatchProgramChoiceBase(model, entity);

            return entity;
        }

        public void PatchProgramChoice(ProgramChoice model, ocaslr_programchoice entity)
        {
            PatchProgramChoiceBase(model, entity);
            entity.Ocaslr_programstartdate = model.IntakeStartDate;
            entity.Ocaslr_withdrawalduetoclosure = model.WithdrawalDueToClosure;
            entity.Ocaslr_withdrawaldate = model.WithdrawalDate;
            entity.ocaslr_offerstatusid = model.OfferStatusId.ToEntityReference(ocaslr_offerstatus.EntityLogicalName);
            entity.ocaslr_confirmeddate = model.ConfirmedDate;
            entity.ocaslr_pathwayid = model.Pathway;
        }

        private void PatchProgramChoiceBase(ProgramChoiceBase model, ocaslr_programchoice entity)
        {
            entity.ocaslr_applicantid = model.ApplicantId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.ocaslr_applicationid = model.ApplicationId.ToEntityReference(ocaslr_application.EntityLogicalName);
            entity.ocaslr_programintakeid = model.ProgramIntakeId.ToEntityReference(ocaslr_programintake.EntityLogicalName);
            entity.ocaslr_entrylevelid = model.EntryLevelId.ToEntityReference(ocaslr_entrylevel.EntityLogicalName);
            entity.ocaslr_previousyearattended = model.PreviousYearAttended;
            entity.ocaslr_previousyearapplied = model.PreviousYearApplied;
            entity.ocaslr_name = model.Name;
            entity.Ocaslr_sequencenumber = model.SequenceNumber.ToOptionSet<ocaslr_programchoice_Ocaslr_sequencenumber>();
            entity.ocaslr_sourceid = model.SourceId.ToEntityReference(ocaslr_source.EntityLogicalName);
            entity.ocaslr_modifiedbyuser = model.ModifiedBy;

            if (model.EffectiveDate.HasValue && model.EffectiveDate.Value != entity.ocaslr_effectivedate)
            {
                entity.ocaslr_effectivedate = model.EffectiveDate;
            }
        }
    }
}
