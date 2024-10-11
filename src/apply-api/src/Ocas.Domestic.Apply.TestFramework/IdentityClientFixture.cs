using System.Threading.Tasks;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Identity.Provider.Clients;

namespace Ocas.Domestic.Apply.TestFramework
{
    public static class IdentityClientFixture
    {
        public static async Task CreateApplicant(Applicant applicant)
        {
            var user = new NewUserInfo
            {
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                DateOfBirth = applicant.BirthDate.ToDateTime(),
                Password = IdentityConstants.ValidPassword
            };

            using (var client = new OcasIdentityProviderClient(IdentityConstants.AuthBaseUri, IdentityConstants.AccountBaseUri, IdentityConstants.Apply.Client, IdentityConstants.RedirectUri, IdentityConstants.Apply.ApplicantScope, Provider.Local))
            {
                await client.RegisterAsync(user);
            }
        }
    }
}
