using System.Collections.Generic;
using Ocas.Domestic.Apply.Settings;

namespace Ocas.Domestic.Apply.Admin.Core.Settings
{
    public interface IAppSettings : IAppSettingsBase
    {
        IList<string> IdSvrRolesPartnerCollegeUser { get; }
        IList<string> IdSvrRolesPartnerHighSchoolUser { get; }
        IList<string> IdSvrRolesPartnerHSBoardUser { get; }
    }
}
