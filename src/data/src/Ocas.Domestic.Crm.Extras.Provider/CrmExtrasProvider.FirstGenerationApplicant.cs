using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<FirstGenerationApplicant>> GetFirstGenerationApplicants(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.FirstGenerationApplicantsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<FirstGenerationApplicant>(Sprocs.FirstGenerationApplicantsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<FirstGenerationApplicant> GetFirstGenerationApplicant(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.FirstGenerationApplicantsGet.Id, id },
                { Sprocs.FirstGenerationApplicantsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<FirstGenerationApplicant>(Sprocs.FirstGenerationApplicantsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
