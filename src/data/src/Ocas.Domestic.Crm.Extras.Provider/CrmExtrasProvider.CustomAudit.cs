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
        public async Task<IList<CustomAudit>> GetCustomAudits(GetCustomAuditOptions options, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CustomAuditsGet.ApplicantId, options.ApplicantId },
                { Sprocs.CustomAuditsGet.ApplicationId, options.ApplicationId },
                { Sprocs.CustomAuditsGet.FromDate, options.FromDate },
                { Sprocs.CustomAuditsGet.ToDate, options.ToDate },
                { Sprocs.CustomAuditsGet.Locale, (int)locale }
            };

            // https://dapper-tutorial.net/result-multi-mapping#example-query-multi-mapping-one-to-many
            var resultDictionary = new Dictionary<Guid, CustomAudit>();

            await Connection.QueryAsync<CustomAudit, CustomAuditDetail, CustomAudit>(
                Sprocs.CustomAuditsGet.Sproc,
                (master, detail) =>
                {
                    if (!resultDictionary.TryGetValue(master.Id, out var tempMaster))
                    {
                        tempMaster = master;
                        tempMaster.Details = new List<CustomAuditDetail>();
                        resultDictionary.Add(tempMaster.Id, tempMaster);
                    }

                    if (detail != null)
                    {
                        tempMaster.Details.Add(detail);
                    }

                    return tempMaster;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout);

            return resultDictionary.Values.ToList();
        }

        public async Task<CustomAudit> GetCustomAudit(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CustomAuditsGet.Id, id },
                { Sprocs.CustomAuditsGet.Locale, (int)locale }
            };

            // https://dapper-tutorial.net/result-multi-mapping#example-query-multi-mapping-one-to-many
            var resultDictionary = new Dictionary<Guid, CustomAudit>();

            await Connection.QueryAsync<CustomAudit, CustomAuditDetail, CustomAudit>(
                Sprocs.CustomAuditsGet.Sproc,
                (master, detail) =>
                {
                    if (!resultDictionary.TryGetValue(master.Id, out var tempMaster))
                    {
                        tempMaster = master;
                        tempMaster.Details = new List<CustomAuditDetail>();
                        resultDictionary.Add(tempMaster.Id, tempMaster);
                    }

                    if (detail != null)
                    {
                        tempMaster.Details.Add(detail);
                    }

                    return tempMaster;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout);

            return resultDictionary.Values.SingleOrDefault();
        }
    }
}