using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<MinistryApproval> GetMinistryApproval(Guid ministryApprovalId, Locale locale)
        {
            return CrmExtrasProvider.GetMinistryApproval(ministryApprovalId, locale);
        }

        public Task<IList<MinistryApproval>> GetMinistryApprovals(Locale locale)
        {
            return CrmExtrasProvider.GetMinistryApprovals(locale);
        }
    }
}
