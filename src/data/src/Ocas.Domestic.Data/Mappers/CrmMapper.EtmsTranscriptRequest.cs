using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public void PatchEtmsTranscriptRequest(EtmsTranscriptRequest model, Ocaslr_etmstranscriptrequest entity)
        {
            entity.Ocaslr_AccountNumber = model.AccountNumber;
            entity.Ocaslr_ApplicationNumber = model.ApplicationNumber;
            entity.Ocaslr_CampusName = model.CampusName;
            entity.Ocaslr_DateLastAttended = model.DateLastAttended;
            entity.Ocaslr_DateofBirth = model.DateOfBirth;
            entity.Ocaslr_Email = model.Email;
            entity.Ocaslr_FormerSurname = model.FormerSurname;
            entity.Ocaslr_GenderId = model.GenderId.ToEntityReference(ocaslr_gendercode.EntityLogicalName);
            entity.Ocaslr_Graduated = model.Graduated;
            entity.Ocaslr_InstitutionName = model.InstitutionName;
            entity.ocaslr_languageofinstruction = model.LanguageOfInstruction;
            entity.Ocaslr_LastGradeCompleted = model.LastGradeCompleted;
            entity.Ocaslr_LastModifiedBy = model.ModifiedBy;
            entity.Ocaslr_LegalFirstGivenName = model.LegalFirstGivenName;
            entity.Ocaslr_LegalFirstNameinFinalYearofHighSchool = model.LegalFirstNameInFinalYearOfHighSchool;
            entity.Ocaslr_LegalLastFamilyName = model.LegalLastFamilyName;
            entity.Ocaslr_LegalSurnameinFinalYearofHighSchool = model.LegalSurnameInFinalYearOfHighSchool;
            entity.Ocaslr_LevelofStudy = model.LevelOfStudy;
            entity.Ocaslr_MiddleName = model.MiddleName;
            entity.Ocaslr_OEN = model.OEN;
            entity.Ocaslr_PhoneNumber = model.PhoneNumber;
            entity.Ocaslr_ProgramName = model.ProgramName;
            entity.Ocaslr_RequestTypeEnum = (Ocaslr_etmstranscriptrequest_Ocaslr_RequestType?)model.EtmsRequestType;
            entity.Ocaslr_StudentNumber = model.StudentNumber;
            entity.Ocaslr_Title = model.Title;
        }
    }
}
