using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Apply.TestFramework.Models;

namespace Ocas.Domestic.Apply.Admin.IntegrationTests
{
    public class IdentityUserFixture : IdentityUserFixtureBase
    {
        private const int ExpiryMinutes = 5;
        private static readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);
        private static readonly JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();

        public TestUser OcasCccUser { get; set; }
        public TestUser OcasBoUser { get; set; }

        public TestUser OcasNoAccessUser { get; set; }

        public TestUser CollegeUserSene { get; set; }

        public TestUser ActonHsUser { get; set; }

        public TestUser HaltonBoardUser { get; set; }

        public async Task InitializeAsync()
        {
            await _syncLock.WaitAsync();
            try
            {
                OcasCccUser = OcasCccUser ?? await GetOcasUser(TestConstants.Identity.Providers.OcasAdfs.QaCccUsername, TestConstants.Identity.Providers.OcasAdfs.QaCccPassword);
                OcasBoUser = OcasBoUser ?? await GetOcasUser(TestConstants.Identity.Providers.OcasAdfs.QaBoUsername, TestConstants.Identity.Providers.OcasAdfs.QaBoPassword);
                OcasNoAccessUser = OcasNoAccessUser ?? await GetOcasUser(TestConstants.Identity.Providers.OcasAdfs.QaNoAccessUsername, TestConstants.Identity.Providers.OcasAdfs.QaNoAccessPassword);

                CollegeUserSene = CollegeUserSene ?? await GetPartnerUser(TestConstants.Identity.Providers.PartnerIdp.SeneTestUsername, TestConstants.Identity.Providers.PartnerIdp.SeneTestPassword);
                ActonHsUser = ActonHsUser ?? await GetPartnerUser(TestConstants.Identity.Providers.PartnerIdp.ActonHsUsername, TestConstants.Identity.Providers.PartnerIdp.ActonHsPassword);
                HaltonBoardUser = HaltonBoardUser ?? await GetPartnerUser(TestConstants.Identity.Providers.PartnerIdp.HaltonBoardUsername, TestConstants.Identity.Providers.PartnerIdp.HaltonBoardPassword);

                var jsonToken = _handler.ReadToken(OcasCccUser.AccessToken) as JwtSecurityToken;
                if (jsonToken.ValidTo < DateTime.UtcNow.AddMinutes(ExpiryMinutes))
                {
                    OcasCccUser = await GetOcasUser(TestConstants.Identity.Providers.OcasAdfs.QaCccUsername, TestConstants.Identity.Providers.OcasAdfs.QaCccPassword);
                }

                jsonToken = _handler.ReadToken(OcasBoUser.AccessToken) as JwtSecurityToken;
                if (jsonToken.ValidTo < DateTime.UtcNow.AddMinutes(ExpiryMinutes))
                {
                    OcasCccUser = await GetOcasUser(TestConstants.Identity.Providers.OcasAdfs.QaBoUsername, TestConstants.Identity.Providers.OcasAdfs.QaBoPassword);
                }

                jsonToken = _handler.ReadToken(OcasNoAccessUser.AccessToken) as JwtSecurityToken;
                if (jsonToken.ValidTo < DateTime.UtcNow.AddMinutes(ExpiryMinutes))
                {
                    OcasNoAccessUser = await GetOcasUser(TestConstants.Identity.Providers.OcasAdfs.QaNoAccessUsername, TestConstants.Identity.Providers.OcasAdfs.QaNoAccessPassword);
                }

                jsonToken = _handler.ReadToken(CollegeUserSene.AccessToken) as JwtSecurityToken;
                if (jsonToken.ValidTo < DateTime.UtcNow.AddMinutes(ExpiryMinutes))
                {
                    CollegeUserSene = await GetPartnerUser(TestConstants.Identity.Providers.PartnerIdp.SeneTestUsername, TestConstants.Identity.Providers.PartnerIdp.SeneTestPassword);
                }

                jsonToken = _handler.ReadToken(ActonHsUser.AccessToken) as JwtSecurityToken;
                if (jsonToken.ValidTo < DateTime.UtcNow.AddMinutes(ExpiryMinutes))
                {
                    ActonHsUser = await GetPartnerUser(TestConstants.Identity.Providers.PartnerIdp.ActonHsUsername, TestConstants.Identity.Providers.PartnerIdp.ActonHsPassword);
                }

                jsonToken = _handler.ReadToken(HaltonBoardUser.AccessToken) as JwtSecurityToken;
                if (jsonToken.ValidTo < DateTime.UtcNow.AddMinutes(ExpiryMinutes))
                {
                    HaltonBoardUser = await GetPartnerUser(TestConstants.Identity.Providers.PartnerIdp.HaltonBoardUsername, TestConstants.Identity.Providers.PartnerIdp.HaltonBoardPassword);
                }
            }
            finally
            {
                _syncLock.Release();
            }
        }

        private static Task<TestUser> GetOcasUser(string username, string password)
        {
            return GetOcasUser(username, password, IdentityConstants.Admin.Client, IdentityConstants.Admin.OcasScope);
        }

        private static Task<TestUser> GetPartnerUser(string username, string password)
        {
            return GetPartnerUser(username, password, IdentityConstants.Admin.Client, IdentityConstants.Admin.OcasScope);
        }
    }
}
