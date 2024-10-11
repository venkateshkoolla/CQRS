using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public void PatchAcademicRecord(AcademicRecord model, ocaslr_academicdata entity)
        {
            PatchAcademicRecordBase(model, entity);
        }

        public void PatchAcademicRecordBase(AcademicRecordBase model, ocaslr_academicdata entity)
        {
            entity.ocaslr_modifiedbyuser = model.ModifiedBy;
            entity.ocaslr_contactid = model.ApplicantId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.ocaslr_gradedate = model.DateCredentialAchieved;
            entity.ocaslr_communityinvolvementid = model.CommunityInvolvementId.ToEntityReference(ocaslr_communityinvolvement.EntityLogicalName);
            entity.ocaslr_highesteducationid = model.HighestEducationId.ToEntityReference(ocaslr_highesteducation.EntityLogicalName);
            entity.ocaslr_highskillsmajorid = model.HighSkillsMajorId.ToEntityReference(ocaslr_highskillsmajor.EntityLogicalName);
            entity.ocaslr_literacytestid = model.LiteracyTestId.ToEntityReference(ocaslr_literacytest.EntityLogicalName);
            entity.ocaslr_mident = model.Mident;
            entity.ocaslr_name = model.Name;
            entity.ocaslr_shmcompletionid = model.ShsmCompletionId.ToEntityReference(ocaslr_shsmcompletion.EntityLogicalName);
        }

        public ocaslr_academicdata MapAcademicRecordBase(AcademicRecordBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new ocaslr_academicdata();
            PatchAcademicRecordBase(model, entity);

            return entity;
        }
    }
}