using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public async Task<Program> GetProgram(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramsGet.Id, id }
            };

            var programDictionary = new Dictionary<Guid, Program>();

            await Connection.QueryAsync<Program, Guid?, Program>(
                Sprocs.ProgramsGet.Sproc,
                (program, entryLevelId) =>
                {
                    if (!programDictionary.TryGetValue(program.Id, out var programEntry))
                    {
                        programEntry = program;
                        programEntry.EntryLevels = new List<Guid>();
                        programDictionary.Add(programEntry.Id, programEntry);
                    }

                    if (entryLevelId.HasValue)
                        programEntry.EntryLevels.Add(entryLevelId.Value);

                    return programEntry;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "EntryLevelId");

            return programDictionary.Values.FirstOrDefault();
        }

        public Task<IList<ProgramApplication>> GetProgramApplications(GetProgramApplicationsOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramsGetApplications.ProgramId, options.ProgramId },
                { Sprocs.ProgramsGetApplications.IntakeId, options.IntakeId },
                { Sprocs.ProgramsGetApplications.ApplicationStatusId, options.ApplicationStatusId },
                { Sprocs.ProgramsGetApplications.StateCode, options.State }
            };

            return Connection.QueryAsync<ProgramApplication>(Sprocs.ProgramsGetApplications.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public async Task<IList<Program>> GetPrograms(GetProgramsOptions programOptions)
        {
            var title = string.IsNullOrWhiteSpace(programOptions.Title) ? null : $"%{programOptions.Title}%";

            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramsGet.ApplicationCycleId, programOptions.ApplicationCycleId },
                { Sprocs.ProgramsGet.CollegeId, programOptions.CollegeId },
                { Sprocs.ProgramsGet.CampusId, programOptions.CampusId },
                { Sprocs.ProgramsGet.DeliveryId, programOptions.DeliveryId },
                { Sprocs.ProgramsGet.Code, programOptions.Code },
                { Sprocs.ProgramsGet.Title, title },
                { Sprocs.ProgramsGet.StateCode, (int)programOptions.StateCode }
            };

            var programDictionary = new Dictionary<Guid, Program>();

            await Connection.QueryAsync<Program, Guid?, Program>(
                Sprocs.ProgramsGet.Sproc,
                (program, entryLevelId) =>
                {
                    if (!programDictionary.TryGetValue(program.Id, out var programEntry))
                    {
                        programEntry = program;
                        programEntry.EntryLevels = new List<Guid>();
                        programDictionary.Add(programEntry.Id, programEntry);
                    }

                    if (entryLevelId.HasValue)
                        programEntry.EntryLevels.Add(entryLevelId.Value);

                    return programEntry;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "EntryLevelId");

            return programDictionary.Values.ToList();
        }
    }
}
