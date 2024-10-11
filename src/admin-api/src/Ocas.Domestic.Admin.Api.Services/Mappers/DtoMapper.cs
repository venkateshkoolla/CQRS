using System;
using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Apply.Services.Mappers;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public class DtoMapper : DtoMapperBase, IDtoMapper
    {
        public void PatchAcademicRecordBase(Dto.AcademicRecordBase dbDto, AcademicRecordBase model)
        {
            dbDto.CommunityInvolvementId = model.CommunityInvolvementId;
            dbDto.HighestEducationId = model.HighestEducationId;
            dbDto.HighSkillsMajorId = model.HighSkillsMajorId;
            dbDto.LiteracyTestId = model.LiteracyTestId;
            dbDto.DateCredentialAchieved = model.DateCredentialAchieved.ToNullableDateTime();
        }

        public void PatchApplicantUpdateInfo(Dto.Contact dbDto, ApplicantUpdateInfo model)
        {
            dbDto.FirstName = model.FirstName;
            dbDto.LastName = model.LastName;
            dbDto.BirthDate = model.BirthDate.ToDateTime();
        }

        public void PatchOntarioStudentCourseCredit(Dto.OntarioStudentCourseCreditBase dbDto, OntarioStudentCourseCreditBase model, string modifiedBy)
        {
            PatchOntarioStudentCourseCreditCommon(dbDto, model, modifiedBy);
        }

        public void PatchOntarioStudentCourseCreditBase(Dto.OntarioStudentCourseCreditBase dbDto, OntarioStudentCourseCreditBase model, string modifiedBy, Guid transcriptId)
        {
            PatchOntarioStudentCourseCreditCommon(dbDto, model, modifiedBy);
            dbDto.TranscriptId = transcriptId;
        }

        public void PatchOntarioStudentCourseCreditCommon(Dto.OntarioStudentCourseCreditBase dbDto, OntarioStudentCourseCreditBase model, string modifiedBy)
        {
            dbDto.ApplicantId = model.ApplicantId;
            dbDto.CompletedDate = model.CompletedDate.ToDateTime(Constants.DateFormat.YearMonthDashed).ToString(Constants.DateFormat.CompletedDate);
            dbDto.CourseCode = model.CourseCode;
            dbDto.CourseMident = model.CourseMident;
            dbDto.CourseDeliveryId = model.CourseDeliveryId;
            dbDto.CourseStatusId = model.CourseStatusId;
            dbDto.CourseTypeId = model.CourseTypeId;
            dbDto.Credit = model.Credit;
            dbDto.Grade = model.Grade;
            dbDto.GradeTypeId = model.GradeTypeId;

            dbDto.Notes = null;
            if (model.Notes != null)
            {
                dbDto.Notes = string.Concat(model.Notes);
            }

            dbDto.ModifiedBy = modifiedBy;
        }

        public void PatchProgram(Dto.Program dbDto, Program model, string modifiedBy, IList<McuCode> mcuCodes, IList<Dto.ProgramSpecialCode> specialCodes, IList<LookupItem> promotions, IList<LookupItem> adultTrainings)
        {
            PatchProgramCommon(dbDto, model, modifiedBy, mcuCodes, specialCodes, promotions, adultTrainings);
        }

        public void PatchProgramBase(Dto.ProgramBase dbDto, ProgramBase model, string modifiedBy, IList<McuCode> mcuCodes, IList<Dto.ProgramSpecialCode> specialCodes, IList<LookupItem> promotions, IList<LookupItem> adultTrainings)
        {
            PatchProgramCommon(dbDto, model, modifiedBy, mcuCodes, specialCodes, promotions, adultTrainings);

            dbDto.CollegeApplicationCycleId = model.ApplicationCycleId;
            dbDto.CampusId = model.CampusId;
            dbDto.Code = model.Code;
            dbDto.DeliveryId = model.DeliveryId;
        }

        public void PatchProgramIntakeBase(Dto.ProgramIntakeBase dbDto, ProgramIntake model, Dto.Program program, string modifiedBy)
        {
            dbDto.ProgramId = program.Id;
            dbDto.AvailabilityId = model.IntakeAvailabilityId;
            dbDto.StartDate = model.StartDate;
            dbDto.Name = $"{program.Code}-{model.StartDate}";
            dbDto.ExpiryDate = model.ExpiryDate.ToNullableDateTime();
            dbDto.EnrolmentProjection = model.EnrolmentEstimate;
            dbDto.EnrolmentMaximum = model.EnrolmentMax;
            dbDto.ExpiryActionId = model.IntakeExpiryActionId;
            dbDto.ProgramIntakeStatusId = model.IntakeStatusId;
            dbDto.DefaultEntrySemesterId = model.DefaultEntryLevelId;
            dbDto.ModifiedBy = modifiedBy;

            dbDto.HasSemesterOverride = model.EntryLevelIds?.Any() == true && dbDto.DefaultEntrySemesterId.HasValue;
        }

        private void PatchProgramCommon(Dto.ProgramBase dbDto, ProgramBase model, string modifiedBy, IList<McuCode> mcuCodes, IList<Dto.ProgramSpecialCode> specialCodes, IList<LookupItem> promotions, IList<LookupItem> adultTrainings)
        {
            dbDto.Title = model.Title;
            dbDto.ProgramTypeId = model.ProgramTypeId;
            dbDto.Length = model.Length;
            dbDto.LengthTypeId = model.LengthTypeId;
            dbDto.McuCodeId = mcuCodes.First(x => x.Code == model.McuCode).Id;
            dbDto.CredentialId = model.CredentialId;
            dbDto.DefaultEntryLevelId = model.DefaultEntryLevelId;
            dbDto.StudyAreaId = model.StudyAreaId;
            dbDto.HighlyCompetitiveId = model.HighlyCompetitiveId;
            dbDto.LanguageId = model.LanguageId;
            dbDto.LevelId = model.LevelId;

            if (!model.PromotionId.HasValue)
            {
                var promotion = promotions.First(x => x.Code == Constants.Promotions.Standard);
                dbDto.PromotionId = promotion.Id;
            }
            else
            {
                dbDto.PromotionId = model.PromotionId.Value;
            }

            if (!model.AdultTrainingId.HasValue)
            {
                var adultTraining = adultTrainings.First(x => x.Code == Constants.AdultTraining.No);
                dbDto.AdultTrainingId = adultTraining.Id;
            }
            else
            {
                dbDto.AdultTrainingId = model.AdultTrainingId.Value;
            }

            dbDto.SpecialCodeId = null;
            if (!string.IsNullOrWhiteSpace(model.SpecialCode))
                dbDto.SpecialCodeId = specialCodes.First(x => x.Code == model.SpecialCode).Id;

            dbDto.ApsNumber = model.ApsNumber;
            dbDto.MinistryApprovalId = model.MinistryApprovalId;
            dbDto.Url = model.Url;
            dbDto.ProgramCategory1Id = model.ProgramCategory1Id;
            dbDto.ProgramSubCategory1Id = model.ProgramSubCategory1Id;
            dbDto.ProgramCategory2Id = model.ProgramCategory2Id;
            dbDto.ProgramSubCategory2Id = model.ProgramSubCategory2Id;
            dbDto.ModifiedBy = modifiedBy;
        }
    }
}
