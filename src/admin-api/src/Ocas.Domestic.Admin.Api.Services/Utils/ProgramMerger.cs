using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Utils
{
    public static class ProgramMerger
    {
        public static async Task MergeProgramEntryLevels(IDomesticContext domesticContext, IList<Domestic.Models.ProgramEntryLevel> existingEntryLevels, IList<Guid> entryLevelIds, Guid programId)
        {
            entryLevelIds = entryLevelIds ?? new List<Guid>();

            ////////////
            // DELETE //
            ////////////

            var removedEntryLevels = existingEntryLevels.Where(x => entryLevelIds.All(y => y != x.EntryLevelId));

            foreach (var entryLevel in removedEntryLevels)
            {
                await domesticContext.DeleteProgramEntryLevel(entryLevel.Id);
            }

            ////////////
            // CREATE //
            ////////////

            var newEntryLevels = entryLevelIds.Where(x => existingEntryLevels.All(y => x != y.EntryLevelId));

            foreach (var entryLevelId in newEntryLevels)
            {
                await domesticContext.CreateProgramEntryLevel(new Domestic.Models.ProgramEntryLevelBase
                {
                    EntryLevelId = entryLevelId,
                    ProgramId = programId
                });
            }

            // No update since I'm assuming we don't deactivate entry levels
        }

        public static async Task MergeIntakes(IDomesticContext domesticContext, IDtoMapper dtoMapper, IList<Domestic.Models.ProgramIntake> existingIntakes, IList<ProgramIntake> intakes, Domestic.Models.Program program, string modifiedBy)
        {
            intakes = intakes ?? new List<ProgramIntake>();

            ////////////
            // DELETE //
            ////////////

            var deleteIntakes = existingIntakes.Where(x => intakes.All(y => x.StartDate != y.StartDate));

            foreach (var intake in deleteIntakes)
            {
                await domesticContext.DeleteProgramIntake(intake.Id);
            }

            ////////////
            // CREATE //
            ////////////

            var createIntakes = intakes.Where(x => existingIntakes.All(y => x.StartDate != y.StartDate));

            foreach (var intake in createIntakes)
            {
                var dtoIntake = new Domestic.Models.ProgramIntakeBase();

                dtoMapper.PatchProgramIntakeBase(dtoIntake, intake, program, modifiedBy);

                var createdIntake = await domesticContext.CreateProgramIntake(dtoIntake);

                await MergeIntakeEntryLevels(domesticContext, new List<Domestic.Models.ProgramIntakeEntrySemester>(), intake.EntryLevelIds, createdIntake.Id);
            }

            ////////////
            // UPDATE //
            ////////////

            var updateIntakes = existingIntakes.Where(x => intakes.Any(y => x.StartDate == y.StartDate));

            foreach (var intake in updateIntakes)
            {
                if (intake.State == State.Inactive)
                    await domesticContext.ActivateProgramIntake(intake.Id);

                var newIntake = intakes.First(x => x.StartDate == intake.StartDate);

                dtoMapper.PatchProgramIntakeBase(intake, newIntake, program, modifiedBy);

                await domesticContext.UpdateProgramIntake(intake);

                var existingIntakeEntryLevels = await domesticContext.GetProgramIntakeEntrySemesters(new Domestic.Models.GetProgramIntakeEntrySemesterOptions
                {
                    ProgramIntakeId = intake.Id
                });

                await MergeIntakeEntryLevels(domesticContext, existingIntakeEntryLevels, newIntake.EntryLevelIds, intake.Id);
            }
        }

        public static async Task MergeIntakeEntryLevels(IDomesticContext domesticContext, IList<Domestic.Models.ProgramIntakeEntrySemester> existingEntryLevels, IList<Guid> entryLevelIds, Guid intakeId)
        {
            entryLevelIds = entryLevelIds ?? new List<Guid>();

            ////////////
            // DELETE //
            ////////////

            var removedEntryLevels = existingEntryLevels.Where(x => entryLevelIds.All(y => y != x.EntrySemesterId));

            foreach (var entryLevel in removedEntryLevels)
            {
                await domesticContext.DeleteProgramIntakeEntrySemester(entryLevel.Id);
            }

            ////////////
            // CREATE //
            ////////////

            var newEntryLevels = entryLevelIds.Where(x => existingEntryLevels.All(y => x != y.EntrySemesterId));

            foreach (var entryLevelId in newEntryLevels)
            {
                await domesticContext.CreateProgramIntakeEntrySemester(new Domestic.Models.ProgramIntakeEntrySemesterBase
                {
                    EntrySemesterId = entryLevelId,
                    ProgramIntakeId = intakeId
                });
            }

            // No update since I'm assuming we don't deactivate entry levels
        }
    }
}
