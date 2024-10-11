using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Ocas.Domestic.Apply.TestFramework.Models;
using Ocas.Identity.Provider.Clients;

namespace Ocas.Domestic.Apply.TestFramework
{
    public abstract class IdentityUserFixtureBase
    {
        private static readonly DiscoveryCache _discoveryCache = new DiscoveryCache(IdentityConstants.AuthBaseUri + "/.well-known/openid-configuration");

        public static async Task<TestUser> GetApplicantUser(string username, string password, string clientId = IdentityConstants.Apply.Client, string scope = IdentityConstants.Apply.ApplicantScope)
        {
            var discoveryDocument = await _discoveryCache.GetAsync();

            using (var client = new OcasIdentityProviderClient(IdentityConstants.AuthBaseUri, IdentityConstants.AccountBaseUri, clientId, IdentityConstants.RedirectUri, scope, Provider.Local))
            {
                var response = await client.SignInAsync(username, password);

                // get tokens
                var idToken = response.IdentityToken;
                var accessToken = response.AccessToken;

                // get user details
                using (var httpClient = new HttpClient())
                {
                    var userInfoResponse = await httpClient.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = discoveryDocument.UserInfoEndpoint,
                        Token = accessToken
                    });

                    var sub = userInfoResponse.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
                    var name = userInfoResponse.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
                    var email = userInfoResponse.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

                    return new TestUser(sub, idToken, accessToken, name, email);
                }
            }
        }

        public static async Task<TestUser> GetOcasUser(string username, string password, string clientId = IdentityConstants.Apply.Client, string scope = IdentityConstants.Apply.OcasScope)
        {
            var discoveryDocument = await _discoveryCache.GetAsync();

            using (var client = new OcasIdentityProviderClient(IdentityConstants.AuthBaseUri, IdentityConstants.AccountBaseUri, clientId, IdentityConstants.RedirectUri, scope, Provider.Adfs))
            {
                var response = await client.SignInAsync(username, password);

                // get tokens
                var idToken = response.IdentityToken;
                var accessToken = response.AccessToken;

                // get user details
                using (var httpClient = new HttpClient())
                {
                    var userInfoResponse = await httpClient.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = discoveryDocument.UserInfoEndpoint,
                        Token = accessToken
                    });

                    var sub = userInfoResponse.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
                    var name = userInfoResponse.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
                    var email = userInfoResponse.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

                    return new TestUser(sub, idToken, accessToken, name, email);
                }
            }
        }

        public static async Task<TestUser> GetPartnerUser(string username, string password, string clientId = IdentityConstants.Apply.Client, string scope = IdentityConstants.Apply.OcasScope)
        {
            var discoveryDocument = await _discoveryCache.GetAsync();

            using (var client = new OcasIdentityProviderClient(IdentityConstants.AuthBaseUri, IdentityConstants.AccountBaseUri, clientId, IdentityConstants.RedirectUri, scope, Provider.PartnerIdp))
            {
                var response = await client.SignInAsync(username, password);

                // get tokens
                var idToken = response.IdentityToken;
                var accessToken = response.AccessToken;

                // get user details
                using (var httpClient = new HttpClient())
                {
                    var userInfoResponse = await httpClient.GetUserInfoAsync(new UserInfoRequest
                    {
                        Address = discoveryDocument.UserInfoEndpoint,
                        Token = accessToken
                    });

                    var sub = userInfoResponse.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
                    var name = userInfoResponse.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
                    var email = userInfoResponse.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

                    return new TestUser(sub, idToken, accessToken, name, email);
                }
            }
        }
    }
}
