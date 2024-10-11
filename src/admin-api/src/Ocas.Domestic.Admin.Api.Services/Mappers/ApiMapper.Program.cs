using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public Program MapProgram(Dto.Program dbDto, IList<McuCode> mcuCodes, IList<Dto.ProgramSpecialCode> specialCodes)
        {
            var program = _mapper.Map<Program>(dbDto);

            program.EntryLevelIds = dbDto.EntryLevels;
            program.McuCode = mcuCodes.FirstOrDefault(m => m.Id == dbDto.McuCodeId)?.Code;
            program.SpecialCode = !dbDto.SpecialCodeId.IsEmpty() ? specialCodes.FirstOrDefault(s => s.Id == dbDto.SpecialCodeId)?.Code : null;

            return program;
        }

        public Program MapProgram(Dto.Program dbDto, IList<McuCode> mcuCodes, IList<Dto.ProgramSpecialCode> specialCodes, IList<Dto.ProgramIntake> dtoProgramIntakes, IList<Dto.ProgramApplication> dtoProgramApplications)
        {
            var program = MapProgram(dbDto, mcuCodes, specialCodes);

            program.Intakes = dtoProgramIntakes.Select(dto => MapProgramIntake(dto, dtoProgramApplications.All(a => a.IntakeId != dto.Id))).ToList();

            return program;
        }

        public IList<ProgramBrief> MapProgramBriefs(IList<Dto.Program> list, IList<LookupItem> programDeliveries, IList<College> colleges, IList<Campus> campuses)
        {
            var briefs = _mapper.Map<IList<ProgramBrief>>(list);
            foreach (var brief in briefs)
            {
                if (brief.DeliveryId.HasValue)
                    brief.Delivery = programDeliveries.FirstOrDefault(d => d.Id == brief.DeliveryId)?.Label;

                brief.College = colleges.FirstOrDefault(d => d.Id == brief.CollegeId)?.Name;
                brief.Campus = campuses.FirstOrDefault(d => d.Id == brief.CampusId)?.Name;
            }

            return briefs;
        }

        public IList<ProgramExport> MapProgramExports(IList<Dto.Program> list, CollegeApplicationCycle collegeApplicationCycle, IList<College> colleges, IList<Campus> campuses, IList<LookupItem> studyMethods, IList<McuCode> mcuCodes, IList<SpecialCode> specialCodes, IList<LookupItem> programTypes, IList<LookupItem> promotions, IList<LookupItem> programLengths, IList<LookupItem> adultTrainings, IList<LookupItem> credentials, IList<LookupItem> studyAreas, IList<LookupItem> highlyComptitives, IList<LookupItem> programLanguages, IList<LookupItem> entryLevels, IList<LookupItem> ministryApprovals, IList<LookupItem> programCategories, IList<SubCategory> programSubCategories)
        {
            return list.Select(p =>
            new ProgramExport
            {
                ProgramId = p.Id,
                ApplicationCycle = collegeApplicationCycle.Year,
                CollegeCode = colleges.FirstOrDefault(o => o.Id == p.CollegeId)?.Code,
                CampusCode = campuses.FirstOrDefault(o => o.Id == p.CampusId)?.Code,
                ProgramCode = p.Code,
                ProgramTitle = p.Title,
                ProgramDelivery = studyMethods.FirstOrDefault(o => o.Id == p.DeliveryId)?.Label,
                ProgramType = programTypes.FirstOrDefault(o => o.Id == p.ProgramTypeId)?.Label,
                Promotion = promotions.FirstOrDefault(o => o.Id == p.PromotionId)?.Label,
                Length = p.Length,
                UnitOfMeasure = programLengths.FirstOrDefault(o => o.Id == p.LengthTypeId)?.Label,
                AdultTraining = adultTrainings.FirstOrDefault(o => o.Id == p.AdultTrainingId)?.Label,
                ProgramSpecialCode = p.SpecialCodeId.HasValue ? specialCodes.FirstOrDefault(o => o.Id == p.SpecialCodeId)?.Code : null,
                ProgramSpecialCodeDescription = p.SpecialCodeId.HasValue ? specialCodes.FirstOrDefault(o => o.Id == p.SpecialCodeId)?.Description : null,
                Credential = credentials.FirstOrDefault(o => o.Id == p.CredentialId)?.Label,
                ApsNumber = p.ApsNumber,
                StudyArea = studyAreas.FirstOrDefault(o => o.Id == p.StudyAreaId)?.Label,
                HighlyCompetitive = highlyComptitives.FirstOrDefault(o => o.Id == p.HighlyCompetitiveId)?.Label,
                ProgramLanguage = programLanguages.FirstOrDefault(o => o.Id == p.LanguageId)?.Label,
                ProgramEntryLevel = entryLevels.FirstOrDefault(o => o.Id == p.DefaultEntryLevelId)?.Label,
                McuCode = mcuCodes.FirstOrDefault(o => o.Id == p.McuCodeId)?.Code,
                McuDescription = mcuCodes.FirstOrDefault(o => o.Id == p.McuCodeId)?.Title,
                MinistryApproval = ministryApprovals.FirstOrDefault(o => o.Id == p.MinistryApprovalId)?.Label,
                Url = p.Url,
                ProgramCategory1 = programCategories.FirstOrDefault(o => o.Id == p.ProgramCategory1Id)?.Label,
                ProgramSubCategory1 = programSubCategories.FirstOrDefault(o => o.Id == p.ProgramSubCategory1Id)?.Label,
                ProgramCategory2 = p.ProgramCategory2Id.HasValue ? programCategories.FirstOrDefault(o => o.Id == p.ProgramCategory2Id)?.Label : null,
                ProgramSubCategory2 = p.ProgramSubCategory2Id.HasValue ? programSubCategories.FirstOrDefault(o => o.Id == p.ProgramSubCategory2Id)?.Label : null
            }).ToList();
        }
    }
}
