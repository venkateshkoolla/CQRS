using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.TestFramework;
using Ocas.Domestic.Apply.TestFramework.Models;

namespace Ocas.Domestic.Apply.IntegrationTests
{
    public class IdentityUserFixture : IdentityUserFixtureBase
    {
        private const int ExpiryMinutes = 5;
        private static readonly SemaphoreSlim _syncLock = new SemaphoreSlim(1, 1);
        private static readonly JwtSecurityTokenHandler _handler = new JwtSecurityTokenHandler();

        public TestUser OcasCccUser { get; set; }
        public TestUser OcasNoAccessUser { get; set; }
        public TestUser Applicant { get; set; }

        public async Task InitializeAsync()
        {
            await _syncLock.WaitAsync();
            try
            {
                OcasCccUser = OcasCccUser ?? await GetOcasUser(TestConstants.Identity.Providers.OcasAdfs.QaCccUsername, TestConstants.Identity.Providers.OcasAdfs.QaCccPassword);
                OcasNoAccessUser = OcasNoAccessUser ?? await GetOcasUser(TestConstants.Identity.Providers.OcasAdfs.QaNoAccessUsername, TestConstants.Identity.Providers.OcasAdfs.QaNoAccessPassword);
                Applicant = Applicant ?? await GetApplicantUser(TestConstants.Identity.Providers.OcasApplicants.ApplicantTestUsername, TestConstants.Identity.Providers.OcasApplicants.ApplicantTestUsernamePw);

                var jsonToken = _handler.ReadToken(OcasCccUser.AccessToken) as JwtSecurityToken;
                if (jsonToken.ValidTo < DateTime.UtcNow.AddMinutes(ExpiryMinutes))
                {
                    OcasCccUser = await GetOcasUser(TestConstants.Identity.Providers.OcasAdfs.QaCccUsername, TestConstants.Identity.Providers.OcasAdfs.QaCccPassword);
                }

                jsonToken = _handler.ReadToken(OcasNoAccessUser.AccessToken) as JwtSecurityToken;
                if (jsonToken.ValidTo < DateTime.UtcNow.AddMinutes(ExpiryMinutes))
                {
                    OcasNoAccessUser = await GetOcasUser(TestConstants.Identity.Providers.OcasAdfs.QaNoAccessUsername, TestConstants.Identity.Providers.OcasAdfs.QaNoAccessPassword);
                }

                jsonToken = _handler.ReadToken(Applicant.AccessToken) as JwtSecurityToken;
                if (jsonToken.ValidTo < DateTime.UtcNow.AddMinutes(ExpiryMinutes))
                {
                    Applicant = await GetApplicantUser(TestConstants.Identity.Providers.OcasApplicants.ApplicantTestUsername, TestConstants.Identity.Providers.OcasApplicants.ApplicantTestUsernamePw);
                }
            }
            finally
            {
                _syncLock.Release();
            }
        }
    }
}
