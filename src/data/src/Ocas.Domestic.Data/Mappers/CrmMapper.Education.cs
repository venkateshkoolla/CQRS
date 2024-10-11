using System;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public void PatchEducation(Education model, ocaslr_education entity)
        {
            PatchEducationBase(model, entity);
        }

        public void PatchEducationBase(EducationBase model, ocaslr_education entity)
        {
            entity.ocaslr_modifiedbyuser = model.ModifiedBy;
            entity.ocaslr_applicantid = model.ApplicantId.ToEntityReference(Crm.Entities.Contact.EntityLogicalName);
            entity.ocaslr_academicupgrade = model.AcademicUpgrade;
            entity.ocaslr_countryid = model.CountryId.ToEntityReference(ocaslr_country.EntityLogicalName);
            entity.ocaslr_provinceid = model.ProvinceId.ToEntityReference(ocaslr_provincestate.EntityLogicalName);
            entity.ocaslr_cityid = model.CityId.ToEntityReference(ocaslr_city.EntityLogicalName);
            entity.ocaslr_institutetypeid = model.InstituteTypeId.ToEntityReference(ocaslr_institutetype.EntityLogicalName);
            entity.ocaslr_school = model.InstituteId.ToEntityReference(Account.EntityLogicalName);
            entity.ocaslr_institutename = model.InstituteName;
            entity.ocaslr_name = model.InstituteName;
            entity.ocaslr_currentlyattending = model.CurrentlyAttending;
            entity.ocaslr_graduatestatus = model.Graduated;
            entity.ocaslr_oen = model.OntarioEducationNumber;
            entity.ocaslr_studentnumber = model.StudentNumber;
            entity.ocaslr_firstnameonrecord = model.FirstNameOnRecord;
            entity.ocaslr_lastnameonrecord = model.LastNameOnRecord;
            entity.ocaslr_major = model.Major;
            entity.ocaslr_levelachievedid = model.LevelAchievedId.ToEntityReference(ocaslr_levelachieved.EntityLogicalName);
            entity.ocaslr_lastgradecompletedid = model.LastGradeCompletedId.ToEntityReference(ocaslr_lastgradecompleted.EntityLogicalName);
            entity.ocaslr_credentialid = model.CredentialId.ToEntityReference(ocaslr_credential.EntityLogicalName);
            entity.ocaslr_credentialreceivedother = model.OtherCredential;
            entity.ocaslr_levelofstudiesid = model.LevelOfStudiesId.ToEntityReference(ocaslr_credential.EntityLogicalName);
            entity.ocaslr_languageofinstruction = model.LanguageOfInstruction;

            entity.ocaslr_attendedfromdate = model.AttendedFrom.YearMonthToDateTime();
            entity.ocaslr_attendedtodate = model.AttendedTo.YearMonthToDateTime();
        }

        public ocaslr_education MapEducationBase(EducationBase model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            var entity = new ocaslr_education();
            PatchEducationBase(model, entity);

            return entity;
        }
    }
}
