﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<CourseStatus> GetCourseStatus(Guid courseStatusId, Locale locale);
        Task<IList<CourseStatus>> GetCourseStatuses(Locale locale);
    }
}
