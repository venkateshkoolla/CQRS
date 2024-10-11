using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public void PatchOntarioStudentCourseCredit(OntarioStudentCourseCredit model, ocaslr_ontariostudentcoursecredit entity)
        {
            PatchOntarioStudentCourseCreditBase(model, entity);
        }

        public void PatchOntarioStudentCourseCreditBase(OntarioStudentCourseCreditBase model, ocaslr_ontariostudentcoursecredit entity)
        {
            entity.Ocaslr_modifiedbyuser = model.ModifiedBy;
            entity.ocaslr_contactid = model.ApplicantId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.ocaslr_creditvalue = model.Credit.HasValue ? (double)model.Credit : (double?)null;
            entity.ocaslr_coursecode = model.CourseCode;
            entity.ocaslr_completiondate = model.CompletedDate;
            entity.ocaslr_note = model.Notes;
            entity.ocaslr_coursemident = model.CourseMident;
            entity.ocaslr_transcriptid = model.TranscriptId.ToEntityReference(ocaslr_transcript.EntityLogicalName);
            entity.ocaslr_grade = model.Grade;
            entity.ocaslr_coursestatusid = model.CourseStatusId.ToEntityReference(ocaslr_coursestatus.EntityLogicalName);
            entity.ocaslr_coursetypeid = model.CourseTypeId.ToEntityReference(ocaslr_coursetype.EntityLogicalName);
            entity.ocaslr_gradetypeid = model.GradeTypeId.ToEntityReference(ocaslr_gradetype.EntityLogicalName);
            entity.ocaslr_coursedeliveryid = model.CourseDeliveryId.ToEntityReference(ocaslr_coursedelivery.EntityLogicalName);
        }

        public ocaslr_ontariostudentcoursecredit MapOntarioStudentCourseCreditBase(OntarioStudentCourseCreditBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new ocaslr_ontariostudentcoursecredit();
            PatchOntarioStudentCourseCreditBase(model, entity);

            return entity;
        }
    }
}