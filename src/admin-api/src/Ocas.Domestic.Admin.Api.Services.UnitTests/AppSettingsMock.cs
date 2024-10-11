using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Apply.Admin.Core.Settings;

namespace Ocas.Domestic.Apply.Admin.Api.Services.UnitTests
{
    public class AppSettingsMock : IAppSettings
    {
        public string ApplicationName => string.Empty;

        public string Environment => "tests";

        public string AppSettingsBaseUrl => string.Empty;

        public string IdSvrAuthority => string.Empty;

        public string IdSvrAppSettingsClientId => string.Empty;

        public string IdSvrAppSettingsClientSecret => string.Empty;

        public IList<string> IdSvrRolesPartnerHighSchoolUser => new List<string> { "UAT_PartnerSchoolAdministrators", "UAT_PartnerSchoolUsers", "UAT_PartnerSchoolReadOnly" };

        public IList<string> IdSvrRolesPartnerHSBoardUser => new List<string> { "UAT_PartnerBoardAdministrators", "UAT_PartnerBoardUsers", "UAT_PartnerBoardReadOnly" };

        public IList<string> IdSvrRolesOcasUser => new List<string> { "PortalAdmin", "BO", "CCC" };

        public string IdSvrAppSettingsScope => string.Empty;

        public IList<string> IdSvrRolesPartnerCollegeUser => new List<string> { "UAT_PartnerCollegeAdministrators", "UAT_PartnerCollegeUsers", "UAT_PartnerCollegeReadOnly" };

        public string GetAppSetting(string key)
        {
            return default(string);
        }

        public T GetAppSetting<T>(string key)
        {
            return default(T);
        }

        public string GetAppSettingOrDefault(string key, string defaultValue)
        {
            return defaultValue;
        }

        public T GetAppSettingOrDefault<T>(string key, T defaultValue)
        {
            return defaultValue;
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}