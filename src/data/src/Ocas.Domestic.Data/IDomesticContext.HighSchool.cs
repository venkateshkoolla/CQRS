﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<HighSchool> GetHighSchool(Guid highSchoolId, Locale locale);
        Task<IList<HighSchool>> GetHighSchools(Locale locale);
    }
}
