using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<PagedResult<ApplicantBrief>> GetApplicantBriefs(GetApplicantBriefOptions options, UserType userType, string partnerCode);
    }
}
