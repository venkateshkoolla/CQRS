using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Admin.Api.Services
{
    public interface IUserAuthorization : IUserAuthorizationBase
    {
        Task CanAccessCollegeAsync(IPrincipal user, Guid collegeId);
        UserType GetUserType(IPrincipal user);
        bool IsHighSchoolUser(IPrincipal user);
        bool IsOcasTier2User(IPrincipal user);
    }
}
