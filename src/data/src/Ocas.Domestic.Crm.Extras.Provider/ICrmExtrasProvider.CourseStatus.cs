using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial interface ICrmExtrasProvider
    {
        Task<IList<CourseStatus>> GetCourseStatuses(Locale locale);
        Task<CourseStatus> GetCourseStatus(Guid id, Locale locale);
    }
}
