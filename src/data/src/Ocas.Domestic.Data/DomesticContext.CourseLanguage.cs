using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<CourseLanguage> GetCourseLanguage(Guid courseLanguageId, Locale locale);
        Task<IList<CourseLanguage>> GetCourseLanguages(Locale locale);
    }
}
