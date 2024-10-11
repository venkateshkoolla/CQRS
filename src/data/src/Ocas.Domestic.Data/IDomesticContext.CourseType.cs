using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<CourseType> GetCourseType(Guid courseTypeId, Locale locale);
        Task<IList<CourseType>> GetCourseTypes(Locale locale);
    }
}