using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_application MapApplication(ApplicationBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new ocaslr_application();
            PatchApplicationBase(model, entity);

            return entity;
        }

        public void PatchApplication(Application model, ocaslr_application entity)
        {
            PatchApplicationBase(model, entity);
            entity.ocaslr_applicationnumber = model.ApplicationNumber;
            entity.ocaslr_basisforadmissionlock = model.BasisForAdmissionLock;
            entity.ocaslr_currentlock = model.CurrentLock;
            entity.ocaslr_currentid = model.CurrentId.ToEntityReference(ocaslr_current.EntityLogicalName);
            entity.ocaslr_basisforadmissionid = model.BasisForAdmissionId.ToEntityReference(ocaslr_basisforadmission.EntityLogicalName);
            entity.ocaslr_completedsteps = model.CompletedSteps.ToOptionSet<ocaslr_application_ocaslr_completedsteps>();
        }

        private void PatchApplicationBase(ApplicationBase model, ocaslr_application entity)
        {
            entity.ocaslr_applicationstatusid = model.ApplicationStatusId.ToEntityReference(Ocaslr_applicationstatus.EntityLogicalName);
            entity.ocaslr_applicantid = model.ApplicantId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.ocaslr_applicationcycleid = model.ApplicationCycleId.ToEntityReference(ocaslr_applicationcycle.EntityLogicalName);
            entity.ocaslr_effectivedate = model.EffectiveDate;
            entity.ocaslr_modifiedbyuser = model.ModifiedBy;
        }
    }
}
