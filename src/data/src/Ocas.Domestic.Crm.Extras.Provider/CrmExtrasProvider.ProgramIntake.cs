using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Crm.Extras.Provider.Extensions;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public async Task<ProgramIntake> GetProgramIntake(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramIntakesGet.Ids, id.UniqueIdentifierListParameter() }
            };

            var intakeDictionary = new Dictionary<Guid, ProgramIntake>();

            await Connection.QueryAsync<ProgramIntake, Guid?, ProgramIntake>(
                Sprocs.ProgramIntakesGet.Sproc,
                (intake, entryLevelId) =>
                {
                    if (!intakeDictionary.TryGetValue(intake.Id, out var intakeEntry))
                    {
                        intakeEntry = intake;
                        intakeEntry.EntryLevels = new List<Guid>();
                        intakeDictionary.Add(intakeEntry.Id, intakeEntry);
                    }

                    if (entryLevelId.HasValue)
                        intakeEntry.EntryLevels.Add(entryLevelId.Value);

                    return intakeEntry;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "EntryLevelId");

            return intakeDictionary.Values.FirstOrDefault();
        }

        public async Task<IList<ProgramIntake>> GetProgramIntakes(GetProgramIntakeOptions options)
        {
            var programTitle = string.IsNullOrWhiteSpace(options.ProgramTitle) ? null : $"%{options.ProgramTitle}%";

            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramIntakesGet.Ids, options.Ids.UniqueIdentifierListParameter() },
                { Sprocs.ProgramIntakesGet.StateCode, (int?)options.StateCode },
                { Sprocs.ProgramIntakesGet.StatusCode, (int?)options.StatusCode },
                { Sprocs.ProgramIntakesGet.ApplicationCycleId, options.ApplicationCycleId },
                { Sprocs.ProgramIntakesGet.CollegeId, options.CollegeId },
                { Sprocs.ProgramIntakesGet.CampusId, options.CampusId },
                { Sprocs.ProgramIntakesGet.ProgramCode, options.ProgramCode },
                { Sprocs.ProgramIntakesGet.ProgramTitle, programTitle },
                { Sprocs.ProgramIntakesGet.FromDate, options.FromDate },
                { Sprocs.ProgramIntakesGet.ProgramDeliveryId, options.ProgramDeliveryId },
                { Sprocs.ProgramIntakesGet.ProgramId, options.ProgramId }
            };

            var intakeDictionary = new Dictionary<Guid, ProgramIntake>();

            await Connection.QueryAsync<ProgramIntake, Guid?, ProgramIntake>(
                Sprocs.ProgramIntakesGet.Sproc,
                (intake, entryLevelId) =>
                {
                    if (!intakeDictionary.TryGetValue(intake.Id, out var intakeEntry))
                    {
                        intakeEntry = intake;
                        intakeEntry.EntryLevels = new List<Guid>();
                        intakeDictionary.Add(intakeEntry.Id, intakeEntry);
                    }

                    if (entryLevelId.HasValue)
                        intakeEntry.EntryLevels.Add(entryLevelId.Value);

                    return intakeEntry;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "EntryLevelId");

            return intakeDictionary.Values.ToList();
        }

        public async Task<IList<ProgramIntake>> GetProgramIntakes(Guid programId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramIntakesGet.ProgramId, programId }
            };

            var intakeDictionary = new Dictionary<Guid, ProgramIntake>();

            await Connection.QueryAsync<ProgramIntake, Guid?, ProgramIntake>(
                Sprocs.ProgramIntakesGet.Sproc,
                (intake, entryLevelId) =>
                {
                    if (!intakeDictionary.TryGetValue(intake.Id, out var intakeEntry))
                    {
                        intakeEntry = intake;
                        intakeEntry.EntryLevels = new List<Guid>();
                        intakeDictionary.Add(intakeEntry.Id, intakeEntry);
                    }

                    if (entryLevelId.HasValue)
                        intakeEntry.EntryLevels.Add(entryLevelId.Value);

                    return intakeEntry;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "EntryLevelId");

            return intakeDictionary.Values.ToList();
        }
    }
}
