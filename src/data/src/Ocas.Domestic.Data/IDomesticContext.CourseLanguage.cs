using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<CourseLanguage> GetCourseLanguage(Guid courseLanguageId, Locale locale)
        {
            return CrmExtrasProvider.GetCourseLanguage(courseLanguageId, locale);
        }

        public Task<IList<CourseLanguage>> GetCourseLanguages(Locale locale)
        {
            return CrmExtrasProvider.GetCourseLanguages(locale);
        }
    }
}
