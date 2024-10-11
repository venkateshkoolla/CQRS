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
        public Task<IList<OfferStudyMethod>> GetOfferStudyMethods(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfferStudyMethodsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<OfferStudyMethod>(Sprocs.OfferStudyMethodsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<OfferStudyMethod> GetOfferStudyMethod(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.OfferStudyMethodsGet.Id, id },
                { Sprocs.OfferStudyMethodsGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<OfferStudyMethod>(Sprocs.OfferStudyMethodsGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
