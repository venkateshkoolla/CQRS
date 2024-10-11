using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<HighSchool>> GetHighSchools(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.HighSchoolsGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<HighSchool, Address, HighSchool>(
                Sprocs.HighSchoolsGet.Sproc,
                (highschool, address) =>
                {
                    highschool.MailingAddress = address;
                    return highschool;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "MailingAddressSplit").QueryToListAsync();
        }

        public async Task<HighSchool> GetHighSchool(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.HighSchoolsGet.Id, id },
                { Sprocs.HighSchoolsGet.Locale, (int)locale },
                { Sprocs.HighSchoolsGet.StateCode, null }
            };

            var result = await Connection.QueryAsync<HighSchool, Address, HighSchool>(
                Sprocs.HighSchoolsGet.Sproc,
                (highschool, address) =>
                {
                    highschool.MailingAddress = address;
                    return highschool;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "MailingAddressSplit");

            return result.FirstOrDefault();
        }
    }
}
