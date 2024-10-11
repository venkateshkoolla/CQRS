using System;
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
        public IList<IntakeBrief> MapProgramIntakeBriefs(IList<Dto.ProgramIntake> dbDtos, IList<LookupItem> studyMethods, IList<College> colleges, IList<Campus> campuses, IList<LookupItem> intakeStatuses, IList<LookupItem> intakeAvailabilities, string props)
        {
            var briefs = _mapper.Map<IList<IntakeBrief>>(dbDtos);

            foreach (var brief in briefs)
            {
                brief.Delivery = studyMethods.FirstOrDefault(d => d.Id == brief.DeliveryId)?.Label;
                brief.CollegeName = colleges.FirstOrDefault(d => d.Id == brief.CollegeId)?.Name;
                brief.CampusName = campuses.FirstOrDefault(d => d.Id == brief.CampusId)?.Name;
                brief.IntakeStatus = intakeStatuses.FirstOrDefault(d => d.Id == brief.IntakeStatusId)?.Label;
                brief.IntakeAvailability = intakeAvailabilities.FirstOrDefault(d => d.Id == brief.IntakeAvailabilityId)?.Label;

                if (!string.IsNullOrEmpty(props))
                {
                    var propList = props.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    // These props are JsonIgnore and should not be null
                    propList.Add(nameof(IntakeBrief.Delivery));
                    propList.Add(nameof(IntakeBrief.IntakeStatus));
                    propList.Add(nameof(IntakeBrief.IntakeAvailability));
                    brief.PropsFilter(propList);
                }
            }

            return briefs;
        }

        public ProgramIntake MapProgramIntake(Dto.ProgramIntake dbDto, bool canDelete)
        {
            var intake = _mapper.Map<ProgramIntake>(dbDto);

            intake.CanDelete = canDelete;

            intake.EntryLevelIds = new List<Guid>();
            intake.DefaultEntryLevelId = null;
            if (dbDto.HasSemesterOverride == true)
            {
                intake.EntryLevelIds = dbDto.EntryLevels;
                intake.DefaultEntryLevelId = dbDto.DefaultEntrySemesterId;
            }

            return intake;
        }

        public IList<IntakeExport> MapProgramIntakeExports(IList<Dto.ProgramIntake> dtoList, IList<College> colleges, IList<Campus> campuses, CollegeApplicationCycle collegeApplicationCycle, IList<LookupItem> programDeliveries, IList<LookupItem> programIntakeAvailabilities, IList<LookupItem> programIntakeStatuses, IList<LookupItem> intakeExpiryActions, IList<LookupItem> entryLevels)
        {
            return dtoList.Select(p => new IntakeExport
            {
                IntakeId = p.Id,
                ApplicationCycle = collegeApplicationCycle.Year,
                CampusCode = campuses.FirstOrDefault(x => x.Id == p.CampusId)?.Code,
                CollegeCode = colleges.FirstOrDefault(x => x.Id == p.CollegeId)?.Code,
                ProgramCode = p.ProgramCode,
                ProgramTitle = p.ProgramTitle,
                ProgramDelivery = programDeliveries.FirstOrDefault(x => x.Id == p.ProgramDeliveryId)?.Label,
                StartDate = MapIntakeStartDate(p.StartDate),
                ProgramIntakeAvailability = programIntakeAvailabilities.FirstOrDefault(x => x.Id == p.AvailabilityId)?.Label,
                ProgramIntakeStatus = programIntakeStatuses.FirstOrDefault(x => x.Id == p.ProgramIntakeStatusId)?.Label,
                ExpiryDate = p.ExpiryDate.ToStringOrDefault(),
                Expiration = p.ExpiryActionId.HasValue ? intakeExpiryActions.FirstOrDefault(x => x.Id == p.ExpiryActionId.Value)?.Label : string.Empty,
                HasSemesterOverride = p.HasSemesterOverride == true ? "1" : string.Empty,
                DefaultSemesterOverride = p.DefaultEntrySemesterId.HasValue ? entryLevels.FirstOrDefault(x => x.Id == p.DefaultEntrySemesterId)?.Label : string.Empty
            }).ToList();
        }

        private string MapIntakeStartDate(string startDateRaw)
        {
            if (string.IsNullOrWhiteSpace(startDateRaw)) return string.Empty;

            return startDateRaw.IsDate(Constants.DateFormat.IntakeStartDate)
                ? startDateRaw.ToDateTime(Constants.DateFormat.IntakeStartDate).ToStringOrDefault(Constants.DateFormat.YearShortMonth)
                : string.Empty;
        }
    }
}
