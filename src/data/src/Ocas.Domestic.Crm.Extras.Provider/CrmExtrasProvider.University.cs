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
        public Task<IList<University>> GetUniversities()
        {
            return Connection.QueryAsync<University, Address, University>(
                Sprocs.UniversitiesGet.Sproc,
                (university, address) =>
                {
                    university.MailingAddress = address;
                    return university;
                },
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "MailingAddressSplit").QueryToListAsync();
        }

        public async Task<University> GetUniversity(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.UniversitiesGet.Id, id },
                { Sprocs.UniversitiesGet.StateCode, null }
            };

            var result = await Connection.QueryAsync<University, Address, University>(
                Sprocs.UniversitiesGet.Sproc,
                (university, address) =>
                {
                    university.MailingAddress = address;
                    return university;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "MailingAddressSplit");

            return result.FirstOrDefault();
        }
    }
}
