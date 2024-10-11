using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<ProgramSpecialCode>> GetProgramSpecialCodes()
        {
            return Connection.QueryAsync<ProgramSpecialCode>(Sprocs.ProgramSpecialCodesGet.Sproc, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<IList<ProgramSpecialCode>> GetProgramSpecialCodes(Guid collegeApplicationId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramSpecialCodesGet.CollegeApplicationId, collegeApplicationId }
            };

            return Connection.QueryAsync<ProgramSpecialCode>(Sprocs.ProgramSpecialCodesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<ProgramSpecialCode> GetProgramSpecialCode(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ProgramSpecialCodesGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<ProgramSpecialCode>(Sprocs.ProgramSpecialCodesGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
