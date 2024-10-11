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
        public Task<AcademicRecord> GetAcademicRecord(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AcademicRecordGet.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<AcademicRecord>(Sprocs.AcademicRecordGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<IList<AcademicRecord>> GetAcademicRecords(Guid applicantId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.AcademicRecordGet.ApplicantId, applicantId }
            };

            return Connection.QueryAsync<AcademicRecord>(Sprocs.AcademicRecordGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
