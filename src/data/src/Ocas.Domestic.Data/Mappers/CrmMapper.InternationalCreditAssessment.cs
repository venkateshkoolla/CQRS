using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_internationalcreditassessment MapInternationalCreditAssessmentBase(InternationalCreditAssessmentBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new ocaslr_internationalcreditassessment();
            PatchInternationalCreditAssessmentBase(model, entity);

            return entity;
        }

        public void PatchInternationalCreditAssessment(InternationalCreditAssessmentBase model, ocaslr_internationalcreditassessment entity)
        {
            PatchInternationalCreditAssessmentBase(model, entity);
        }

        private void PatchInternationalCreditAssessmentBase(InternationalCreditAssessmentBase model, ocaslr_internationalcreditassessment entity)
        {
            entity.ocaslr_applicantid = model.ApplicantId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.ocaslr_internationalcreditassesmentstaid = model.InternationalCreditAssessmentStatusId.ToEntityReference(ocaslr_internationalcreditassesmentstatus.EntityLogicalName);
            entity.ocaslr_havecoursesecondaryevaluation = model.HaveHighSchoolCourseEvaluation;
            entity.ocaslr_referencenumber = model.ReferenceNumber;
            entity.ocaslr_coursesecondaryevaluatorid = model.CredentialEvaluationAgencyId.ToEntityReference(Account.EntityLogicalName);
            entity.ocaslr_havepostsecondaryevaluation = model.HavePostSecondaryEvaluation;
            entity.ocaslr_modifiedbyuser = model.ModifiedBy;
        }
    }
}
