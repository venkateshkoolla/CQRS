using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Coltrane.Bds.Provider
{
    public partial class ColtraneBdsProvider
    {
        public async Task<IList<CollegeTransmission>> GetCollegeTransmissions(string applicationNumber, GetCollegeTransmissionHistoryOptions options = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ApplicationNumber", applicationNumber },
                { "FromDate", options?.FromDate },
                { "ToDate", options?.ToDate },
                { "TransactionType", options?.TransactionType },
                { "TransactionCode", options?.TransactionCode }
            };

            if (string.IsNullOrEmpty(applicationNumber)) return new List<CollegeTransmission>();

            var sql = @"SELECT DISTINCT AuditID as Id
                , Audit_ColtraneXC.TransactionCodeId
                , ColtraneXCID AS ColtraneXcId
                , CollegeCode
                , AccountNumber
                , ApplicationNumber
                , LastLoadDateTime
                , TransactionCode
                , TransactionType
                , Data
                , BusinessKey
                , Description
                FROM   Audit_ColtraneXC
                       JOIN PL_TransactionCode
                         ON Audit_ColtraneXC.TransactionCodeID =
                            PL_TransactionCode.TransactionCodeID
                            AND ActivityHistoryFlag = 1
                WHERE ApplicationNumber = @ApplicationNumber";

            if (options?.FromDate != null)
            {
                sql += " AND [LastLoadDateTime] >= @FromDate ";
            }

            if (options?.ToDate != null)
            {
                sql += " AND [LastLoadDateTime] < @ToDate ";
            }

            if (!string.IsNullOrWhiteSpace(options?.TransactionCode))
            {
                sql += " AND [PL_TransactionCode].[TransactionCode] = @TransactionCode ";
            }

            if (options?.TransactionType != null)
            {
                sql += " AND [Audit_ColtraneXC].[TransactionType] = @TransactionType ";
            }

            sql += " ORDER BY [Audit_ColtraneXC].[LastLoadDateTime] DESC";

            return await Connection.QueryAsync<CollegeTransmission>(sql, parameters, commandTimeout: _commandTimeout).QueryToListAsync();
        }
    }
}
