using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<CustomAuditDetail> GetCustomAuditDetail(Guid id)
        {
            return CrmExtrasProvider.GetCustomAuditDetail(id);
        }

        public Task<IList<CustomAuditDetail>> GetCustomAuditDetails(Guid auditId)
        {
            return CrmExtrasProvider.GetCustomAuditDetails(auditId);
        }
    }
}
