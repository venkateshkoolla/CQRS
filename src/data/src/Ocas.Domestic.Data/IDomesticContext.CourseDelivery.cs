using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<CourseDelivery> GetCourseDelivery(Guid courseDeliveryId, Locale locale);

        Task<IList<CourseDelivery>> GetCourseDeliveries(Locale locale);
    }
}
