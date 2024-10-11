﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<PreferredCorrespondenceMethod> GetPreferredCorrespondenceMethod(Guid preferredCorrespondenceMethodId, Locale locale);
        Task<IList<PreferredCorrespondenceMethod>> GetPreferredCorrespondenceMethods(Locale locale);
    }
}
