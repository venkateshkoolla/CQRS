using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Models;
using Dto = Ocas.Domestic.Models;
using DtoEnum = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<ProgramChoice> MapProgramChoices(IList<Dto.ProgramChoice> dbDtos, IList<Dto.ProgramIntake> intakes, Dto.ShoppingCart shoppingCart)
        {
            var choices = new List<ProgramChoice>();
            foreach (var dbDto in dbDtos)
            {
                var choice = MapProgramChoice(dbDto);

                if (dbDto.SupplementalFeePaid == false)
                {
                    choice.SupplementalFeeDescription = shoppingCart.Details.FirstOrDefault(x => x.ReferenceId == choice.CollegeId)?.Description;
                }

                choice.EligibleEntryLevelIds = intakes.FirstOrDefault(i => i.Id == choice.IntakeId)?.EntryLevels;
                choice.IsActive = intakes.FirstOrDefault(i => i.Id == choice.IntakeId)?.State == DtoEnum.State.Active;
                choices.Add(choice);
            }

            return choices;
        }
    }
}
