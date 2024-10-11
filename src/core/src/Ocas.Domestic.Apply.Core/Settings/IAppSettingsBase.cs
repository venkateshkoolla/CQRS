using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ocas.Domestic.Apply.Settings
{
    public interface IAppSettingsBase
    {
        string ApplicationName { get; }
        string Environment { get; }
        string AppSettingsBaseUrl { get; }
        string IdSvrAuthority { get; }
        string IdSvrAppSettingsClientId { get; }
        string IdSvrAppSettingsClientSecret { get; }
        string IdSvrAppSettingsScope { get; }
        IList<string> IdSvrRolesOcasUser { get; }

        string GetAppSetting(string key);
        T GetAppSetting<T>(string key);
        string GetAppSettingOrDefault(string key, string defaultValue);
        T GetAppSettingOrDefault<T>(string key, T defaultValue);
        Task InitializeAsync();
    }
}
