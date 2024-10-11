using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<CourseDelivery> GetCourseDelivery(Guid courseDeliveryId, Locale locale)
        {
            return CrmExtrasProvider.GetCourseDelivery(courseDeliveryId, locale);
        }

        public Task<IList<CourseDelivery>> GetCourseDeliveries(Locale locale)
        {
            return CrmExtrasProvider.GetCourseDeliveries(locale);
        }
    }
}
