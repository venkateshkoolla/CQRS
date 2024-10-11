using System;

namespace Ocas.Domestic.AppSettings.Extras
{
    public interface IAppSettingsExtras
    {
        DateTime GetApplicationCycleEqualConsiderationDate(int year);
        DateTime? GetNextSendDate(DateTime utcMaxLock);
    }
}
