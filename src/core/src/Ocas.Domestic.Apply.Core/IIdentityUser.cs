using System.Security.Principal;

namespace Ocas.Domestic.Apply
{
    public interface IIdentityUser
    {
        IPrincipal User { get; }
    }
}
