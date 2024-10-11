using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<Voucher> GetVoucher(GetVoucherOptions options)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.VouchersGet.Id, options.Id },
                { Sprocs.VouchersGet.Code, options.Code },
                { Sprocs.VouchersGet.ApplicationId, options.ApplicationId },
                { Sprocs.VouchersGet.StateCode, options.State }
            };

            return Connection.QueryFirstOrDefaultAsync<Voucher>(Sprocs.VouchersGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
