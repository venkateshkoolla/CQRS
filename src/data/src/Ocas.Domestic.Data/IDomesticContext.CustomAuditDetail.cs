using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<CustomAuditDetail> GetCustomAuditDetail(Guid id);
        Task<IList<CustomAuditDetail>> GetCustomAuditDetails(Guid auditId);
    }
}
