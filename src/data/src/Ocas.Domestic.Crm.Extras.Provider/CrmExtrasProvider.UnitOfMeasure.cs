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
        public Task<IList<UnitOfMeasure>> GetUnitOfMeasures(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.UnitOfMeasuresGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<UnitOfMeasure>(Sprocs.UnitOfMeasuresGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<UnitOfMeasure> GetUnitOfMeasure(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.UnitOfMeasuresGet.Id, id },
                { Sprocs.UnitOfMeasuresGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<UnitOfMeasure>(Sprocs.UnitOfMeasuresGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
