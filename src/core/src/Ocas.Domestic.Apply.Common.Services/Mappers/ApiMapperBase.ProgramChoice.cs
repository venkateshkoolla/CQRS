using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Dto = Ocas.Domestic.Models;
using DtoEnum = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Services.Mappers
{
    public partial class ApiMapperBase
    {
        public IList<ProgramChoice> MapProgramChoices(IList<Dto.ProgramChoice> dbDtos, IList<Dto.ProgramIntake> intakes, IList<LookupItem> intakeAvailabilities, IList<Dto.ShoppingCartDetail> details)
        {
            var choices = new List<ProgramChoice>();
            foreach (var dbDto in dbDtos)
            {
                var choice = MapProgramChoice(dbDto);

                if (dbDto.SupplementalFeePaid == false)
                {
                    choice.SupplementalFeeDescription = details.FirstOrDefault(x => x.ReferenceId == choice.CollegeId)?.Description;
                }

                var intake = intakes.FirstOrDefault(i => i.Id == choice.IntakeId);
                if (intake != null)
                {
                    choice.EligibleEntryLevelIds = intake.EntryLevels;

                    var intakeAvailability = intakeAvailabilities.FirstOrDefault(s => s.Id == intake.AvailabilityId);
                    choice.IsActive = intake.State == DtoEnum.State.Active
                        && intakeAvailability.Code == Constants.ProgramIntakeAvailabilities.Open
                        && intake.StartDate.ToDateTime(Constants.DateFormat.IntakeStartDate).TotalMonths() > -Constants.ProgramChoices.MonthsToInactivity;
                }

                choices.Add(choice);
            }

            return choices;
        }

        public ProgramChoice MapProgramChoice(Dto.ProgramChoice dbDto)
        {
            return _mapper.Map<ProgramChoice>(dbDto);
        }
    }
}
