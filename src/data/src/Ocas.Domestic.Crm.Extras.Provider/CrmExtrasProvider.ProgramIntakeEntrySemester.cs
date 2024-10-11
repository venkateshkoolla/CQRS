using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider : ICrmExtrasProvider
    {
        public Task<ProgramIntakeEntrySemester> GetProgramIntakeEntrySemester(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramIntakeEntrySemestersGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramIntakeEntrySemester>(Sprocs.ProgramIntakeEntrySemestersGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<ProgramIntakeEntrySemester>> GetProgramIntakeEntrySemesters(GetProgramIntakeEntrySemesterOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramIntakeEntrySemestersGet.StateCode, (int)options.StateCode },
                { Sprocs.ProgramIntakeEntrySemestersGet.StatusCode, (int)options.StatusCode },
                { Sprocs.ProgramIntakeEntrySemestersGet.ProgramIntakeId, options.ProgramIntakeId },
                { Sprocs.ProgramIntakeEntrySemestersGet.EntrySemesterId, options.EntrySemesterId }
            };

            return Connection.QueryAsync<ProgramIntakeEntrySemester>(Sprocs.ProgramIntakeEntrySemestersGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
