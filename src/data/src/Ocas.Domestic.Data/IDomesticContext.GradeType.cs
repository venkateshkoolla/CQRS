﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<GradeType> GetGradeType(Guid gradeTypeId, Locale locale);
        Task<IList<GradeType>> GetGradeTypes(Locale locale);
    }
}