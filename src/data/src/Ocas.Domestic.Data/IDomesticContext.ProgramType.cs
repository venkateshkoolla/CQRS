﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<ProgramType> GetProgramType(Guid programTypeId, Locale locale);
        Task<IList<ProgramType>> GetProgramTypes(Locale locale);
    }
}
