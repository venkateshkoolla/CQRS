using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<PagedResult<ApplicantBrief>> GetApplicantBriefs(GetApplicantBriefOptions options, UserType userType, string partnerCode);
    }
}
