using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data.Mappers
{
    public partial class CrmMapper
    {
        public ocaslr_program MapProgram(Program program)
        {
            var crm = MapProgramBase(program);
            crm.Id = program.Id;
            return crm;
        }

        public ocaslr_program MapProgramBase(ProgramBase program)
        {
            return new ocaslr_program
            {
                ocaslr_collegeapplicationcycleid = program.CollegeApplicationCycleId.ToEntityReference(ocaslr_collegeapplicationcycle.EntityLogicalName),
                ocaslr_campusid = program.CampusId.ToEntityReference(Account.EntityLogicalName),
                ocaslr_programcode = program.Code,
                ocaslr_title = program.Title,
                ocaslr_name = program.Title.Truncate(100),
                ocaslr_ProgramDelivery = program.DeliveryId.ToEntityReference(ocaslr_offerstudymethod.EntityLogicalName),
                ocaslr_programtypeid = program.ProgramTypeId.ToEntityReference(ocaslr_programtype.EntityLogicalName),
                ocaslr_length = program.Length,
                ocaslr_unitofmeasureid = program.LengthTypeId.ToEntityReference(ocaslr_unitofmeasure.EntityLogicalName),
                ocaslr_mcucodeid = program.McuCodeId.ToEntityReference(ocaslr_mcucode.EntityLogicalName),
                ocaslr_credentialid = program.CredentialId.ToEntityReference(ocaslr_credential.EntityLogicalName),
                ocaslr_entrylevelid = program.DefaultEntryLevelId.ToEntityReference(ocaslr_entrylevel.EntityLogicalName),
                ocaslr_studyareaid = program.StudyAreaId.ToEntityReference(ocaslr_studyarea.EntityLogicalName),
                ocaslr_highlycompetitiveid = program.HighlyCompetitiveId.ToEntityReference(ocaslr_highlycompetitive.EntityLogicalName),
                ocaslr_programlanguageid = program.LanguageId.ToEntityReference(ocaslr_programlanguage.EntityLogicalName),
                ocaslr_programlevelid = program.LevelId.ToEntityReference(ocaslr_programlevel.EntityLogicalName),
                ocaslr_promotionid = program.PromotionId.ToEntityReference(ocaslr_promotion.EntityLogicalName),
                ocaslr_adulttrainingid = program.AdultTrainingId.ToEntityReference(ocaslr_adulttraining.EntityLogicalName),
                ocaslr_specialcodeid = program.SpecialCodeId.ToEntityReference(ocaslr_programspecialcode.EntityLogicalName),
                ocaslr_apsnumber = program.ApsNumber,
                ocaslr_ministryapprovalid = program.MinistryApprovalId.ToEntityReference(ocaslr_ministryapproval.EntityLogicalName),
                ocaslr_url = program.Url,
                ocaslr_programcategory1id = program.ProgramCategory1Id.ToEntityReference(ocaslr_programcategory.EntityLogicalName),
                ocaslr_subcategory1id = program.ProgramSubCategory1Id.ToEntityReference(ocaslr_programsubcategory.EntityLogicalName),
                ocaslr_programcategory2id = program.ProgramCategory2Id.ToEntityReference(ocaslr_programcategory.EntityLogicalName),
                ocaslr_programsubcategory2id = program.ProgramSubCategory2Id.ToEntityReference(ocaslr_programcategory.EntityLogicalName),
                ocaslr_modifiedbyuser = program.ModifiedBy
            };
        }
    }
}
