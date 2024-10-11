using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<CommunityInvolvement> GetCommunityInvolvement(Guid communityInvolvementId, Locale locale)
        {
            return CrmExtrasProvider.GetCommunityInvolvement(communityInvolvementId, locale);
        }

        public Task<IList<CommunityInvolvement>> GetCommunityInvolvements(Locale locale)
        {
            return CrmExtrasProvider.GetCommunityInvolvements(locale);
        }
    }
}