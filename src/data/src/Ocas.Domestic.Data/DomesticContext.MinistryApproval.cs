using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<MinistryApproval> GetMinistryApproval(Guid ministryApprovalId, Locale locale);
        Task<IList<MinistryApproval>> GetMinistryApprovals(Locale locale);
    }
}
