using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<CourseStatus> GetCourseStatus(Guid courseStatusId, Locale locale)
        {
            return CrmExtrasProvider.GetCourseStatus(courseStatusId, locale);
        }

        public Task<IList<CourseStatus>> GetCourseStatuses(Locale locale)
        {
            return CrmExtrasProvider.GetCourseStatuses(locale);
        }
    }
}
