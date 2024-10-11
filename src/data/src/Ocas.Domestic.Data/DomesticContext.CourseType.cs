using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<CourseType> GetCourseType(Guid courseTypeId, Locale locale)
        {
            return CrmExtrasProvider.GetCourseType(courseTypeId, locale);
        }

        public Task<IList<CourseType>> GetCourseTypes(Locale locale)
        {
            return CrmExtrasProvider.GetCourseTypes(locale);
        }
    }
}
