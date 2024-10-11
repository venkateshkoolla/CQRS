using System;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Ocas.Domestic.Apply
{
    public interface IUserAuthorizationBase
    {
        Task CanAccessApplicantAsync(IPrincipal user, Guid applicantId, bool skipActiveCheck = false);
        bool IsOcasUser(IPrincipal user);
    }
}
